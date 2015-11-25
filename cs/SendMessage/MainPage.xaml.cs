using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AppCore;
using BandSupport;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SendMessage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Get the shared instance of the Band client. The instance of the band client is
            // shared be the whole context of the app as prescribed by the Band documention.
            var bandClient = await AppBandManager.Instance.GetBandClientAsync();

            // Get an instance of the tile that I want to create. Because I am using an abstraction
            // I can get the tile from the custom tile manager and then tell 
            var customMessagesTile = AppBandManager.Instance.AppBandTileManager.CustomMessagesTile;

            // use the abstraction to create the actual band tile and add it to the band using
            // the band client that is passed in. We could have used the tile wrapper to just get
            // and instance of the real band tile and then made a second call to add the tile to
            // the band. Passing in the band client just reduces that to one line of code instead
            // of two.
            await customMessagesTile.CreateBandTileIfNotExistsAsync(bandClient);
        }

        private async void SendMessageWithDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await AppBandManager.Instance.GetBandClientAsync();

            var notification = new Notification
            {
                Title = "Message With Dialog",
                Message = "This is the long message that goes under the title."
            };

            await AppBandManager.Instance.AppBandTileManager.MessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }

        private async void SendMessageWithoutDialogButton_Click(object sender, RoutedEventArgs e)
        {

            var bandClient = await AppBandManager.Instance.GetBandClientAsync();

            var notification = new Notification
            {
                Title = "Message With Dialog",
                Message = "This is the long message that goes under the title.",
                ShowDialog = false
            };

            await AppBandManager.Instance.AppBandTileManager.MessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }
    }
}
