using Reborn_Zune_MusicLibraryEFCoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public interface IMLDataModel
    {
        void UnwrapDataFields(IEFCoreModel model);
    }
}
