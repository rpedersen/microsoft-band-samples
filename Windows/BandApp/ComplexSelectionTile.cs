using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore;
using Microsoft.Band;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;

namespace BandApp
{
    public class ComplexSelectionTile : AppBandTile
    {
        private static readonly Guid BandTileId = new Guid("F7AE4702-6971-4B90-AEF9-7C258A1EA27E");
        private static readonly Guid PageId = new Guid("76BD2020-6173-4BC5-8ECB-23E40C367D1E");

        private const int ComplexSelectionLayoutIndex = 0;
        private const int NumberPickerLayoutIndex = 1;
        private const int AlphaPickerLayoutIndex = 2;

        private readonly string[] _selection;
        private int _currentSelectorPosition;

        public ComplexSelectionTile()
        {
            Id = BandTileId;
            Name = "Complex Selection";
            Kind = AppBandTileKind.ComplexSelection;
            TileIconUri = new Uri("ms-appx:///Assets/ComplexSelectionTileLarge.png");
            SmallIconUri = new Uri("ms-appx:///Assets/ComplexSelectionTileSmall.png");

            _selection = new[] { "0", "1", "A" };
            _currentSelectorPosition = 0;
        }

        protected override async Task<BandTile> CreateBandTilelAsync()
        {
            var bandTile = await base.CreateBandTilelAsync();

            var seatDupLayout = CreateComplexSelectionLayout();
            bandTile.PageLayouts.Add(seatDupLayout);

            var numberPickerLayout = CreatePickerLayout<NumberButton>();
            bandTile.PageLayouts.Add(numberPickerLayout);

            var alphaPickerLayout = CreatePickerLayout<AlphaButton>();
            bandTile.PageLayouts.Add(alphaPickerLayout);

            return bandTile;

        }

