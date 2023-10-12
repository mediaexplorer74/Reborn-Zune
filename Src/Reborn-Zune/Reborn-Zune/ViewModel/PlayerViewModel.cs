using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Core;

namespace Reborn_Zune.ViewModel
{
    public class PlayerViewModel : ViewModelBase, IDisposable
    {
        bool disposed;
        MediaPlayer player;
        CoreDispatcher dispatcher;
        MediaPlaybackList subscribedPlaybackList;
        MediaListViewModel mediaList;

        bool canSkipNext;
        bool canSkipPrevious;

        public PlaybackSessionViewModel PlaybackSession { get; private set; }

        public PlayerViewModel(MediaPlayer player, CoreDispatcher dispatcher)
        {
            this.player = player;
            this.dispatcher = dispatcher;
            this.player.AudioCategory = MediaPlayerAudioCategory.Media;
        }

        public MediaPlayer GetPlayer()
        {
            return player;
        }

        public bool CanSkipNext
        {
            get { return canSkipNext; }
            set
            {
                if (canSkipNext != value)
                {
                    canSkipNext = value;
                    RaisePropertyChanged("CanSkipNext");
                }
            }
        }

        public bool CanSkipPrevious
        {
            get { return canSkipPrevious; }
            set
            {
                if (canSkipPrevious != value)
                {
                    canSkipPrevious = value;
                    RaisePropertyChanged("CanSkipPrevious");
                }
            }
        }

        public MediaListViewModel MediaList
        {
            get { return mediaList; }
            set
            {
                if (mediaList != value)
                {
                    if (subscribedPlaybackList != null)
                    {
                        subscribedPlaybackList.CurrentItemChanged -= SubscribedPlaybackList_CurrentItemChanged;
                        subscribedPlaybackList.Items.VectorChanged -= Items_VectorChanged;
                        subscribedPlaybackList = null;
                    }

                    mediaList = value;

                    if (mediaList != null)
                    {
                        if (player.Source != mediaList.PlaybackList)
                            player.Source = mediaList.PlaybackList;

                        subscribedPlaybackList = mediaList.PlaybackList;
                        subscribedPlaybackList.CurrentItemChanged += SubscribedPlaybackList_CurrentItemChanged;
                        subscribedPlaybackList.Items.VectorChanged += Items_VectorChanged;
                        HandlePlaybackListChanges(subscribedPlaybackList.Items);
                    }
                    else
                    {
                        CanSkipNext = false;
                        CanSkipPrevious = false;
                    }

                    RaisePropertyChanged("MediaList");
                }
            }
        }

        private void HandlePlaybackListChanges(IObservableVector<MediaPlaybackItem> vector)
        {
            if (vector.Count > 0)
            {
                CanSkipNext = true;
                CanSkipPrevious = true;
            }
            else
            {
                CanSkipNext = false;
                CanSkipPrevious = false;
            }
        }

        private async void Items_VectorChanged(IObservableVector<MediaPlaybackItem> sender, IVectorChangedEventArgs @event)
        {
            if (disposed) return;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (disposed) return;
                HandlePlaybackListChanges(sender);
            });
        }

        private async void SubscribedPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            if (disposed) return;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (disposed) return;
                HandlePlaybackListChanges(sender.Items);
            });
        }

        public void Dispose()
        {
            if (disposed)
                return;

            if (MediaList != null)
            {
                MediaList.Dispose();
                MediaList = null; // Setter triggers vector unsubscribe logic
            }

            PlaybackSession.Dispose();

            disposed = true;
        }

        internal void SetCurrentItem(int selectedIndex)
        {
            MediaList.CurrentItemIndex = selectedIndex;
        }

    }
}
