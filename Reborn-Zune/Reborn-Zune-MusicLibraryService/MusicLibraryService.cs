using Microsoft.Toolkit.Uwp.Helpers;
using Reborn_Zune_Common.Services;
using Reborn_Zune_MusicLibraryService.DataBase;
using Reborn_Zune_MusicLibraryService.DataModel;
using Reborn_Zune_MusicLibraryService.LibraryDisk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Reborn_Zune_MusicLibraryService
{
    // MusicLibraryService class
    public class MusicLibraryService : IService
    {
        // IsFirstUse
        // bool IsFirstUse => SystemInformation.IsFirstRun;
        private bool IsFirstUse
        {
            get
            {
                return SystemInformation.IsFirstRun;
            }
        }

        // Completed property
        public event EventHandler Completed;

       
        // Library property
        public Library Library { get; set; }


        // 
        public MusicLibraryService()
        {
            Run();
        }

        // Run
        public async void Run()
        {
            InitializeDBMS();

            await LoadLibraryDiskAsync();
            await CreateLibraryInstanceAsync();

            Completed?.Invoke(this, EventArgs.Empty);

            //RnD
            RefreshLibrary();
        }//Run end


        // Clean
        public void Clean()
        {
            DataBaseEngine.Reset();
        }

        #region DBMS (Sealed)

        // InitializeDBMS
        private void InitializeDBMS()
        {
            try
            {
                Debug.WriteLine("DBMS Initialize");
                DataBaseEngine.Initialize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DBMS Initialize exception: " + ex.Message);
            }

        }//InitializeDBMS end


        // CreateLibraryInstanceAsync 
        private async Task CreateLibraryInstanceAsync()
        {
            try
            {
                //RnD
                Library = await DataBaseEngine.FetchAllAsync();
                //Library = DataBaseEngine.FetchAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] CreateLibraryInstanceAsync exception:" + ex.Message);
            }

        }//CreateLibraryInstanceAsync end
        #endregion


        #region LibraryDisk (Sealed)

        // LoadLibraryDiskAsync
        private async Task LoadLibraryDiskAsync()
        {
            try
            {
                Debug.WriteLine("[i] Library Initialize");

                // RnD
                List<KeyValuePair<StorageLibraryChangeType, object>> result = 
                   await LibraryEngine.Initialize(IsFirstUse);                              
                //List<KeyValuePair<StorageLibraryChangeType, object>> result 
                //    = await LibraryEngine.Initialize(true);

                foreach (KeyValuePair<StorageLibraryChangeType, object> i in result)
                {
                    if (i.Value.GetType().Name == "StorageFile") //Add/Update DataBase
                    {
                        //Debug.WriteLine("StorageFile");
                        if (i.Key == StorageLibraryChangeType.ContentsChanged)
                        {
                            //Debug.WriteLine("ContentChanged");
                            await DataBaseEngine.Update(i.Value as StorageFile);
                        }
                        else if (i.Key == StorageLibraryChangeType.MovedIntoLibrary)
                        {
                            //Debug.WriteLine("MovedIntoLibrary");
                            await DataBaseEngine.Add((StorageFile)i.Value);
                        }

                    }
                    else if (i.Value.GetType().Name == "String") //Moved Out
                    {
                        //Debug.WriteLine("Move out");
                        DataBaseEngine.Delete(i.Value.ToString());
                    }
                    else if (i.Value.GetType().Name == "KeyValuePair`2") //Moved or Renamed
                    {
                        //Debug.WriteLine("Moved or Renamed");
                        DataBaseEngine.Update((KeyValuePair<string, string>)i.Value);
                    }
                }

                //RnD : needed or not ?
                //InitializeFinished?.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] LoadLibraryDiskAsync Exception: " + ex.Message);
            }

        }//LoadLibraryDiskAsync end

        #endregion


        #region ServiceOperation

        // AddSongsToPlaylist
        public void AddSongsToPlaylist(string v, List<MLMusicModel> musics)
        {
            DataBaseEngine.AddSongsToPlaylist(v, musics);

            RefreshLibrary();
        
        }//AddSongsToPlaylist end


        // CreatePlaylist
        public bool CreatePlaylist(string playlistName)
        {
            if (!DataBaseEngine.PlaylistNameAvailable(playlistName))
            {
                return false;
            }
            else
            {
                DataBaseEngine.CreatePlaylist(playlistName);

                RefreshLibrary();
                
                return true;
            }

        }//CreatePlaylist end


        // EditPlaylistName
        public void EditPlaylistName(string oldName, string newName)
        {
            DataBaseEngine.EditPlaylistName(oldName, newName);

            RefreshLibrary();

        }//EditPlaylistName end 


        // DeletePlaylist 
        public void DeletePlaylist(string name)
        {
            DataBaseEngine.DeletePlaylist(name);

            RefreshLibrary();

        }//DeletePlaylist end 


        // RemoveSongsFromPlaylist
        public void RemoveSongsFromPlaylist(string playlistName, List<MLMusicModel> musics)
        {
            DataBaseEngine.RemoveSongsFromPlaylist(playlistName, musics);

            RefreshLibrary();

        }//RemoveSongsFromPlaylist end


        // RefreshLibrary
        //private -> public 
            
        public void RefreshLibrary()
        {
            Library.MInP = new ObservableCollection<MLMusicInPlaylistModel>(DataBaseEngine.FetchSongPlaylistRelationship().Select(m => new MLMusicInPlaylistModel(m)).ToList());

            Library.Playlists = new ObservableCollection<MLPlayListModel>(DataBaseEngine.FetchPlaylist().Select(p => new MLPlayListModel(p)).ToList());
        
        }//RefreshLibrary end

        #endregion

    }//MusicLibraryService class end

}//namespace end
