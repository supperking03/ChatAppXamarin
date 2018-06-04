using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChattingApp.Enums;

using Prism.Events;

namespace ChattingApp.Events
{
    public class FindEvent: PubSubEvent<GenderEnum>
    {
    }
}
