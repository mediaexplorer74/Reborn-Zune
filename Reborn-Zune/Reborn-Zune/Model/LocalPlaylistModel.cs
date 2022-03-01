using GalaSoft.MvvmLight;
using Reborn_Zune.Model.Interface;
using Reborn_Zune.ViewModel;
using Reborn_Zune_MusicLibraryService.DataModel;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalPlaylistModel : ObservableObject, ILocalListModel
    {
        public LocalPlaylistModel()
        {
            Musics = new ObservableCollection<LocalMusicModel>();
        }


        private MLPlayListModel _playlist;
        public MLPlayListModel Playlist
        {
            get
            {
                return _playlist;
            }
            set
            {
                Set<MLPlayListModel>(() => this.Playlist, ref _playlist, value);
            }
        }

        private ObservableCollection<LocalMusicModel> _musics;
        public ObservableCollection<LocalMusicModel> Musics
        {
            get
            {
                return _musics;
            }
            set
            {
                Set<ObservableCollection<LocalMusicModel>>(() => this.Musics, ref _musics, value);
            }
        }

        public LibraryViewModel LibraryViewModel { get; set; }
        public bool isEditable
        {
            get
            {
                return true;
            }
        }

        public Visibility isVisible
        {
            get
            {
                return Visibility.Collapsed;
            }
        }

        public ImageSource GetImage()
        {
            return new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png"));
        }

        public string GetTitle()
        {
            return Playlist.Name;
        }

        public string GetArtist()
        {
            return "Various Artists";
        }
    }
}
