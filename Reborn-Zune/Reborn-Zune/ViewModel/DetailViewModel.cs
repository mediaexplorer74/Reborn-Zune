using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.ViewModel
{
    public class DetailViewModel : ViewModelBase
    {
        private BitmapImage _thumbnail;
        private string _title;
        private string _artist;
        private string _albumYear;
        private ObservableCollection<LocalMusicModel> _musics;
        private bool _animate = false;
        private ObservableCollection<LocalPlaylistModel> _playlists;

        public DetailViewModel(BitmapImage image, string title, string artist, string year, ObservableCollection<LocalMusicModel> musics, ObservableCollection<LocalPlaylistModel> playlists)
        {
            Thumbnail = image;
            Title = title;
            Artist = artist;
            AlbumYear = year;
            Musics = musics;
            Playlists = playlists;
        }

        public ObservableCollection<LocalPlaylistModel> Playlists
        {
            get
            {
                return _playlists;
            }
            set
            {
                if(_playlists != value)
                {
                    _playlists = value;
                    RaisePropertyChanged(() => Playlists);
                }
            }
        }

        public BitmapImage Thumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                if(_thumbnail != value)
                {
                    _thumbnail = value;
                    RaisePropertyChanged(() => Thumbnail);
                }
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if(_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        public string Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                if(_artist != value)
                {
                    _artist = value;
                    RaisePropertyChanged(() => Artist);
                }
            }
        }

        public ObservableCollection<LocalMusicModel> Musics
        {
            get
            {
                return _musics;
            }
            set
            {
                if(_musics != value)
                {
                    _musics = value;
                    RaisePropertyChanged(() => Musics);
                }
            }
        }

        public string AlbumYear
        {
            get
            {
                return _albumYear;
            }
            set
            {
                if(_albumYear != AlbumYear)
                {
                    _albumYear = value;
                    RaisePropertyChanged(() => AlbumYear);
                }
            }
        }

        public bool Animate
        {
            get
            {
                return _animate;
            }
            set
            {
                if(_animate != value)
                {
                    _animate = value;
                    RaisePropertyChanged(() => Animate);
                }
            }
        }
    }
}
