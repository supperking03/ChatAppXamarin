using ChattingApp.Modules.FacebookLogin;
using ChattingApp.Modules.MessageSending;

using Prism.Unity;
using ChattingApp.Views;

using Microsoft.Practices.Unity;

using Prism.Events;

using Xamarin.Forms;

namespace ChattingApp
{
    public partial class App : PrismApplication
    {
        public App ( IPlatformInitializer initializer = null ) : base ( initializer )
        {
            
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<MenuPage>();
            Container.RegisterTypeForNavigation<ChatPage>();
            Container.RegisterType ( typeof ( ILoginFacebook ), typeof ( LoginFacebookHandler ), null,
                new ContainerControlledLifetimeManager ( ),
                new InjectionConstructor ( Container.Resolve < IEventAggregator > ( ) ) );
            Container.RegisterType ( typeof ( IMessageSender ), typeof ( MessageSender ),
                new ContainerControlledLifetimeManager ( ),
                new InjectionConstructor ( Container.Resolve < IEventAggregator > ( ) ) );
            Container.RegisterTypeForNavigation<SearchPage>();
        }

        public IEventAggregator GetEventAggerator ( )
        {
            return Container.Resolve < IEventAggregator > ( );
        }
    }
}
