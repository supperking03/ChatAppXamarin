using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChattingApp.Events;
using ChattingApp.Models;

using Microsoft.Practices.Unity;

using Prism.Events;
using Prism.Navigation;

namespace ChattingApp.Modules.MessageSending
{
    public class MessageSender: IMessageSender
    {
        private readonly IEventAggregator _eventAggregator;
        public MessageSender (IEventAggregator eventAggregator )
        {
            _eventAggregator = eventAggregator;
        }
        public void SendMessage ( MessageModel message )
        {
            _eventAggregator.GetEvent<SendEvent> (  ).Publish ( message );
        }
    }
}
