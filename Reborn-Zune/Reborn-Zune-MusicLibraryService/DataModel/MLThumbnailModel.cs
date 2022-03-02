using GalaSoft.MvvmLight;
using Reborn_Zune_MusicLibraryEFCoreModel;
using System;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class MLThumbnailModel : ObservableObject, IMLDataModel
    {
        public MLThumbnailModel(Thumbnail thumbnail)
        {
            UnwrapDataFields(thumbnail);
            GetBitmapImage();
        }
        public string Id { get; set; }
        public byte[] ImageBytes { get; set; }
        public BitmapImage Image { get; set; }

        public void GetBitmapImage()
        {
            //RnD: todo

            /*
            if (ImageBytes.Length == 0)
            {
                Image = new BitmapImage(new Uri("ms-appx:///Assets/Vap-logo-placeholder.jpg"));
            }
            else
            {
                InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
                DataWriter writer = new DataWriter(randomAccessStream.GetOutputStreamAt(0));
                writer.WriteBytes(ImageBytes);
                _ = writer.StoreAsync();
                Image = new BitmapImage();
                Image.SetSource(randomAccessStream);
            }
            */

            //TEMP
            //Image = new BitmapImage(new Uri("ms-appx:///Assets/Vap-logo-placeholder.jpg"));
        }

        public void UnwrapDataFields(IEFCoreModel model)
        {
            var thumb = model as Thumbnail;
            this.Id = thumb.Id;
            this.ImageBytes = thumb.ImageBytes;
        }
    }
}
