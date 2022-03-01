using GalaSoft.MvvmLight;
using Reborn_Zune.Model.Interface;
using Reborn_Zune.ViewModel;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalAlbumModel : ObservableObject, ILocalListModel
    {
        public LocalAlbumModel()
        {
            Musics = new ObservableCollection<LocalMusicModel>();
        }

        private string _title;
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

        private string _year;
        public string Year
        {
            get
            {
                return _year;
            }
            set
            {
                if(_year != value)
                {
                    _year = value;
                    RaisePropertyChanged(() => Year);
                }
            }
        }

        private string _albumArtist;
        public string AlbumArtist
        {
            get
            {
                return _albumArtist;
            }
            set
            {
                if(_albumArtist != value)
                {
                    _albumArtist = value;
                    RaisePropertyChanged(() => AlbumArtist);
                }
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

        private LibraryViewModel _libraryViewModel;
        public LibraryViewModel LibraryViewModel
        {
            get
            {
                return _libraryViewModel;
            }
            set
            {
                Set<LibraryViewModel>(() => this.LibraryViewModel, ref _libraryViewModel, value);
                RaisePropertyChanged(() => LibraryViewModel);
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

        public bool isEditable
        {
            get
            {
                return false;
            }
        }

        public Visibility isVisible
        {
            get
            {
                return Visibility.Visible;
            }
        }

        public ImageSource GetImage()
        {
            return Image;
        }

        public string GetTitle()
        {
            return Title;
        }

        public string GetArtist()
        {
            return AlbumArtist;
        }
    }
}