using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SkyStream;
using LLSSample;

namespace SkyStream
{
    public partial class Playlist : PhoneApplicationPage
    {
        public static SkyDriveEntity selectedItem;

        public Playlist()
        {
            InitializeComponent();
            List<AlphaKeyGroup<SkyDriveEntity>> dataSource = AlphaKeyGroup<SkyDriveEntity>.CreateGroups(MainPage.data,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (SkyDriveEntity s) => { return s.Name; }, true);

            
            this.PlaylistSelector.ItemsSource = dataSource;
        }

        private void PlaylistSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = (SkyDriveEntity)(this.PlaylistSelector.SelectedItem);
            bool ans = NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
            

        }

        //List<SkyDriveEntity> playList = new List<SkyDriveEntity>();
        
    }
}