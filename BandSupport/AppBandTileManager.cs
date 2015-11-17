using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Band;

namespace BandSupport
{
    public class AppBandTileManager
    {
        public AppBandTileManager()
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

        public List<AppBandTile> AppBandTiles { get; private set; }

        public MessagesTile MessagesTile { get; } = new MessagesTile();

        public CustomMessagesTile CustomMessagesTile { get; } = new CustomMessagesTile();

        public BasicTwoWayCommunicationTile BasicTwoWayCommunicationTile { get; } = new BasicTwoWayCommunicationTile();

        public ComplexSelectionTile ComplexSelectionTile { get; } = new ComplexSelectionTile();
    }
}