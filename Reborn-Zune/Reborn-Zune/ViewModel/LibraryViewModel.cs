using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using Reborn_Zune.Model.Interface;
using Reborn_Zune_Common.Services;
using Reborn_Zune_MusicLibraryService;
using Reborn_Zune_MusicLibraryService.DataModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.ViewModel
{
    public class LibraryViewModel : ViewModelBase
    {
        private const string UNKNOWN_ARTIST = "Unknown Artist";
        private const string UNKNOWN_ALBUM = "Unknown Album";
        private const string UNKNOWN_YEAR = "Unknown Year";
        public LibraryViewModel()
        {
            Thumbnails = new ObservableCollection<BitmapImage>();
            Musics = new ObservableCollection<LocalMusicModel>();
            Albums = new ObservableCollection<LocalAlbumModel>();
            Artists = new ObservableCollection<LocalArtistModel>();
            Playlists = new ObservableCollection<LocalPlaylistModel>();

            Service.Completed += Service_Completed;
        }

        private void Service_Completed(object sender, EventArgs e)
        {
            BuildLibraryTree();
        }

        private void buildLocalPlaylistModels()
        {
            foreach (var list in Service.Library.Playlists)
            {
                var playlist = new LocalPlaylistModel
                {
                    Playlist = Service.Library.Playlists.Where(p => p.Id == list.Id).FirstOrDefault(),
                };
                Playlists.Add(playlist);
            }

            foreach (var pair in Service.Library.MInP)
            {
                var playlist = Playlists.Where(p => p.Playlist.Id == pair.PlaylistId).FirstOrDefault();
                var music = Musics.Where(m => m.Music.Id == pair.MusicId).FirstOrDefault();
                playlist.Musics.Add(music);
            }
        }

        private void buildLocalMusicAlbumArtistModels()
        {
            foreach (var song in Service.Library.Musics)
            {
                var music = new LocalMusicModel
                {
                    Music = song,
                    Image = Service.Library.Thumbnails.Where(t => t.Id == song.ThumbnailId).FirstOrDefault().Image
                };
                Musics.Add(music);


                var privilegedArtist = "";
                if (song.AlbumArtist != UNKNOWN_ARTIST)
                {
                    privilegedArtist = song.AlbumArtist;
                }
                else if (song.Artist != UNKNOWN_ARTIST)
                {
                    privilegedArtist = song.Artist;
                }
                else
                {
                    privilegedArtist = UNKNOWN_ARTIST;
                }

                var album = Albums.Where(a => a.Title == song.AlbumTitle && a.AlbumArtist == privilegedArtist).FirstOrDefault();
                if (album == null)
                {
                    album = new LocalAlbumModel
                    {
                        Title = song.AlbumTitle,
                        AlbumArtist = privilegedArtist,
                        Image = Service.Library.Thumbnails.Where(t => t.Id == music.Music.ThumbnailId).FirstOrDefault().Image,
                        Year = song.Year
                    };
                    album.Musics.Add(music);
                    Albums.Add(album);
                }
                else
                {
                    album.Musics.Add(music);
                }



                var artist = Artists.Where(a => a.Name == privilegedArtist).FirstOrDefault();
                if (artist == null)
                {
                    artist = new LocalArtistModel
                    {
                        Name = privilegedArtist
                    };
                    artist.Albums.Add(album);
                    artist.Musics.Add(music);
                    Artists.Add(artist);
                }
                else
                {
                    if (!artist.Albums.Contains(album))
                    {
                        artist.Albums.Add(album);
                        artist.Musics.Add(music);
                    }
                    else
                    {
                        artist.Musics.Add(music);
                    }
                }
            }
        }

        private void buildLocalThumbnailModels()
        {
            foreach (var thumb in Service.Library.Thumbnails)
            {
                if (thumb.Image.UriSource != new Uri("ms-appx:///Vap-logo-placeholder.jpg"))
                    Thumbnails.Add(thumb.Image);
            }
        }

        private void BuildLibraryTree()
        {
            try
            {
                buildLocalMusicAlbumArtistModels();
                buildLocalPlaylistModels();
                buildLocalThumbnailModels();

                foreach (var album in Albums)
                {
                    album.LibraryViewModel = this;
                }

                foreach (var playlist in Playlists)
                {
                    playlist.LibraryViewModel = this;
                }

                Albums = new ObservableCollection<LocalAlbumModel>(Albums.OrderBy(a => a.Title).ToList());

                RaisePropertyChanged(nameof(hasPlaylistReverse));
                RaisePropertyChanged(nameof(hasPlaylist));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }


        public MusicLibraryService Service
        {
            get { return ServiceLocator.GetInstance("MusicLibraryService") as MusicLibraryService; }
        }

        #region Properties
        private ObservableCollection<BitmapImage> _thumbnails;
        public ObservableCollection<BitmapImage> Thumbnails
        {
            get
            {
                return _thumbnails;
            }
            set
            {
                Set<ObservableCollection<BitmapImage>>(() => this.Thumbnails, ref _thumbnails, value);
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
        private ObservableCollection<LocalArtistModel> _artists;
        public ObservableCollection<LocalArtistModel> Artists
        {
            get
            {
                return _artists;
            }
            set
            {
                Set<ObservableCollection<LocalArtistModel>>(() => this.Artists, ref _artists, value);
            }
        }

        private ObservableCollection<LocalPlaylistModel> _playlists;
        public ObservableCollection<LocalPlaylistModel> Playlists
        {
            get
            {
                return _playlists;
            }
            set
            {
                Set<ObservableCollection<LocalPlaylistModel>>(() => this.Playlists, ref _playlists, value);
            }
        }
        #endregion



        public bool CreatePlaylist(string text)
        {
            bool result = Service.CreatePlaylist(text);
            if (result == true)
            {
                UpdatePlaylists();
                RaisePropertyChanged(nameof(hasPlaylistReverse));
                RaisePropertyChanged(nameof(hasPlaylist));
            }
            return result;
        }

        private void UpdatePlaylists()
        {
            try
            {
                List<LocalPlaylistModel> list = new List<LocalPlaylistModel>();

                foreach (var playlist in Service.Library.Playlists)
                {
                    if (!Playlists.Select(p => p.Playlist.Id).ToList().Contains(playlist.Id))
                    {
                        var playlistModel = new LocalPlaylistModel
                        {
                            Playlist = playlist
                        };
                        Playlists.Add(playlistModel);
                        list.Add(playlistModel);
                    }
                }

                foreach (var pair in Service.Library.MInP)
                {
                    var playlist = Playlists.Where(p => p.Playlist.Id == pair.PlaylistId).FirstOrDefault();
                    var music = Musics.Where(m => m.Music.Id == pair.MusicId).FirstOrDefault();
                    if (!playlist.Musics.Contains(music))
                    {
                        playlist.Musics.Add(music);
                    }
                }

                Playlists = new ObservableCollection<LocalPlaylistModel>(Playlists.OrderBy(p => p.Playlist.Name).ToList());

                foreach (var playlist in Playlists)
                {
                    playlist.LibraryViewModel = this;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public Visibility hasPlaylist
        {
            get
            {
                return Playlists.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility hasPlaylistReverse
        {
            get
            {
                return Playlists.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private ILocalListModel _albumAddToPlaylist;
        public ILocalListModel AlbumAddToPlaylist
        {
            get
            {
                return _albumAddToPlaylist;
            }
            set
            {
                if(_albumAddToPlaylist != value)
                {
                    _albumAddToPlaylist = value;
                    RaisePropertyChanged(() => AlbumAddToPlaylist);
                }
            }
        }

        public void AddSongsToPlaylist(string playlistName, List<LocalMusicModel> enumerable)
        {
            var playlist = Playlists.Where(p => p.Playlist.Name == playlistName).FirstOrDefault();
            foreach (var music in enumerable)
            {
                playlist.Musics.Add(music);
            }
            Service.AddSongsToPlaylist(playlistName, enumerable.Select(e => e.Music).ToList());
            RaisePropertyChanged(() => Playlists);
        }

        public void SortAlbums(string selected)
        {
            switch (selected)
            {
                case "A-Z":
                    Albums = new ObservableCollection<LocalAlbumModel>(Albums.OrderBy(a => a.Title).ToList());
                    break;
                case "Z-A":
                    Albums = new ObservableCollection<LocalAlbumModel>(Albums.OrderByDescending(a => a.Title).ToList());
                    break;
                case "Artist":
                    Albums = new ObservableCollection<LocalAlbumModel>(Albums.OrderBy(a => a.AlbumArtist).ToList());
                    break;
            }
        }

        public void SortPlaylists(string selected)
        {
            switch (selected)
            {
                case "A-Z":
                    Playlists = new ObservableCollection<LocalPlaylistModel>(Playlists.OrderBy(p => p.Playlist.Name).ToList());
                    break;
                case "Z-A":
                    Playlists = new ObservableCollection<LocalPlaylistModel>(Playlists.OrderByDescending(p => p.Playlist.Name).ToList());
                    break;
            }
        }
    }
}