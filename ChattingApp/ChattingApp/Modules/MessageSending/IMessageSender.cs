using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChattingApp.Models;

using Prism.Events;

namespace ChattingApp.Modules.MessageSending
{
    public interface IMessageSender
    {
        void SendMessage ( MessageModel message);
    }
}
