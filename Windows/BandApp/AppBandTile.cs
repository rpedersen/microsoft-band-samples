using System;
using System.Linq;
using System.Threading.Tasks;
using AppCore;
using Microsoft.Band;
using Microsoft.Band.Notifications;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;

namespace BandApp
{
    public abstract class AppBandTile
    {
        public async Task CreateBandTileIfNotExistsAsync(IBandClient bandClient)
        {
            if (!await ExistsOnBandAsync(bandClient))
            {
                var bandTile = await CreateBandTilelAsync();
                await bandClient.TileManager.AddTileAsync(bandTile);

                var pageData = GetInitialPageData();
                if (pageData != null)
                {
                    await bandClient.TileManager.SetPagesAsync(Id, pageData); 
                }
            }
        }

        public async Task<bool> ExistsOnBandAsync(IBandClient bandClient)
        {
            var existingTiles = await bandClient.TileManager.GetTilesAsync();
            var tileExists = existingTiles.Any(bt => bt.TileId == Id);
            return tileExists;
        }

        protected virtual async Task<BandTile> CreateBandTilelAsync()
        {
            var bandTile = new BandTile(Id)
            {
                Name = Name,
                TileIcon = await TileIconUri.GetBandIconAsync(),
                SmallIcon = await SmallIconUri.GetBandIconAsync(),
            };

            return bandTile;
        }

        protected virtual PageData GetInitialPageData()
        {
            return null;
        }

        public virtual async Task TileButtonPressedAsync(IBandClient bandClient, BandTileEventArgs<IBandTileButtonPressedEvent> args)
        {
            await bandClient.NotificationManager.VibrateAsync(VibrationType.NotificationOneTone);
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public AppBandTileKind Kind { get; set; }

        public Uri TileIconUri { get; set; }

        public Uri SmallIconUri { get; set; }

        public virtual async Task ReceiveNotificationAsync(IBandClient bandClient, Notification notification)
        { }
    }
}
