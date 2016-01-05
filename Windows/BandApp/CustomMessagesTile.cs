using System;
using System.Threading.Tasks;
using AppCore;
using Microsoft.Band;
using Microsoft.Band.Notifications;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;

namespace BandApp
{
    public class CustomMessagesTile : AppBandTile
    {
        private const int CustomMessageLayoutIndex = 0;
        private const int CustomMessageWithButtonLayoutIndex = 1;

        public CustomMessagesTile()
        {
            Id = new Guid("EC6941B0-4793-4915-9A4C-C2647E94FBE9");
            Name = "Custom Messages";
            Kind = AppBandTileKind.CustomMessages;
            TileIconUri = new Uri("ms-appx:///Assets/CustomMessagesTileLarge.png");
            SmallIconUri = new Uri("ms-appx:///Assets/CustomMessagesTileSmall.png");
        }

        protected async override Task<BandTile> CreateBandTilelAsync()
        {
            var bandTile = await base.CreateBandTilelAsync();

            var customMessageLayout = CreateMessageLayout();
            bandTile.PageLayouts.Add(customMessageLayout);

            var customMessageWithAckButtonLayout = CreateMessageWithButtonLayout();
            bandTile.PageLayouts.Add(customMessageWithAckButtonLayout);

            return bandTile;
        }

        private static PageLayout CreateMessageLayout()
        {
            var messageWrappedTextBlock = new WrappedTextBlock()
            {
                ElementId = PageElementKind.CustomMessageText,
                Rect = new PageRect(0, 0, 258, 102),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            var messageWithoutAckButtonPanel = new ScrollFlowPanel(messageWrappedTextBlock)
            {
                Orientation = FlowPanelOrientation.Vertical,
                Rect = new PageRect(0, 0, 258, 102),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            var messageWithoutAckButtonLayout = new PageLayout(messageWithoutAckButtonPanel);
            return messageWithoutAckButtonLayout;
        }

        private static PageLayout CreateMessageWithButtonLayout()
        {
            var customMessageWrappedTextBlock = new WrappedTextBlock()
            {
                ElementId = PageElementKind.CustomMessageText2,
                Rect = new PageRect(0, 0, 258, 102),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            var customMessageButton = new TextButton()
            {
                ElementId = PageElementKind.CustomMessageButton,
                Margins = new Margins(25, 10, 25, 10),
                Rect = new PageRect(0, 0, 170, 45),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var customMessageWithAckButtonPanel = new ScrollFlowPanel(customMessageWrappedTextBlock, customMessageButton)
            {
                Orientation = FlowPanelOrientation.Vertical,
                Rect = new PageRect(0, 0, 258, 102),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            var customMessageWithAckButtonLayout = new PageLayout(customMessageWithAckButtonPanel);
            return customMessageWithAckButtonLayout;
        }

        public async override Task ReceiveNotificationAsync(IBandClient bandClient, Notification notification)
        {
            PageData messagePageData;

            if (notification.Kind == NotificationKind.CustomMessage)
            {
                messagePageData = new PageData(notification.Id,
                   CustomMessageLayoutIndex,
                   new WrappedTextBlockData(PageElementKind.CustomMessageText, notification.Message));
            }
            else
            {
                messagePageData = new PageData(notification.Id,
                   CustomMessageWithButtonLayoutIndex,
                   new WrappedTextBlockData(PageElementKind.CustomMessageText2, notification.Message),
                   new TextButtonData(PageElementKind.CustomMessageButton, "Acknowledge"));
            }

            await bandClient.TileManager.SetPagesAsync(Id, messagePageData);

            await bandClient.NotificationManager.VibrateAsync(VibrationType.TwoToneHigh);
        }

        public async override Task TileButtonPressedAsync(IBandClient bandClient, BandTileEventArgs<IBandTileButtonPressedEvent> bandTileEventArgs)
        {
            await base.TileButtonPressedAsync(bandClient, bandTileEventArgs);

            if (bandTileEventArgs.TileEvent.ElementId == PageElementKind.CustomMessageButton)
            {
                OnCustomMessageButtonPressed(bandTileEventArgs);
            }
        }

        private void OnCustomMessageButtonPressed(BandTileEventArgs<IBandTileButtonPressedEvent> bandTileEventArgs)
        {
            // this is a good place to integrate logging like event tracking with Application Insights
            // https://azure.microsoft.com/en-us/documentation/articles/app-insights-api-custom-events-metrics/#track-event

            if (CustomMessageButtonPressed != null)
            {
                var eventArgs = new EventArgs();
                CustomMessageButtonPressed(this, eventArgs);
            }
        }

        public event EventHandler CustomMessageButtonPressed;
    }
}