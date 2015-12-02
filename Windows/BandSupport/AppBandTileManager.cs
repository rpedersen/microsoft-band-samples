using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore;
using Microsoft.Band;

namespace BandSupport
{
    public class AppBandTileManager
    {
        public static AppBandTileManager Instance = new AppBandTileManager();

        private AppBandTileManager()
        {
            AppBandTiles = new List<AppBandTile>
            {
                MessagesTile,
                CustomMessagesTile,
                BasicTwoWayCommunicationTile,
                ComplexSelectionTile
            };
        }

        public async Task DeleteMessagesAsync(IBandClient bandClient)
        {
            await bandClient.TileManager.RemovePagesAsync(MessagesTile.Id);
            await bandClient.TileManager.RemovePagesAsync(CustomMessagesTile.Id);
        }

        private static async Task DeleteBandTiles(IBandClient bandClient)
        {
            var tiles = await bandClient.TileManager.GetTilesAsync();

            foreach (var tile in tiles)
            {
                await bandClient.TileManager.RemoveTileAsync(tile);
            }
        }

        public async Task CreateBandTilesAsync(IBandClient bandClient)
        {
            foreach (var appBandTile in AppBandTiles)
            {
                await appBandTile.CreateBandTileIfNotExistsAsync(bandClient);
            }
        }

        public async Task ReceiveNotificationAsync(IBandClient bandClient, Notification notification)
        {
            AppBandTile tile = null;

            if (notification.Kind == NotificationKind.CustomMessage)
            {
                tile = CustomMessagesTile;
            }

            if (tile != null)
            {
                await tile.ReceiveNotificationAsync(bandClient, notification);
            }
        }

        public async Task SetupBandAsync(IBandClient bandClient)
        {
            await DeleteBandTiles(bandClient);
            await CreateBandTilesAsync(bandClient);
        }

        public List<AppBandTile> AppBandTiles { get; }

        public MessagesTile MessagesTile { get; } = new MessagesTile();

        public CustomMessagesTile CustomMessagesTile { get; } = new CustomMessagesTile();

        public BasicTwoWayCommunicationTile BasicTwoWayCommunicationTile { get; } = new BasicTwoWayCommunicationTile();

        public ComplexSelectionTile ComplexSelectionTile { get; } = new ComplexSelectionTile();
    }
}