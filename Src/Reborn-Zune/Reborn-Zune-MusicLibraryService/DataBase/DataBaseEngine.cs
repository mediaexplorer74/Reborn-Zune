// DataBaseEngine

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

using Reborn_Zune_MusicLibraryEFCoreModel;
using Reborn_Zune_MusicLibraryService.DataModel;
using Reborn_Zune_MusicLibraryService.Utility;

using TagLib;
using Windows.UI.Xaml.Media.Imaging;

// Reborn_Zune_MusicLibraryService.DataBase namespace
namespace Reborn_Zune_MusicLibraryService.DataBase
{
    // DataBaseEngine class 
    static class DataBaseEngine
    {
        private const string UNKNOWN_ARTIST = "Unknown Artist";
        private const string UNKNOWN_ALBUM = "Unknown Album";
        private const string UNKNOWN_YEAR = "Unknown Year";

        // Initialize
        public static void Initialize()
        {
            try
            {
                using (var db = new MusicLibraryDbContext())
                {
                    // "Deploy db"
                    db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DatabaseEngine Initialize Exception: " + ex.Message);
            }

        }//Initialize end



        #region Normal Database Operation (CRUD)

        // Add
        public static async Task Add(StorageFile File)
        {
            try
            {
                Debug.WriteLine(File.Name + " Music meta data start retreiving");


                StorageItemThumbnail thumbnail = await File.GetThumbnailAsync(
                    ThumbnailMode.MusicView, 100, ThumbnailOptions.UseCurrentScale);

                MusicProperties properties = await File.Properties.GetMusicPropertiesAsync();

                string path = File.Path;

                byte[] bytearray = await ConvertThumbnailToBytesAsync(thumbnail);

                string artistName = properties.Artist != "" ? properties.Artist : UNKNOWN_ARTIST;

                string albumArtistName = properties.AlbumArtist != "" ? properties.AlbumArtist : UNKNOWN_ARTIST;

                string albumTitle = properties.Album != "" ? properties.Album : UNKNOWN_ALBUM;
                string duration = properties.Duration.ToString(@"mm\:ss");

                string albumYear = properties.Year != 0 ? properties.Year.ToString() : UNKNOWN_YEAR;

                string songTitle = properties.Title != "" ? properties.Title
                    : Path.GetFileNameWithoutExtension(File.Path);

                Debug.WriteLine("[i] Access into database (add)");
                using (var _context = new MusicLibraryDbContext())
                {
                    var thumb = new Thumbnail
                    {
                        ImageBytes = bytearray,
                        Id = Guid.NewGuid().ToString()
                    };
                    _context.Thumbnails.Add(thumb);
                    _context.SaveChanges();
                    Debug.WriteLine("Thumbnail Done");

                    Music Music = new Music
                    {
                        Path = path,
                        Title = songTitle,
                        AlbumTitle = albumTitle,
                        Artist = artistName,
                        AlbumArtist = albumArtistName,
                        Year = albumYear,
                        ThumbnailId = thumb.Id,
                        Duration = duration,

                        Id = Guid.NewGuid().ToString()
                    };
                    _context.Musics.Add(Music);
                    _context.SaveChanges();
                    Debug.WriteLine("Music Done");
                }
                Debug.WriteLine("DataBase Add Succeed");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DatabaseEngine Add Exception: " + ex.Message);
            }

        }//Add end


        // Delete
        public static void Delete(string path)
        {
            try
            {
                Debug.WriteLine("[i] Access into database (item delete)");
                using (var _context = new MusicLibraryDbContext())
                {
                    var music = _context.Musics.Where(m => m.Path == path).First();
                    _context.Musics.Remove(music);
                    _context.SaveChanges();
                }
                Debug.WriteLine("[i] Delete item (from Database) Succeed");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DatabaseEngine (Delete item) Exception: " + ex.Message);
            }

        }//Delete end


        // Update 
        public static async Task Update(StorageFile File)
        {
            try
            {
                Debug.WriteLine("[i]" + File.Name + " Music meta data start retreiving");

                var properties = await File.Properties.GetMusicPropertiesAsync();
                var fileStream = await File.OpenStreamForReadAsync();

                var tagFile = TagLib.File.Create(new StreamFileAbstraction(File.Name,
                                 fileStream, fileStream));

                var types = tagFile.TagTypes;

                if ((types & (TagTypes.Id3v1 | TagTypes.Id3v2)) == (TagTypes.Id3v1 | TagTypes.Id3v2))
                {
                    types = TagTypes.Id3v2;
                }

                var tags = tagFile.GetTag(types);

                var path = File.Path;
                var bytearray = tags.Pictures.Length == 0 ? new byte[] { } : tags.Pictures[0].Data.Data;
                var artistName = properties.Artist != "" ? properties.AlbumArtist : UNKNOWN_ARTIST;
                var albumArtistName = properties.AlbumArtist != null ? properties.AlbumArtist : UNKNOWN_ARTIST;
                var albumTitle = properties.Album != "" ? properties.Album : UNKNOWN_ALBUM;
                var duration = properties.Duration.ToString(@"mm\:ss");
                var albumYear = properties.Year != 0 ? properties.Year.ToString() : UNKNOWN_YEAR;
                var songTitle = properties.Title != "" ? properties.Title : Path.GetFileNameWithoutExtension(File.Path);

                Debug.WriteLine("[i] Access into database (update)");
                using (MusicLibraryDbContext _context = new MusicLibraryDbContext())
                {
                    Music music = _context.Musics.Where(m => m.Path == path).First();
                    Thumbnail thumbnail = _context.Thumbnails.Where(t => t.Id == music.ThumbnailId).First();

                    music.Artist = artistName;
                    music.AlbumArtist = albumArtistName;
                    music.AlbumTitle = albumTitle;
                    music.Title = songTitle;
                    music.Year = albumYear;
                    music.Duration = duration;

                    _context.Musics.Update(music);

                    if (thumbnail.ImageBytes != bytearray)
                    {
                        thumbnail.ImageBytes = bytearray;
                        _context.Thumbnails.Update(thumbnail);
                    }


                    _context.SaveChanges();
                }
                Debug.WriteLine("[i] Database Update Succeed");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DatabaseEngine Update Exception: " + ex.Message);
            }
        
        }//Update end

        // Update
        public static void Update(KeyValuePair<string, string> pathChange)
        {
            try
            {
                Debug.WriteLine("[i] Access into database (update)");
                
                using (MusicLibraryDbContext _context = new MusicLibraryDbContext())
                {
                    Music music = _context.Musics.Where(m => m.Path == pathChange.Key).First();
                    
                    music.Path = pathChange.Value;
                    
                    _context.Musics.Update(music);
                    
                    _context.SaveChanges();
                }
                Debug.WriteLine("Database Music Update Succeed");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DatabaseEngine Update Exception: " + ex.Message);
            }

        }//Update end



        // FetchAllAsync
        public static async Task<Library> FetchAllAsync()
        //public static Library FetchAll()
        {
            Library library = new Library();

            try
            {
                Debug.WriteLine("[i] Access into database (fetch all)");

                // ----------------------------------------------------------
                // Plan B
                ObservableCollection<Music> libMusics = null;
                ObservableCollection<MusicInPlaylist> libMinPs = null;
                ObservableCollection<Playlist> libPlaylists = null;
                ObservableCollection<Thumbnail> libThumbnails = null;
                // ----------------------------------------------------------

                using (MusicLibraryDbContext _context = new MusicLibraryDbContext())
                {
                    
                    try
                    {
                        // Plan A
                        // This code don't wants to work at 15063, sadly :(
                        //IQueryable<MLMusicModel> ml = _context.Musics.Select(m => new MLMusicModel(m));
                        //List<MLMusicModel> dlist = b.ToList();
                        //library.Musics = new ObservableCollection<MLMusicModel>(dlist);
                        
                        // Plan B
                        // * Musics section * -----------------------------------------------------------                        
                        //List<Music> ml = _context.Musics.Where(m => m.Path != null).ToList();
                        //libMusics = new ObservableCollection<Music>(ml);
                        libMusics = new ObservableCollection<Music>
                        (
                            _context.Musics.Where(m => m.Path != null).ToList()
                        );

                        List<MLMusicModel> libMusiclist = new List<MLMusicModel>();
                        foreach (Music libmusic in libMusics)
                        {
                            MLMusicModel mitem = new MLMusicModel(libmusic)
                            {
                                Id = libmusic.Id,
                                Path = libmusic.Path,
                                Title = libmusic.Title,
                                Duration = libmusic.Duration,                                 
                                AlbumTitle= libmusic.Title,
                                AlbumArtist = libmusic.AlbumArtist,
                                Artist = libmusic.Artist,
                                Year = libmusic.Year,
                                ThumbnailId = libmusic.ThumbnailId,
                                
                                //RnD
                                //File = await StorageFile.GetFileFromPathAsync(libmusic.Path), // await 
                            };

                            libMusiclist.Add(mitem);
                        }

                        library.Musics = new ObservableCollection<MLMusicModel>(libMusiclist);
                        // --------------------------------------------------------------
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("[ex] library.Musics Exception: " + ex.Message);
                    }

                    try
                    {
                        //Plan A
                        // This code don't wants to work at 15063, sadly :(
                        //library.MInP = new ObservableCollection<MLMusicInPlaylistModel>(
                        //  _context.MusicInPlaylists.Select(m => new MLMusicInPlaylistModel(m)).ToList()
                        //);

                        // Plan B
                        // ---- * MiP * -----------                        
                        libMinPs = new ObservableCollection<MusicInPlaylist>
                        (
                            _context.MusicInPlaylists.Where(m => m.PlaylistId != null).ToList()
                        );

                        List<MLMusicInPlaylistModel> libMiPlist = new List<MLMusicInPlaylistModel>();
                        foreach (MusicInPlaylist libmip in libMinPs)
                        {
                            MLMusicInPlaylistModel mipitem = new MLMusicInPlaylistModel(libmip)
                            {
                                MusicId = libmip.MusicId,
                                PlaylistId = libmip.PlaylistId,
                               
                            };

                            libMiPlist.Add(mipitem);
                        }
                        library.MInP = new ObservableCollection<MLMusicInPlaylistModel>(libMiPlist);
                        
                        // --------------------------------------------------------------
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("[ex] library.MInP Exception: " + ex.Message);
                    }

                    try
                    {
                        // Plan A
                        // This code don't wants to work at 15063, sadly :(
                        //library.Playlists = new ObservableCollection<MLPlayListModel>
                        //(
                        //  _context.Playlists.Select(p => new MLPlayListModel(p)).ToList()
                        //);

                        // Plan B
                        // ---- * Playlists * -----------
                        
                        libPlaylists = new ObservableCollection<Playlist>
                        (
                            _context.Playlists.Where(m => m.Id != null).ToList()
                        );

                        List<MLPlayListModel> libPlayListlist = new List<MLPlayListModel>();
                        foreach (Playlist libplaylist in libPlaylists)
                        {
                            MLPlayListModel playlistitem = new MLPlayListModel(libplaylist)
                            {
                                Id = libplaylist.Id,
                                Name = libplaylist.Name,                               
                            };

                            libPlayListlist.Add(playlistitem);
                        }
                        library.Playlists = new ObservableCollection<MLPlayListModel>(libPlayListlist);
                        
                        // --------------------------------------------------------------

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("[ex] library.Playlists Exception: " + ex.Message);
                    }

                    try
                    {
                        // Plan A
                        //This code don't wants to work at 15063, sadly :(
                        //library.Thumbnails = new ObservableCollection<MLThumbnailModel>
                        //(
                        //  //_context.Thumbnails.Select(t => new MLThumbnailModel(t)).ToList()
                        //);

                        // Plan B
                        // ---- * Thumbnails * -----------
                        
                        libThumbnails = new ObservableCollection<Thumbnail>
                        (
                            _context.Thumbnails.Where(m => m.Id != null).ToList()
                        );

                        List<MLThumbnailModel> libThumbnaillist = new List<MLThumbnailModel>();
                        foreach (Thumbnail thumbnaillist in libThumbnails)
                        {
                            MLThumbnailModel playlistitem = new MLThumbnailModel(thumbnaillist)
                            {
                                Id = thumbnaillist.Id,
                                ImageBytes = thumbnaillist.ImageBytes,
                                Image = new BitmapImage(new Uri("ms-appx:///Assets/Vap-logo-placeholder.jpg")),
                                //Image = new BitmapImage(),
                                //(new Uri("ms-appx:///Assets/Vap-logo-placeholder.jpg")),//; (thumbnaillist.ImageBytes),
                            };

                            libThumbnaillist.Add(playlistitem);
                        }
                        library.Thumbnails = 
                        new ObservableCollection<MLThumbnailModel>
                        (
                            libThumbnaillist
                        );
                        
                        // --------------------------------------------------------------
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("[ex] library.Thumbnails Exception: " + ex.Message);
                    }
                }

                try
                {
                    foreach (MLMusicModel item in library.Musics)
                    {
                                                                   
                        await item.GetFileAsync(); // await 
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ex] library.Musics Exception:" + ex.Message);
                }

                try
                {
                    
                    foreach (var item in library.Thumbnails)
                    {
                        item.GetBitmapImage();
                    }
                    
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ex] Thumbnails GetBitmapImage Exception:" + ex.Message);
                }

                return library;
            }
            catch (Exception ex1)
            {
                Debug.WriteLine("[ex] DatabaseEngine FetchAllAsync Exception:" + ex1.Message);
            }

            
            return library;

        }//FetchAll(Async) end


