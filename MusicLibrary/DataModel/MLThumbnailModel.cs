﻿using Reborn_Zune_MusicLibraryEFCoreModel;
using System;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class MLThumbnailModel
    {
        public MLThumbnailModel(Thumbnail thumbnail)
        {
            Thumbnail = thumbnail;
            GetBitmapImage();
        }
        public Thumbnail Thumbnail { get; set; }
        public BitmapImage Image { get; set; }

        public void GetBitmapImage()
        {
            if (Thumbnail.ImageBytes.Length == 0)
            {
                Image = new BitmapImage(new Uri("ms-appx:///Assets/Vap-logo-placeholder.jpg"));
            }
            else
            {
                InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
                DataWriter writer = new DataWriter(randomAccessStream.GetOutputStreamAt(0));
                writer.WriteBytes(Thumbnail.ImageBytes);
                writer.StoreAsync();
                Image = new BitmapImage();
                Image.SetSource(randomAccessStream);
            }
        }
    }
}
