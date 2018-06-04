using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ChattingApp.Enums;
using ChattingApp.Events;
using ChattingApp.Models;

using Prism.Events;
using Prism.Navigation;

using Xamarin.Forms;

namespace ChattingApp.ViewModels
{
    public class MenuPageViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;

        private UserModel _user;

        public UserModel User
        {
            get
            {
                return _user;
            }
            set
            {
                SetProperty ( ref _user, value );
            }
        }

        public MenuPageViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            User = new UserModel (  );
            _eventAggregator.GetEvent < LoginSuccessEvent > ( ).Subscribe ( LoginSuccess );
            _eventAggregator.GetEvent < LogoutSucessEvent > ( ).Subscribe ( LogoutSuccess );
        }

        private void LogoutSuccess ( )
        {
            //User = new UserModel (  );
        }

        private void LoginSuccess ( UserModel obj )
        {
            User = obj;
        }

        public void OnNavigatedFrom ( NavigationParameters parameters )
        {

        }

        public void OnNavigatedTo ( NavigationParameters parameters )
        {
            if (parameters.ContainsKey("user"))
            {
                User = (UserModel)parameters["user"];
            }
        }

        public void OnNavigatingTo ( NavigationParameters parameters )
        {
        }
    }
}
