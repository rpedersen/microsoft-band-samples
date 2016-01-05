using System;

namespace BandApp
{
    public class SelectionEventArgs : EventArgs
    {
        public SelectionEventArgs(string selection)
        {
            Selection = selection;
        }

        public string Selection { get; set; }
    }
}