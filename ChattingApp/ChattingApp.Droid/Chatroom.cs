using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ChattingApp.Models;

namespace ChattingApp.Droid
{
    public class Chatroom
    {
        public string User1
        {
            get;
            set;
        }

        public string User2
        {
            get;
            set;
        }

        public bool IsAvailable
        {
            get;
            set;
        }
    }
}