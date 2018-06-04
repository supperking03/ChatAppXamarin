using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChattingApp.Events;

using Prism.Events;

namespace ChattingApp.Modules.FacebookLogin
{
    class LoginFacebookHandler: ILoginFacebook
    {
        private readonly IEventAggregator _eventAggregator;
        public LoginFacebookHandler ( IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public void LoginFacebook ( )
        {
            _eventAggregator.GetEvent<RequestLoginFacebookEvent>().Publish();
        }
    }
}
