using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SkyStream.Resources;

namespace SkyStream
{
    public partial class Page1 : PhoneApplicationPage
    {
        // Constructor
        public Page1()
        {
            InitializeComponent();
            /*Microsoft.SilverlightMediaFramework.Core.Media.PlaylistItem item = new Microsoft.SilverlightMediaFramework.Core.Media.PlaylistItem();
            item.MediaSource = "";
            SMFPlayer.Playlist = item; */

            this.SongName.Text = Playlist.selectedItem.Name;

            Microsoft.SilverlightMediaFramework.Core.Media.PlaylistItem it = new Microsoft.SilverlightMediaFramework.Core.Media.PlaylistItem();
            it.DeliveryMethod = Microsoft.SilverlightMediaFramework.Plugins.Primitives.DeliveryMethods.Streaming;
            if (Playlist.selectedItem.link != null)
            {
                it.MediaSource = new Uri(Playlist.selectedItem.link);
            }
            else
            {
                MessageBox.Show("Sorry Bro!!");
            }

            var itList = new System.Collections.ObjectModel.ObservableCollection<Microsoft.SilverlightMediaFramework.Core.Media.PlaylistItem>();
            itList.Add(it);
            this.SMFPlay.Playlist = itList;


            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Playlist.xaml", UriKind.Relative));
        } */

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}