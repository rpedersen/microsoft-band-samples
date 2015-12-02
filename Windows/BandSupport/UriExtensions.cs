using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Band;
using Microsoft.Band.Personalization;
using Microsoft.Band.Tiles;

namespace BandSupport
{
    public static class UriExtensions
    {
        public static async Task<BandImage> GetBandImageAsync(this Uri uri)
        {
            var writeableBitmap = await GetWriteableBitmap(uri);
            return writeableBitmap.ToBandImage();
        }


        public static async Task<BandIcon> GetBandIconAsync(this Uri uri)
        {
            var writeableBitmap = await GetWriteableBitmap(uri);
            return writeableBitmap.ToBandIcon();
        }

        private static async Task<WriteableBitmap> GetWriteableBitmap(Uri uri)
        {
            var imageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);

            using (var fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                var writeableBitmap = new WriteableBitmap(1, 1);
                await writeableBitmap.SetSourceAsync(fileStream);
                return writeableBitmap;
            }
        }
    }
}
