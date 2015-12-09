using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AppCore;
using Microsoft.Band;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BandApp
{
    public sealed partial class MainPage : Page
    {
        private readonly AppBandManager _appBandManager;
        private readonly AppBandTileManager _appBandTileManager;

        public MainPage()
        {
            _appBandManager = AppBandManager.Instance;
            _appBandTileManager = AppBandTileManager.Instance;

            this.InitializeComponent();
        }

        private async void CreateMessagesTileButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the shared instance of the Band client. The instance of the band client is
            // shared be the whole context of the app as prescribed by the Band documention.
            var bandClient = await _appBandManager.GetBandClientAsync();

            // Get an instance of the tile that I want to create. Because I am using an abstraction
            // I can get the tile from the custom tile manager. 
            var messagesTile = _appBandTileManager.MessagesTile;

            // use the abstraction to create the actual band tile and add it to the band using
            // the band client that is passed in. We could have used the tile wrapper to just get
            // and instance of the real band tile and then made a second call to add the tile to
            // the band. Passing in the band client just reduces that to one line of code instead
            // of two.
            await messagesTile.CreateBandTileIfNotExistsAsync(bandClient);
        }

        private async void SendMessageWithDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();

            if (! await _appBandTileManager.MessagesTile.ExistsOnBandAsync(bandClient))
            {
                ShowCreateTileDialog();
            }

            var notification = new Notification
            {
                Title = "Message With Dialog",
                Message = "This is the long message that goes under the title.",
                ShowDialog = true
            };

            await _appBandTileManager.MessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }

        private async void SendMessageWithoutDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();

            var notification = new Notification
            {
                Title = "Message With Dialog",
                Message = "This is the long message that goes under the title."
            };

            await _appBandTileManager.MessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }

        private static async void ShowCreateTileDialog()
        {
            var messageDialog = new MessageDialog("The 'Create Messages Tile' button needs to be pressed before sending a message");
            await messageDialog.ShowAsync();
        }

        private async void CreateCustomMessagesTileButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the shared instance of the Band client. The instance of the band client is
            // shared be the whole context of the app as prescribed by the Band documention.
            var bandClient = await _appBandManager.GetBandClientAsync();

            // Get an instance of the tile that I want to create. Because I am using an abstraction
            // I can get the tile from the custom tile manager. 
            var customMessagesTile = _appBandTileManager.CustomMessagesTile;

            // use the abstraction to create the actual band tile and add it to the band using
            // the band client that is passed in. We could have used the tile wrapper to just get
            // and instance of the real band tile and then made a second call to add the tile to
            // the band. Passing in the band client just reduces that to one line of code instead
            // of two.
            await customMessagesTile.CreateBandTileIfNotExistsAsync(bandClient);
        }

        private async void SendCustomMessageWithoutButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();

            var notification = new Notification
            {
                Kind = NotificationKind.CustomMessage,
                Title = "Custom Message",
                Message = "This is the long custom message that goes under the custom title"
            };

            await _appBandTileManager.CustomMessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }

        private async void SendCustomMessageWithButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();

            var notification = new Notification
            {
                Kind = NotificationKind.CustomMessageWithButton,
                Title = "Custom Message",
                Message = "This is the long custom message that goes under the custom title and before the button"
            };

            await _appBandTileManager.CustomMessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }
    }
}
