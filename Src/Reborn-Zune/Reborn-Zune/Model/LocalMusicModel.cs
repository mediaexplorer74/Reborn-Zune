using GalaSoft.MvvmLight;
using Reborn_Zune_MusicLibraryService.DataModel;
using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalMusicModel : ObservableObject
    {
        public const String MediaItemIdKey = "mediaItemId";

        private MLMusicModel _music;

        public MLMusicModel Music
        {
            get
            {
                return _music;
            }
            set
            {
                Set<MLMusicModel>(() => this.Music, ref _music, value);
            }
        }

        private BitmapImage _image;

        public BitmapImage Image
        {
            get
            {
                return _image;
            }
            set
            {
                Set<BitmapImage>(() => this.Image, ref _image, value);
            }
        }

        public String GetMediaItemIdKey
        {
            get
            {
                return MediaItemIdKey;
            }
        }

        public MediaPlaybackItem ToPlaybackItem()
        {
            var source = MediaSource.CreateFromStorageFile(Music.File);

            var playbackItem = new MediaPlaybackItem(source);

            var displayProperties = playbackItem.GetDisplayProperties();

            playbackItem.ApplyDisplayProperties(displayProperties);

            source.CustomProperties[GetMediaItemIdKey] = Music.Id;

            return playbackItem;
        }
    }
}
