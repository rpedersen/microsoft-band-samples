using System;
using System.Threading.Tasks;
using Microsoft.Band;

namespace BandSupport
{
    public class MessagesTile : AppBandTile
    {
        private static readonly Guid BandTileId = new Guid("C0C90758-36EE-4E1F-8A3F-801C3566E8C2");

        public MessagesTile()
        {
            Id = BandTileId;
            Name = "Messages";
            Kind = AppBandTileKind.Messages;
            TileIconUri = new Uri("ms-appx:///Assets/MessagesTileLarge.png");
            SmallIconUri = new Uri("ms-appx:///Assets/MessageTileSmall.png");
        }

        public async override Task CreateBandTileAsync(IBandClient bandClient)
        {
            var bandTile = await CreateBandTileInternalAsync();

            await bandClient.TileManager.AddTileAsync(bandTile);
        }
    }
}