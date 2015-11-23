using BandSupport;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CreateTile
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

        private async void CreateTileButton_Click(object sender, RoutedEventArgs e)
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
            await customMessagesTile.CreateBandTileAsync(bandClient);
        }
    }
}
