using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AppCore;
using Microsoft.Band.Tiles;
using System.Diagnostics;
using Microsoft.Band;
using Microsoft.Band.Sensors;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BandApp
{
    public sealed partial class MainPage : Page
    {
        private readonly CoreDispatcher _dispatcher;
        private readonly AppBandManager _appBandManager;
        private readonly AppBandTileManager _appBandTileManager;

        public MainPage()
        {
            InitializeComponent();

            _dispatcher = Window.Current.Dispatcher;
            _appBandManager = AppBandManager.Instance;
            _appBandTileManager = AppBandTileManager.Instance;

            InitializeEventHandlers();
        }

        private async void InitializeEventHandlers()
        {
            var bandClient = await _appBandManager.GetBandClientAsync();
            await bandClient.TileManager.StartReadingsAsync();

            var messageDialog = new MessageDialog("The Band is connected.\n\nAudience CHEERS loudly :)");
            await messageDialog.ShowAsync();
        }

        private async void StartHeartRateButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();
            if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
            {
                var consent = await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();

                if (!consent)
                {
                    return;
                }
            }

            bandClient.SensorManager.HeartRate.ReadingChanged += HeartRate_ReadingChanged;

            await bandClient.SensorManager.HeartRate.StartReadingsAsync();
        }

        private async void HeartRate_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandHeartRateReading> args)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    HeartRateQuality.Text = args.SensorReading.Quality.ToString();
                    HeartRateValue.Text = args.SensorReading.HeartRate.ToString();
                }
                catch
                { }
            });
        }

        private async void StopHeartRateButton_Click(object sender, RoutedEventArgs args)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();
            await bandClient.SensorManager.HeartRate.StopReadingsAsync();
            bandClient.SensorManager.HeartRate.ReadingChanged -= HeartRate_ReadingChanged;
            HeartRateQuality.Text = "-";
            HeartRateValue.Text = "-";
        }

        private async void TileManagerOnTileButtonPressed(object sender, BandTileEventArgs<IBandTileButtonPressedEvent> bandTileEventArgs)
        {
            var appBandTile = _appBandTileManager.AppBandTiles.FirstOrDefault(dt => bandTileEventArgs.TileEvent.TileId == dt.Id);

            if (appBandTile != null)
            {
                var bandClient = await _appBandManager.GetBandClientAsync();
                await appBandTile.TileButtonPressedAsync(bandClient, bandTileEventArgs);
            }
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

            if (!await _appBandTileManager.MessagesTile.ExistsOnBandAsync(bandClient))
            {
                ShowCreateTileDialog();
            }

            var notification = GetMessageWithDialog();

            await _appBandTileManager.MessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }

        private async void SendMessageWithoutDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();

            // For demonstration purposes I am omitting the call to check if the tile exists
            //if (!await _appBandTileManager.MessagesTile.ExistsOnBandAsync(bandClient))
            //{
            //    ShowCreateTileDialog();
            //}

            var notification = GetMessageWithoutDialog();

            await _appBandTileManager.MessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }

        private static async void ShowCreateTileDialog()
        {
            var messageDialog = new MessageDialog("The presenter didn't press the right button.\n\nEverybody shout, 'Press the Setup Band button!'");
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

            customMessagesTile.CustomMessageButtonPressed += CustomMessagesTileOnCustomMessageButtonPressed;
        }

        private async void SendCustomMessageWithoutButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();

            var notification = GetCustomMessageWithoutButton();

            await _appBandTileManager.CustomMessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }

        private async void SendCustomMessageWithButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();

            var notification = GetCustomMessageWithButton();

            await _appBandTileManager.CustomMessagesTile.ReceiveNotificationAsync(bandClient, notification);
        }

        private async void CreateComplexSelectionTileButton_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();
            var complexSelectionTile = _appBandTileManager.ComplexSelectionTile;

            await complexSelectionTile.CreateBandTileIfNotExistsAsync(bandClient);

        }

        private async void ComplexSelectionTileOnSelectionChanged(object sender, SelectionEventArgs selectionEventArgs)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    ComplexSelectionOutput.Text = selectionEventArgs.Selection;
                }
                catch
                { }
            });
        }

        private async void SetupBand_Click(object sender, RoutedEventArgs e)
        {
            var bandClient = await _appBandManager.GetBandClientAsync();
            await _appBandTileManager.SetupBandAsync(bandClient);

            AppBandTileManager.Instance.CustomMessagesTile.CustomMessageButtonPressed += CustomMessagesTileOnCustomMessageButtonPressed;
            AppBandTileManager.Instance.ComplexSelectionTile.SelectionChanged += ComplexSelectionTileOnSelectionChanged;
        }

        [DebuggerStepThrough]
        private Notification GetMessageWithDialog()
        {
            var notification = new Notification
            {
                Title = "Message With Dialog",
                Message = "Demo is working! Lots of clapping in the audience.",
                ShowDialog = true
            };

            return notification;
        }

        [DebuggerStepThrough]
        private Notification GetMessageWithoutDialog()
        {
            var notification = new Notification
            {
                Title = "Message Without Dialog",
                Message = "Two buttons worked in a row! Ryan is completely surprised :)"
            };

            return notification;
        }

        [DebuggerStepThrough]
        private Notification GetCustomMessageWithoutButton()
        {
            var notification = new Notification
            {
                Kind = NotificationKind.CustomMessage,
                Title = "Custom Message",
                Message = "Custom messages are awesome! The loud clapping from the audience is hurting my ears :)"
            };

            return notification;
        }

        [DebuggerStepThrough]
        private Notification GetCustomMessageWithButton()
        {
            var notification = new Notification
            {
                Kind = NotificationKind.CustomMessageWithButton,
                Title = "Custom Message With Button",
                Message = "A message and a button! This demo is off-the-hook crazy good. Audence shouts, 'Show me more buttons!'"
            };

            return notification;
        }

        private async void CustomMessagesTileOnCustomMessageButtonPressed(object sender, EventArgs eventArgs)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var messageDialog = new MessageDialog("Audience goes wild (clap and cheer)...\n\nThis demo is rock'n!");
                try
                {
                    await messageDialog.ShowAsync();
                }
                catch
                { }
            });
        }
    }
}
