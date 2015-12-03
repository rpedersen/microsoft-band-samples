using System;

namespace BandSupport
{
    public class CustomMessagesTile : AppBandTile
    {
        public CustomMessagesTile()
        {
            Id = new Guid("EC6941B0-4793-4915-9A4C-C2647E94FBE9");
            Name = "Custom Messages";
            Kind = AppBandTileKind.CustomMessages;
            TileIconUri = new Uri("ms-appx:///Assets/CustomMessagesTileLarge.png");
            SmallIconUri = new Uri("ms-appx:///Assets/CustomMessagesTileSmall.png");
        }
    }
}