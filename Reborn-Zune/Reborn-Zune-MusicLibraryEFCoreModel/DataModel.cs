using System.Collections.Generic;

namespace Reborn_Zune_MusicLibraryEFCoreModel
{
    public class Music : IEFCoreModel
    {
        public Music()
        {
           
        }

        public string Id { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public string Artist { get; set; }
        public string Year { get; set; }
        public string ThumbnailId { get; set; }
        public Thumbnail Thumbnail { get; set; }
        
        public ICollection<MusicInPlaylist> MusicInPlaylists { get; set; }

    }
    public class Playlist : IEFCoreModel
    {
        public Playlist()
        {
            
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<MusicInPlaylist> MusicInPlaylists { get; set; }

    }
    public class MusicInPlaylist : IEFCoreModel
    {
        public string MusicId { get; set; }
        public string PlaylistId { get; set; }
        public Music Music { get; set; }
        public Playlist Playlist { get; set; }
    }
    public class Thumbnail : IEFCoreModel
    {
        public Thumbnail()
        {
        }

        public string Id { get; set; }
        public byte[] ImageBytes { get; set; }
        public ICollection<Music> Musics { get; set; }

        
    }
}