        private PageLayout CreateComplexSelectionLayout()
        {
            var complexSelectionNumOneButton = new TextButton()
            {
                ElementId = PageElementKind.ComplexSelectionPartOneButton,
                Rect = new PageRect(0, 0, 50, 60),
                Margins = new Margins(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var complexSelectionNumTwoButton = new TextButton()
            {
                ElementId = PageElementKind.ComplexSelectionPartTwoButton,
                Rect = new PageRect(0, 0, 50, 60),
                Margins = new Margins(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var complexSelectionAlphaOneButton = new TextButton()
            {
                ElementId = PageElementKind.ComplexSelectionPartThreeButton,
                Rect = new PageRect(0, 0, 50, 60),
                Margins = new Margins(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var complexSelectionGoButton = new TextButton()
            {
                ElementId = PageElementKind.ComplexSelectionGoButton,
                Rect = new PageRect(0, 0, 60, 60),
                Margins = new Margins(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var complexSelectionPanel = new ScrollFlowPanel(complexSelectionNumOneButton, complexSelectionNumTwoButton,
                complexSelectionAlphaOneButton, complexSelectionGoButton)
            {
                Orientation = FlowPanelOrientation.Horizontal,
                Rect = new PageRect(0, 0, 258, 102),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var complexSelectionLayout = new PageLayout(complexSelectionPanel);
            return complexSelectionLayout;
        }

        private PageLayout CreatePickerLayout<T>() where T : struct
        {
            var textButtons = new List<TextButton>();

            foreach (var enumValue in (T[])Enum.GetValues(typeof(T)))
            {
                var textButton = new TextButton()
                {
                    ElementId = Convert.ToInt16(enumValue),
                    Rect = new PageRect(0, 0, 50, 60),
                    Margins = new Margins(10, 0, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                textButtons.Add(textButton);
            }

            var numberPickerPanel = new ScrollFlowPanel(textButtons)
            {
                Orientation = FlowPanelOrientation.Horizontal,
                Rect = new PageRect(0, 0, 258, 102),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var pickerLayout = new PageLayout(numberPickerPanel);
            return pickerLayout;
        }

        protected override PageData GetInitialPageData()
        {
            var complexSelectionPageData = CreateComplexSelectionPageData();
            return complexSelectionPageData;
        }

        private PageData CreateComplexSelectionPageData()
        {
            var complexSelectionPageData = new PageData(PageId,
                ComplexSelectionLayoutIndex,
                new TextButtonData(PageElementKind.ComplexSelectionPartOneButton, _selection[0]),
                new TextButtonData(PageElementKind.ComplexSelectionPartTwoButton, _selection[1]),
                new TextButtonData(PageElementKind.ComplexSelectionPartThreeButton, _selection[2]),
                new TextButtonData(PageElementKind.ComplexSelectionGoButton, "Go"));

            return complexSelectionPageData;
        }

        public override async Task TileButtonPressedAsync(IBandClient bandClient,
            BandTileEventArgs<IBandTileButtonPressedEvent> args)
        {
            switch (args.TileEvent.ElementId)
            {
                case PageElementKind.ComplexSelectionPartOneButton:
                    await ComplexSelectionPartOneButtonPushedAsync(bandClient);
                    break;

                case PageElementKind.ComplexSelectionPartTwoButton:
                    await ComplexSelectionPartTwoButtonPushedAsync(bandClient);
                    break;

                case PageElementKind.ComplexSelectionPartThreeButton:
                    await ComplexSelectionPartThreeButtonPushedAsync(bandClient);
                    break;

                case PageElementKind.ComplexSelectionGoButton:
                    ComplexSelectionGoButtonPushed();
                    break;

                case PageElementKind.ZeroButton:
                    await PickerButtonPushedAsync("0", bandClient);
                    break;

                case PageElementKind.OneButton:
                    await PickerButtonPushedAsync("1", bandClient);
                    break;

                case PageElementKind.TwoButton:
                    await PickerButtonPushedAsync("2", bandClient);
                    break;

                case PageElementKind.ThreeButton:
                    await PickerButtonPushedAsync("3", bandClient);
                    break;

                case PageElementKind.FourButton:
                    await PickerButtonPushedAsync("4", bandClient);
                    break;

                case PageElementKind.FiveButton:
                    await PickerButtonPushedAsync("5", bandClient);
                    break;

                case PageElementKind.SixButton:
                    await PickerButtonPushedAsync("6", bandClient);
                    break;

                case PageElementKind.SevenButton:
                    await PickerButtonPushedAsync("7", bandClient);
                    break;

                case PageElementKind.EightButton:
                    await PickerButtonPushedAsync("8", bandClient);
                    break;

                case PageElementKind.NineButton:
                    await PickerButtonPushedAsync("9", bandClient);
                    break;

                case PageElementKind.AButton:
                    await PickerButtonPushedAsync("A", bandClient);
                    break;

                case PageElementKind.BButton:
                    await PickerButtonPushedAsync("B", bandClient);
                    break;

                case PageElementKind.CButton:
                    await PickerButtonPushedAsync("C", bandClient);
                    break;

                case PageElementKind.DButton:
                    await PickerButtonPushedAsync("D", bandClient);
                    break;

                case PageElementKind.EButton:
                    await PickerButtonPushedAsync("E", bandClient);
                    break;

                case PageElementKind.FButton:
                    await PickerButtonPushedAsync("F", bandClient);
                    break;

                case PageElementKind.GButton:
                    await PickerButtonPushedAsync("G", bandClient);
                    break;

                case PageElementKind.HButton:
                    await PickerButtonPushedAsync("H", bandClient);
                    break;

                case PageElementKind.IButton:
                    await PickerButtonPushedAsync("I", bandClient);
                    break;
            }
        }

        private async Task<string> PickerButtonPushedAsync(string pickerValue, IBandClient bandClient)
        {
            _selection[_currentSelectorPosition] = pickerValue;
            var complexSelectionPageData = CreateComplexSelectionPageData();
            await bandClient.TileManager.SetPagesAsync(BandTileId, complexSelectionPageData);

            return string.Empty;
        }

        private async Task<string> ComplexSelectionPartOneButtonPushedAsync(IBandClient bandClient)
        {
            _currentSelectorPosition = 0;
            var numberPickerPageData = CreateNumberPickerPageData();
            await bandClient.TileManager.SetPagesAsync(BandTileId, numberPickerPageData);

            return string.Empty;
        }

        private async Task<string> ComplexSelectionPartTwoButtonPushedAsync(IBandClient bandClient)
        {
            _currentSelectorPosition = 1;
            var numberPickerPageData = CreateNumberPickerPageData();
            await bandClient.TileManager.SetPagesAsync(BandTileId, numberPickerPageData);

            return string.Empty;
        }

        private PageData CreateNumberPickerPageData()
        {
            var numberPickerPageData = new PageData(PageId,
                NumberPickerLayoutIndex,
                new TextButtonData(PageElementKind.ZeroButton, "0"),
                new TextButtonData(PageElementKind.OneButton, "1"),
                new TextButtonData(PageElementKind.TwoButton, "2"),
                new TextButtonData(PageElementKind.ThreeButton, "3"),
                new TextButtonData(PageElementKind.FourButton, "4"),
                new TextButtonData(PageElementKind.FiveButton, "5"),
                new TextButtonData(PageElementKind.SixButton, "6"),
                new TextButtonData(PageElementKind.SevenButton, "7"),
                new TextButtonData(PageElementKind.EightButton, "8"),
                new TextButtonData(PageElementKind.NineButton, "9"));

            return numberPickerPageData;
        }

        private async Task<string> ComplexSelectionPartThreeButtonPushedAsync(IBandClient bandClient)
        {
            _currentSelectorPosition = 2;
            var alphaPickerPageData = CreateAlphaPickerPageData();
            await bandClient.TileManager.SetPagesAsync(BandTileId, alphaPickerPageData);

            return string.Empty;
        }

        private PageData CreateAlphaPickerPageData()
        {

            var alphaPickerPageData = new PageData(PageId,
                AlphaPickerLayoutIndex,
                new TextButtonData(PageElementKind.AButton, "A"),
                new TextButtonData(PageElementKind.BButton, "B"),
                new TextButtonData(PageElementKind.CButton, "C"),
                new TextButtonData(PageElementKind.DButton, "D"),
                new TextButtonData(PageElementKind.EButton, "E"),
                new TextButtonData(PageElementKind.FButton, "F"),
                new TextButtonData(PageElementKind.GButton, "G"),
                new TextButtonData(PageElementKind.HButton, "H"),
                new TextButtonData(PageElementKind.IButton, "I"));

            return alphaPickerPageData;
        }

        private void ComplexSelectionGoButtonPushed()
        {
            var selection = string.Join("", _selection);
            OnSelectionChanged(selection);
        }

        public event EventHandler<SelectionEventArgs> SelectionChanged;

        private void OnSelectionChanged(string selection)
        {
            if (SelectionChanged != null)
            {
                var selectionEventArgs = new SelectionEventArgs(selection);
                SelectionChanged(this, selectionEventArgs);
            }
        }
    }
}