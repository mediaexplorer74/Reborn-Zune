using GalaSoft.MvvmLight;
using Reborn_Zune_MusicLibraryEFCoreModel;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class MLMusicInPlaylistModel : ObservableObject, IMLDataModel
    {
        public MLMusicInPlaylistModel(MusicInPlaylist mInP)
        {
            UnwrapDataFields(mInP);
        }

        public string MusicId { get; set; }
        public string PlaylistId { get; set; }

        public void UnwrapDataFields(IEFCoreModel model)
        {
            var minp = model as MusicInPlaylist;
            this.MusicId = minp.MusicId;
            this.PlaylistId = minp.PlaylistId;
        }
    }
}
