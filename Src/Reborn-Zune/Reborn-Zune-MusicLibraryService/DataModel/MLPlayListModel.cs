using GalaSoft.MvvmLight;
using Reborn_Zune_MusicLibraryEFCoreModel;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class MLPlayListModel : ObservableObject, IMLDataModel
    {
        public MLPlayListModel(Playlist playlist)
        {
            UnwrapDataFields(playlist);
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public void UnwrapDataFields(IEFCoreModel model)
        {
            var pl = model as Playlist;
            this.Id = pl.Id;
            this.Name = pl.Name;
        }
    }
}
