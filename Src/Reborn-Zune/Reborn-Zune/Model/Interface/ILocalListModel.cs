using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Reborn_Zune.Model.Interface
{
    public interface ILocalListModel
    {
        bool isEditable { get; }

        Visibility isVisible { get; }

        ImageSource GetImage();

        string GetTitle();

        string GetArtist();

        ObservableCollection<LocalMusicModel> Musics { get; set; }
    }
}
