using System;
using System.Threading.Tasks;
using AppCore;
using Microsoft.Band;
using Microsoft.Band.Notifications;

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
            SmallIconUri = new Uri("ms-appx:///Assets/MessagesTileSmall.png");
        }

        public async override Task ReceiveNotificationAsync(IBandClient bandClient, Notification notification)
        {
            var messageFlags = notification.ShowDialog ? MessageFlags.ShowDialog : MessageFlags.None;
            await bandClient.NotificationManager.SendMessageAsync(Id, notification.Title, notification.Message, DateTimeOffset.Now, messageFlags);
        }
    }
}