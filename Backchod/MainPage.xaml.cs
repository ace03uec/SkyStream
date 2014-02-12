using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Live;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SkyStream
{
    public partial class MainPage : PhoneApplicationPage
    {

        public static List<SkyDriveEntity> data = new List<SkyDriveEntity>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        //object for live connect session
        LiveConnectSession _currentSession;

        LiveConnectClient liveClient;

        private async void SignInButton_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status == Microsoft.Live.LiveConnectSessionStatus.Connected)
            {
                _currentSession = e.Session;
                this.liveClient = new LiveConnectClient(e.Session);
                await this.GetMe();
                this.Proceed.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.Proceed.Visibility = System.Windows.Visibility.Collapsed;
                this.liveClient = null;
                this.tbGreeting.Text = e.Error != null ? e.Error.ToString() : string.Empty;
            }
        }

        private async Task<int> GetMe()
        {
            try
            {
                LiveOperationResult operationResult = await this.liveClient.GetAsync("me");
                var jsonResult = operationResult.Result as dynamic;
                string firstName = jsonResult.first_name ?? string.Empty;
                string lastName = jsonResult.last_name ?? string.Empty;
                this.tbGreeting.Text = "Welcome " + firstName;
                return 0;
            }
            catch (Exception e)
            {
                this.tbGreeting.Text = e.ToString();
                return 55;
            }
        }

        public async static Task<List<SkyDriveEntity>> GetFolderContents(LiveConnectSession session, string folderId)
        {
            try
            {
                LiveConnectClient client = new LiveConnectClient(session);
                LiveOperationResult result = await client.GetAsync(folderId + "/files");

                //clear entries in data
                data.Clear();

                List<object> container = (List<object>)result.Result["data"];
                foreach (var item in container)
                {
                    SkyDriveEntity entity = new SkyDriveEntity();

                    IDictionary<string, object> dictItem = (IDictionary<string, object>)item;
                    string type = dictItem["type"].ToString();
                    entity.IsFolder = type == "folder" || type == "album" ? true : false;
                    entity.ID = dictItem["id"].ToString();
                    entity.Name = dictItem["name"].ToString();
                    data.Add(entity);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private async void Proceed_Click(object sender, RoutedEventArgs e)
        {
            await GetFolderContents(_currentSession, "me/skydrive/public_documents");

            LiveConnectClient liveClient = new LiveConnectClient(this._currentSession);

            try
            {
                for (int i = 0; i < data.Count; i++)
                {
                    string fName = data[i].ID + "/shared_read_link";
                    LiveOperationResult operationResult = await liveClient.GetAsync(fName);
                    dynamic result = operationResult.Result;
                    string ans = result.link;
                    Regex r = new Regex(@"redir");
                    data[i].link = r.Replace(ans, "download");
                    //data[i].link = result.link;
                }
                NavigationService.Navigate(new Uri("/Playlist.xaml", UriKind.Relative));
            }
            catch
            {
                MessageBox.Show("Sorry Bro!!");
            }
        }

        /*private async string getFolderLink(string name)
        {
            try
            {
                string fName = name + "/shared_read_link";
                LiveConnectClient liveClient = new LiveConnectClient(this._currentSession);
                LiveOperationResult operationResult =
                    await liveClient.GetAsync(fName);
                dynamic result = operationResult.Result;
                //answer in result.link
                this.debug.Text = result.link;

                return result.link;
            }
            catch (LiveConnectException exception)
            {
                //this.infoTextBlock.Text = "Error getting shared read link: " + exception.Message;
            }
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

    public class SkyDriveEntity
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public bool IsFolder { get; set; }
        public string link { get; set; }
    }
}