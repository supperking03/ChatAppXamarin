using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

using ChattingApp.Enums;
using ChattingApp.Events;
using ChattingApp.Models;
using ChattingApp.Modules.MessageSending;

using Prism.Events;
using Prism.Navigation;

namespace ChattingApp.ViewModels
{
    public class ChatPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;
        private readonly IMessageSender _messageSender;
        private ObservableCollection<MessageModel> _messageList;
        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                SetProperty(ref _message, value);
            }
        }

        public ObservableCollection<MessageModel> MessageList
        {
            get
            {
                return _messageList;
            }
            set
            {
                SetProperty(ref _messageList, value);
            }
        }

        public ChatPageViewModel(INavigationService navigationService, IMessageSender messageSender, IEventAggregator eventAggerator)
        {
            _navigationService = navigationService;
            _messageSender = messageSender;
            Message = "";
            MessageList = new ObservableCollection<MessageModel>();
            _eventAggregator = eventAggerator;
            _eventAggregator.GetEvent < ReceiveEvent > ( ).Subscribe ( message =>
            {
                if ( message.MessageType == MessageTypeEnum.ExitChat )
                {
                    _navigationService.GoBackAsync ( );
                }
                if ( message.MessageType == MessageTypeEnum.InfoRequest &&
                     message.Sender == "Me" )
                {
                    return;
                }
                MessageList.Add ( message );
            } );
        }

        public ICommand SendCommand
        {
            get
            {
                return new DelegateCommand(SendMessage);
            }
        }

        public ICommand AskFacebookCommand
        {
            get
            {
                return new DelegateCommand(AskFacebook);
            }
        }

        private void AskFacebook ( )
        {
            var message = new MessageModel
            {
                Time = DateTime.Now.ToString(),
                MessageType = MessageTypeEnum.InfoRequest
            };
            _messageSender.SendMessage(message);
            Message = "";
        }

        public ICommand SendFacebookCommand
        {
            get
            {
                return new DelegateCommand(SendFacebook);
            }
        }

        private void SendFacebook ( )
        {
            var message = new MessageModel
            {
                Time = DateTime.Now.ToString(),
                MessageType = MessageTypeEnum.InfoApproved
            };
            _messageSender.SendMessage(message);
            Message = "";
        }

        private void SendMessage()
        {
            if (Message != "")
            {
                var message = new MessageModel
                {
                    Time = DateTime.Now.ToString ( ),
                    Content = Message,
                    MessageType = MessageTypeEnum.Text
                };
                _messageSender.SendMessage(message);
                Message = "";
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            _eventAggregator.GetEvent<QuitChatEvent> (  ).Publish (  );
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
