using Xamarin.Forms;

namespace ChattingApp.Views
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();
        }

        private void ListView_OnItemSelected ( object sender, SelectedItemChangedEventArgs e )
        {
            ( ( Li
                stView ) sender ).SelectedItem = null;
        }

        private void Element_OnChildAdded ( object sender, ElementEventArgs e )
        {
            ls.ScrollTo ( sender, ScrollToPosition.End, true );
        }
    }
}
