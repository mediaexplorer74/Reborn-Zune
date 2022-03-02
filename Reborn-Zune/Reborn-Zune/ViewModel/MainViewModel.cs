using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using Reborn_Zune.Model.Interface;
using Reborn_Zune.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
#pragma warning disable 0169
namespace Reborn_Zune.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        
        public CoreDispatcher dispatcher;
        
        private LibraryViewModel _libraryViewModel;
        private PlayerViewModel _playerViewModel;
        private DetailViewModel _detailViewmodel;
        
        //private TileViewModel _tileViewModel;
        
        public MediaPlayer _player = PlaybackService.Instance.Player;
        private ILocalListModel _clickedList;
        private ObservableCollection<LocalAlbumModel> _albumList;
        private ObservableCollection<LocalPlaylistModel> _playlistList;
        private ObservableCollection<string> _playlistNameList;
        private Visibility _floatingVisible = Visibility.Collapsed;
        
        public bool _isStop = true;

        #endregion


        #region Constructor
        public MainViewModel(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            LibraryViewModel = new LibraryViewModel();
            PlayerViewModel = new PlayerViewModel(_player, this.dispatcher);
        }

        // LibraryViewModel_InitializeFinished
        private void LibraryViewModel_InitializeFinished(object sender, EventArgs e)
        {
            PlaylistNameList = 
                new ObservableCollection<string>(
                    LibraryViewModel.Playlists.Select(p => p.Playlist.Name).ToList()
                    );

        }//LibraryViewModel_InitializeFinished end


        // SetMediaList
        public void SetMediaList()
        {
            if (PlayBackListConsistencyDetect(DetailViewModel.Musics))
            {
                PlaybackList = ToPlayBackList(DetailViewModel.Musics);
            }

            PlayerViewModel.MediaList = 
                    new MediaListViewModel(DetailViewModel.Musics, PlaybackList, dispatcher);

        }//SetMediaList end


        // SetMediaList 
        public void SetMediaList(ILocalListModel model)
        {
            if (PlayBackListConsistencyDetect(model.Musics))
                PlaybackList = ToPlayBackList(model.Musics);
            PlayerViewModel.MediaList = 
                new MediaListViewModel(model.Musics, PlaybackList, dispatcher);

        }//SetMediaList end

        #endregion


        #region Properties

        // PlayerViewModel property
        public PlayerViewModel PlayerViewModel
        {
            get
            {
                return _playerViewModel;
            }
            set
            {
                _playerViewModel = value;
                RaisePropertyChanged(() => PlayerViewModel);
            }
        }

        // LibraryViewModel
        public LibraryViewModel LibraryViewModel
        {
            get
            {
                return _libraryViewModel;
            }
            set
            {
                if (_libraryViewModel != value)
                {
                    _libraryViewModel = value;
                    RaisePropertyChanged(() => LibraryViewModel);
                }
            }
        }

        // DetailViewModel
        public DetailViewModel DetailViewModel
        {
            get
            {
                return _detailViewmodel;
            }
            set
            {
                if(_detailViewmodel != value)
                {
                    _detailViewmodel = value;
                    RaisePropertyChanged(() => DetailViewModel);
                }
            }
        }

        // PlaybackList
        public MediaPlaybackList PlaybackList
        {
            get { return _player.Source as MediaPlaybackList; }
            set { _player.Source = value; }
        }


        // ClickedList
        public ILocalListModel ClickedList
        {
            get
            {
                return _clickedList;
            }
            set
            {
                if(_clickedList != value)
                {
                    _clickedList = value;
                    RaisePropertyChanged(() => ClickedList);
                }
            }
        }

        // AlbumList
        public ObservableCollection<LocalAlbumModel> AlbumList
        {
            get
            {
                return _albumList;
            }
            set
            {
                if(_albumList != value)
                {
                    _albumList = value;
                    RaisePropertyChanged(() => AlbumList);
                }
            }
        }

        // PlaylistList
        public ObservableCollection<LocalPlaylistModel> PlaylistList
        {
            get
            {
                return _playlistList;
            }
            set
            {
                if(_playlistList != value)
                {
                    _playlistList = value;
                    RaisePropertyChanged(() => PlaylistList);
                }
            }
        }

        // PlaylistNameList
        public ObservableCollection<string> PlaylistNameList
        {
            get
            {
                return _playlistNameList;
            }
            set
            {
                if(_playlistNameList != value)
                {
                    _playlistNameList = value;
                    RaisePropertyChanged(() => PlaylistNameList);
                }
            }
        }

        // FloatingVisible
        public Visibility FloatingVisible
        {
            get
            {
                return _floatingVisible;
            }
            set
            {
                if (_floatingVisible != value)
                {
                    _floatingVisible = value;
                    RaisePropertyChanged(() => FloatingVisible);
                }
            }
        }

        #endregion



        #region Helpers

        //RnD
        //public void CreatTiles()
        //{
        //    TileViewModel.CreateTiles(LibraryViewModel.Thumbnails);
        //}

        //
        public MediaPlaybackList ToPlayBackList(ObservableCollection<LocalMusicModel> musics)
        {
            var playbackList = new MediaPlaybackList();


            // Add playback items to the list
            foreach (var mediaItem in musics)
            {
                playbackList.Items.Add(mediaItem.ToPlaybackItem());
            }

            return playbackList;
        }

        //
        public void SetClickList(ILocalListModel clickedItem)
        {
            if(clickedItem is LocalAlbumModel)
            {
                var item = clickedItem as LocalAlbumModel;
                DetailViewModel = new DetailViewModel(item.Image, item.Title, item.AlbumArtist, item.Year, item.Musics, LibraryViewModel.Playlists);

            }
            else
            {
                var item = clickedItem as LocalPlaylistModel;
                DetailViewModel = new DetailViewModel(new BitmapImage(new Uri("ms-appx:///Assets/Vap-logo-placeholder.jpg")), item.Playlist.Name, "Various Artist", "",item.Musics, LibraryViewModel.Playlists);
            }
        }

        //
        private bool PlayBackListConsistencyDetect(ObservableCollection<LocalMusicModel> currentList)
        {
            if (PlaybackList == null)
                return true;

            // Verify consistency of the lists that were passed in
            System.Collections.Generic.IEnumerable<string> mediaListIds = currentList.Select(i => i.Music.Id);
            var playbackListIds = PlaybackList.Items.Select(
                i => (string)i.Source.CustomProperties.SingleOrDefault(
                    p => p.Key == LocalMusicModel.MediaItemIdKey).Value);

            if (!mediaListIds.SequenceEqual(playbackListIds))
                return true;

            return false;

        }

        //
        public void ShuffleAll()
        {
            if (PlayBackListConsistencyDetect(LibraryViewModel.Musics))
                PlaybackList = ToPlayBackList(LibraryViewModel.Musics);
            PlayerViewModel.MediaList = new MediaListViewModel(LibraryViewModel.Musics, PlaybackList, dispatcher);
        }

        //

        public void ShuffleAllPlaylists()
        {
            if (PlayBackListConsistencyDetect(new ObservableCollection<LocalMusicModel>(LibraryViewModel.Playlists.Select(m => m.Musics).SelectMany(a => a).ToList())))
                PlaybackList = ToPlayBackList(new ObservableCollection<LocalMusicModel>(LibraryViewModel.Playlists.Select(m => m.Musics).SelectMany(a => a).ToList()));
            PlayerViewModel.MediaList = new MediaListViewModel(new ObservableCollection<LocalMusicModel>(LibraryViewModel.Playlists.Select(m => m.Musics).SelectMany(a => a).ToList()), PlaybackList, dispatcher);
        }

        #endregion


    }
}
