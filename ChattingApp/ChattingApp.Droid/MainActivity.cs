using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

using ChattingApp.Enums;
using ChattingApp.Events;
using ChattingApp.Models;

using Firebase.Xamarin.Auth;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Streaming;

using Prism.Unity;
using Microsoft.Practices.Unity;

using Newtonsoft.Json.Linq;

using Prism.Events;

using Xamarin.Auth;

using Android.Media;

namespace ChattingApp.Droid
{
    [Activity(Label = "Làm quen nhé", Icon = "@drawable/appicon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private FirebaseClient _firebaseClient;
        private FirebaseAuthProvider _firebaseAuthProvider;
        private IEventAggregator _eventAggregator;

        private string _currentChatroom;
        private UserModel _currentUser;

        private MediaPlayer _clickMedia;
        private MediaPlayer _findMedia;
        private MediaPlayer _loginMedia;
        private MediaPlayer _messageMedia;

        private IDisposable _messageObservable;
        protected override void OnCreate(Bundle bundle)
        {
            _currentChatroom = "";
            _currentUser = null;

            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;

            _clickMedia = MediaPlayer.Create(this, Resource.Raw.Click_Sound );
            _findMedia = MediaPlayer.Create ( this, Resource.Raw.Find_Sound);
            _loginMedia = MediaPlayer.Create(this, Resource.Raw.LoginSucess_Sound);
            _messageMedia = MediaPlayer.Create(this, Resource.Raw.NewMessage_Sound);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            InitFirebase ( );

            var app = new App(new AndroidInitializer());

            LoadApplication(app);

            _eventAggregator = app.GetEventAggerator();
            _eventAggregator.GetEvent < RequestLoginFacebookEvent > ( ).Subscribe ( LoginFb );
            _eventAggregator.GetEvent < SendEvent > ( ).Subscribe ( SendMessage );
            _eventAggregator.GetEvent < QuitChatEvent > ( ).Subscribe ( QuitChat );
            _eventAggregator.GetEvent < FindEvent > ( ).Subscribe ( StartFinding );
            _eventAggregator.GetEvent < StopFindingEvent > ( ).Subscribe ( StopFinding );
            _eventAggregator.GetEvent < PlayClickEvent > ( ).Subscribe ( PlayClick );
        }

        private void PlayClick ( )
        {
            _clickMedia.Start (  );
        }

        private async void StopFinding ( )
        {
            if (_currentUser != null)
            {
                _currentUser.State = StateEnum.Online;
                await _firebaseClient.Child($"users/{_currentUser.LocalId}").PutAsync(_currentUser);
            }
        }

        private async void StartFinding (GenderEnum gender )
        {
            if ( _currentChatroom != "" )
            {
                return;
            }
            if (_currentUser != null)
            {
                _currentUser.State = StateEnum.Finding;
                _currentUser.TargetGender = gender;
                await _firebaseClient.Child($"users/{_currentUser.LocalId}").PutAsync(_currentUser);
                _findMedia.Start (  );
            }
        }

        private async void QuitChat ( )
        {
            if (_currentUser != null)
            {
                _currentUser.State = StateEnum.Online;
                await _firebaseClient.Child($"users/{_currentUser.LocalId}").PutAsync(_currentUser);
                _messageObservable.Dispose (  );
                await _firebaseClient.Child ( $"chatrooms/{_currentChatroom}/IsAvailable" ).PutAsync ( false );
                await _firebaseClient.Child($"chatrooms/{_currentChatroom}").DeleteAsync();
                _currentChatroom = "";
            }
        }

        protected override async void OnDestroy ( )
        {
            if ( _currentUser != null )
            {
                _currentUser.State = StateEnum.Offline;
                await _firebaseClient.Child($"users/{_currentUser.LocalId}").PutAsync(_currentUser);
                _messageObservable.Dispose (  );
                //_roomObservable.Dispose (  );
            }
        }

        private async void SendMessage ( MessageModel obj )
        {
            if ( obj == null )
            {
                return;
            }
            if ( _currentChatroom == "" )
            {
                return;
            }

            obj.Sender = _currentUser.LocalId;

            if ( obj.MessageType == MessageTypeEnum.InfoApproved )
            {
                obj.Content = _currentUser.LinkFb;
            }
            else if ( obj.MessageType == MessageTypeEnum.InfoRequest )
            {
                obj.Content = "Your partner is asking for facebook";
            }

            await _firebaseClient.Child ( $"chatrooms/{_currentChatroom}/messages" ).PostAsync ( obj );
        }

        private void InitFirebase ( )
        {
            _firebaseClient = new FirebaseClient("https://chattingapp-3ca17.firebaseio.com/");
            _firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyASehOOaMbscFWErLri-3q5L8XzfFbB3l8"));
           
        }

        private void LoginFb ( )
        {
            var auth = new OAuth2Authenticator(
                "344825425959819", // your OAuth2 client id
                "", // the scopes for the particular API you're accessing, delimited by "+" symbols
                new Uri("https://m.facebook.com/dialog/oauth/"),
                new Uri("https://chattingapp-3ca17.firebaseapp.com/__/auth/handler"));

            auth.Completed += AuthOnCompleted;
            auth.Error += AuthOnError;

            this.StartActivity(auth.GetUI(this));
        }

        private void AuthOnError ( object sender, AuthenticatorErrorEventArgs authenticatorErrorEventArgs )
        {
            _eventAggregator.GetEvent<LoginFailEvent>().Publish();
        }

        private async void AuthOnCompleted ( object sender, AuthenticatorCompletedEventArgs authenticatorCompletedEventArgs )
        {
            if ( authenticatorCompletedEventArgs.IsAuthenticated )
            {
                AccountStore.Create ( this ).Save ( authenticatorCompletedEventArgs.Account, "Facebook" );
                var facebookAccessToken = authenticatorCompletedEventArgs.Account.Properties [ "access_token" ];

                var request = new OAuth2Request ( "GET",
                    new Uri ( "https://graph.facebook.com/me?fields=gender,age_range" ),
                    null, authenticatorCompletedEventArgs.Account );

                var response = await request.GetResponseAsync ( );
                var obj = JObject.Parse ( response.GetResponseText ( ) );

                var gender = obj [ "gender" ].ToString ( ).Replace ( "\"", "" );
                var age = int.Parse ( obj [ "age_range" ] [ "min" ].ToString ( ).Replace ( "\"", "" ) ) +
                          int.Parse ( obj [ "age_range" ] [ "max" ].ToString ( ).Replace ( "\"", "" ) );
                age /= 2;

                var auth = await _firebaseAuthProvider.SignInWithOAuthAsync ( FirebaseAuthType.Facebook,
                    facebookAccessToken );

                var user = new UserModel
                {
                    LocalId = auth.User.LocalId,
                    DisplayName = auth.User.DisplayName,
                    Email = auth.User.Email,
                    LinkFb = auth.User.FederatedId,
                    FirstName = auth.User.FirstName,
                    LastName = auth.User.LastName,
                    PhotoUrl = auth.User.PhotoUrl,
                    Age = age,
                    State = StateEnum.Online,
                    Gender = gender == "male" ? GenderEnum.Male : GenderEnum.Female
                };


                await _firebaseClient.Child ( $"users/{user.LocalId}" ).PutAsync ( user );

                _eventAggregator.GetEvent < LoginSuccessEvent > ( ).Publish ( user );
                _currentUser = user;

                _loginMedia.Start (  );

                _firebaseClient.Child("chatrooms").AsObservable<Chatroom>().Subscribe(OnRoomFound);
            }
            else
            {
                _eventAggregator.GetEvent<LoginFailEvent> (  ).Publish (  );
            }
        }

        private void OnRoomFound ( FirebaseEvent <Chatroom> chatroom)
        {
            if ( !chatroom.Object.IsAvailable )
            {
                return;
            }
            if(chatroom.EventType == FirebaseEventType.Delete)
            {
                return;
            }
            if (_currentChatroom != "")
            {
                return;
            }
            if (chatroom.Object.User1 == _currentUser.LocalId ||
                chatroom.Object.User2 == _currentUser.LocalId)
            {
                _currentChatroom = chatroom.Key;
                _eventAggregator.GetEvent<RoomFoundEvent>().Publish();
            }
            _messageObservable = _firebaseClient.Child($"chatrooms/{_currentChatroom}/messages").AsObservable<MessageModel>().Subscribe(
                message =>
                {
                    message.Object.Sender = message.Object.Sender == _currentUser.LocalId ? "Me" : "Other";
                    _eventAggregator.GetEvent<ReceiveEvent>().Publish(message.Object);
                    _messageMedia.Start();
                });
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }
}

