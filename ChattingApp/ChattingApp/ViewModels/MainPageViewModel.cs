using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using ChattingApp.Events;
using ChattingApp.Models;
using ChattingApp.Modules.FacebookLogin;

using Prism.Events;

namespace ChattingApp.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private bool _isLoggingIn;
        private UserModel _user;

        private readonly INavigationService _navigationService;
        private readonly ILoginFacebook _facebookLoginHandler;
        private readonly IEventAggregator _eventAggregator;

        public bool IsLoggingIn
        {
            get
            {
                return _isLoggingIn;
            }
            set
            {
                SetProperty ( ref _isLoggingIn, value );
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand ( LoginFacebook );
            }
        }

        private void LoginFacebook ( )
        {
            if (!IsLoggingIn )
            {
                IsLoggingIn = true;
                _facebookLoginHandler.LoginFacebook();
            }
        }

        public MainPageViewModel(ILoginFacebook facebookLoginHandler, INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _isLoggingIn = false;

            _facebookLoginHandler = facebookLoginHandler;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<RequestLoginFacebookEvent> (  ).Publish (  );
        }

        private void InitEvent ( IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<LoginSuccessEvent>().Subscribe(OnLoginSuccess);
            eventAggregator.GetEvent<LoginFailEvent>().Subscribe(OnLoginFail);
        }

        private void DestroyEvent ( IEventAggregator eventAggregator )
        {
            eventAggregator.GetEvent<LoginSuccessEvent> (  ).Unsubscribe ( OnLoginSuccess );
            eventAggregator.GetEvent<LoginFailEvent>().Unsubscribe(OnLoginFail);
        }

        private void OnLoginFail ( )
        {
            IsLoggingIn = false;
        }

        private async void OnLoginSuccess ( UserModel userModel )
        {
            var parameter = new NavigationParameters
            {
                {"user", userModel}
            };
            await _navigationService.NavigateAsync ("MenuPage/NavigationPage/SearchPage", parameter);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            DestroyEvent (  _eventAggregator);
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            InitEvent(_eventAggregator);
        }

    }
}
