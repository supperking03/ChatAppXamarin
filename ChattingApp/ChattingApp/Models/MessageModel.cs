using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChattingApp.Enums;

using Prism.Mvvm;

namespace ChattingApp.Models
{
    public class MessageModel: BindableBase
    {
        private string _content;
        private string _sender;
        private string _time;
        private MessageTypeEnum _messageType;


        public MessageTypeEnum MessageType
        {
            get
            {
                return _messageType;
            }
            set
            {
                SetProperty(ref _messageType, value);
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                SetProperty(ref _content, value);
            }
        }

        public string Sender
        {
            get
            {
                return _sender;
            }
            set
            {
                SetProperty(ref _sender, value);
            }
        }

        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                SetProperty(ref _time, value);
            }
        }
    }
}
