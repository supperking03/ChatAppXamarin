using System.Collections.Generic;

using Prism.Navigation;

using Xamarin.Forms;

namespace ChattingApp.Views
{
    public partial class MenuPage : MasterDetailPage, IMasterDetailPageOptions
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        public bool IsPresentedAfterNavigation
        {
            get { return Device.Idiom != TargetIdiom.Phone; }
        }
    }
}
