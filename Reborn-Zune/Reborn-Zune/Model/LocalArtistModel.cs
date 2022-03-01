using GalaSoft.MvvmLight;
using Reborn_Zune.Model.Interface;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Reborn_Zune.Model
{
    public class LocalArtistModel : ObservableObject, ILocalListModel
    {
        #region Constructor
        public LocalArtistModel()
        {
            Albums = new ObservableCollection<LocalAlbumModel>();
            Musics = new ObservableCollection<LocalMusicModel>();
        }
        #endregion

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if(_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
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

        private ObservableCollection<LocalAlbumModel> _albums;
        public ObservableCollection<LocalAlbumModel> Albums
        {
            get
            {
                return _albums;
            }
            set
            {
                Set<ObservableCollection<LocalAlbumModel>>(() => this.Albums, ref _albums, value);
            }
        }

        public bool isEditable => throw new System.NotImplementedException();

        public Visibility isVisible => throw new System.NotImplementedException();

        public ImageSource GetImage()
        {
            throw new System.NotImplementedException();
        }

        public string GetTitle()
        {
            throw new System.NotImplementedException();
        }

        public string GetArtist()
        {
            throw new System.NotImplementedException();
        }
    }
}
