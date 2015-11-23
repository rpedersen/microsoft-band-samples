using System;
using System.Threading.Tasks;
using AppCore;
using Microsoft.Band;
using Microsoft.Band.Notifications;
using Microsoft.Band.Tiles;

namespace BandSupport
{
    public abstract class AppBandTile
    {
        public virtual async Task CreateBandTileAsync(IBandClient bandClient)
        {
            var bandTile = await CreateBandTileInternalAsync();
            await bandClient.TileManager.AddTileAsync(bandTile);
        }

        public virtual async Task<string> TileButtonPressedAsync(IBandClient bandClient, BandTileEventArgs<IBandTileButtonPressedEvent> args)
        {
            await bandClient.NotificationManager.VibrateAsync(VibrationType.NotificationOneTone);
            return string.Empty;
        }

        protected async Task<BandTile> CreateBandTileInternalAsync()
        {
            var bandTile = new BandTile(Id)
            {
                Name = Name,
                TileIcon = await TileIconUri.GetBandIconAsync(),
                SmallIcon = await SmallIconUri.GetBandIconAsync(),
            };

            return bandTile;
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