        // Reset
        public static void Reset()
        {
            try
            {
                using (var _context = new MusicLibraryDbContext())
                {
                    _context.Database.EnsureDeleted();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("[ex] DatabaseEngine Reset Exception: " + ex.Message);
            }
        }//Reset end

        #endregion

        #region Helper
        public static bool PlaylistNameAvailable(string playlistName)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return (_context.Playlists.Where(p => p.Name == playlistName).FirstOrDefault() == null);
            }
        }
        private static async Task<byte[]> ConvertThumbnailToBytesAsync(StorageItemThumbnail thumbnail)
        {
            byte[] result = new byte[thumbnail.Size];
            using (var reader = new DataReader(thumbnail))
            {
                await reader.LoadAsync((uint)thumbnail.Size);
                reader.ReadBytes(result);
                string base64ImageString = Convert.ToBase64String(result);
                return base64ImageString == Utilities.NULL_THUMBNAIL_BASE64_STRING ? new byte[0] : result;
            }
        }
        #endregion

        #region Playlist Operation
        public static Playlist CreatePlaylist(string playlistName)
        {
            Playlist playlist = new Playlist
            {
                Name = playlistName,
                Id = Guid.NewGuid().ToString()
            };
            using (var _context = new MusicLibraryDbContext())
            {
                _context.Playlists.Add(playlist);
                _context.SaveChanges();
            }

            return playlist;
        }

