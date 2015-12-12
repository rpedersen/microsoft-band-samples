using System;

namespace BandApp
{
    public class ComplexSelectionTile : AppBandTile
    {
        public ComplexSelectionTile()
        {
            Id = new Guid("5F44F660-EE83-4DB8-B8D3-2CE9DB9033E9");
            Name = "Complex Selection";
        }
    }

    public enum NumberButton
    {
        Zero = PageElementKind.ZeroButton,
        One = PageElementKind.OneButton,
        Two = PageElementKind.TwoButton,
        Three = PageElementKind.ThreeButton,
        Four = PageElementKind.FourButton,
        Five = PageElementKind.FiveButton,
        Six = PageElementKind.SixButton,
        Seven = PageElementKind.SevenButton,
        Eight = PageElementKind.EightButton,
        Nine = PageElementKind.NineButton,
    }

    public enum AlphaButton
    {
        A = PageElementKind.AButton,
        B = PageElementKind.BButton,
        C = PageElementKind.CButton,
        D = PageElementKind.DButton,
        E = PageElementKind.EButton,
        F = PageElementKind.FButton,
        G = PageElementKind.GButton,
        H = PageElementKind.HButton,
        I = PageElementKind.IButton,
    }
}