using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class Library : ObservableObject
    {
        public ObservableCollection<MLMusicModel> Musics { get; set; }
        public ObservableCollection<MLPlayListModel> Playlists { get; set; }
        public ObservableCollection<MLThumbnailModel> Thumbnails { get; set; }
        public ObservableCollection<MLMusicInPlaylistModel> MInP { get; set; }
    }
}
