using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace Reborn_Zune_MusicLibraryService.LibraryDisk
{
    static class LibraryEngine
    {
        public static async Task<List<KeyValuePair<StorageLibraryChangeType, object>>> Initialize(bool IsFirstUse)
        {
            if (IsFirstUse)
            {
                return await LoadLibrary();
            }
            else
            {
                return await LoadChanges();
            }
        }

        private static async Task<List<KeyValuePair<StorageLibraryChangeType, object>>> LoadLibrary()
        {
            List<KeyValuePair<StorageLibraryChangeType, object>> changes = new List<KeyValuePair<StorageLibraryChangeType, object>>();
            try
            {
                QueryOptions queryOption = new QueryOptions
                (CommonFileQuery.OrderByTitle, new string[] { ".mp3", ".m4a", ".mp4" });

                queryOption.FolderDepth = FolderDepth.Deep;

                Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

                IReadOnlyList<StorageFile> files = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions
                  (queryOption).GetFilesAsync();

                foreach (StorageFile file in files)
                {
                    changes.Add(
                        new KeyValuePair<StorageLibraryChangeType, object>(StorageLibraryChangeType.MovedIntoLibrary, file)
                    );
                }


                StorageLibrary musicsLib = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
                StorageLibraryChangeTracker musicTracker = musicsLib.ChangeTracker;
                musicTracker.Enable();

                Debug.WriteLine("Get songs succeed");
                return changes;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return changes;
            }

        }

        private static async Task<List<KeyValuePair<StorageLibraryChangeType, object>>> LoadChanges()
        {
            List<KeyValuePair<StorageLibraryChangeType, object>> changes = new List<KeyValuePair<StorageLibraryChangeType, object>>();
            try
            {
                StorageLibrary musicLibray = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
                musicLibray.ChangeTracker.Enable();
                StorageLibraryChangeReader musicChangeReader = musicLibray.ChangeTracker.GetChangeReader();
                IReadOnlyList<StorageLibraryChange> changeSet = await musicChangeReader.ReadBatchAsync();


                //Below this line is for the blog post. Above the line is for the magazine
                foreach (StorageLibraryChange change in changeSet)
                {
                    if (change.ChangeType == StorageLibraryChangeType.ChangeTrackingLost)
                    {
                        //We are in trouble. Nothing else is going to be valid.
                        Debug.WriteLine("Tracking lost");
                        musicLibray.ChangeTracker.Reset();
                        return changes;
                    }
                    if (change.IsOfType(StorageItemTypes.Folder))
                    {
                        Debug.WriteLine("Folder changes detected");
                    }
                    else if (change.IsOfType(StorageItemTypes.File))
                    {
                        Debug.WriteLine("File changes detected");
                        switch (change.ChangeType)
                        {
                            case StorageLibraryChangeType.ContentsChanged:
                                StorageFile file = await change.GetStorageItemAsync() as StorageFile;
                                changes.Add(new KeyValuePair<StorageLibraryChangeType, object>(change.ChangeType, file));
                                break;
                            case StorageLibraryChangeType.MovedOrRenamed:
                                changes.Add(new KeyValuePair<StorageLibraryChangeType, object>(change.ChangeType,
                                    new KeyValuePair<string, string>(change.Path, change.PreviousPath)));
                                break;
                            case StorageLibraryChangeType.MovedOutOfLibrary:
                                changes.Add(new KeyValuePair<StorageLibraryChangeType, object>(change.ChangeType, change.Path));
                                break;
                            case StorageLibraryChangeType.MovedIntoLibrary:
                                StorageFile File = await change.GetStorageItemAsync() as StorageFile;
                                changes.Add(new KeyValuePair<StorageLibraryChangeType, object>(change.ChangeType, File));
                                break;
                        }
                    }
                }
                await musicChangeReader.AcceptChangesAsync();
                Debug.WriteLine("Get changes succeed");
                return changes;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return changes;
            }

        }
    }
}