        public static void EditPlaylistName(string oldPlaylistName, string newPlaylistName)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Name == oldPlaylistName).First();
                playlist.Name = newPlaylistName;
                _context.Playlists.Update(playlist);
                _context.SaveChanges();
            }
        }

        public static void DeletePlaylist(string playlistName)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Name == playlistName).First();
                _context.Playlists.Remove(playlist);
                _context.SaveChanges();
            }
        }

        public static void AddSongsToPlaylist(string playlistName, List<MLMusicModel> musics)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Name == playlistName).First();
                foreach (var item in musics)
                {
                    var mInP = new MusicInPlaylist
                    {
                        MusicId = item.Id,
                        PlaylistId = playlist.Id
                    };
                    _context.MusicInPlaylists.Add(mInP);
                    _context.SaveChanges();
                }

            }
        }

        public static void RemoveSongsFromPlaylist(string playlistName, List<MLMusicModel> musics)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Name == playlistName).First();
                foreach (var item in musics)
                {
                    var mInP = _context.MusicInPlaylists.Where(m => m.PlaylistId == playlist.Id && m.MusicId == item.Id).First();
                    _context.MusicInPlaylists.Remove(mInP);
                }
                _context.SaveChanges();
            }
        }

        public static IList<MusicInPlaylist> FetchSongPlaylistRelationship()
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return _context.MusicInPlaylists.Select(m => m).ToList();
            }
        }

        public static IList<Playlist> FetchPlaylist()
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return _context.Playlists.Select(m => m).ToList();
            }
        }
        #endregion

    }//DataBaseEngine class end

}//namespace end
