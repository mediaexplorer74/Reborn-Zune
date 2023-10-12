using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Media.Animation;
using Reborn_Zune.Model;
using Reborn_Zune.ViewModel;

namespace Reborn_Zune.Control
{
    public sealed class CustomPopUpMediaTransportControl : MediaTransportControls
    {

        public event EventHandler<EventArgs> PlayButtonClicked;
        public event EventHandler<EventArgs> PreviousButtonClicked;
        public event EventHandler<EventArgs> NextButtonClicked;
        public event EventHandler<EventArgs> TilePageButtonClicked;

        Grid TopTextBlocks;
        Storyboard TopStoryboard;
        Image MusicThumbnail;
        TextBlock TopArtistTextBlock;
        TextBlock TopMusicTextBlock;
        TextBlock MusicTitle;
        Button PreviousTrackButton;
        Button PlayPauseButton;
        Button NextTrackButton;
        Button TilePageButton;

        public CustomPopUpMediaTransportControl()
        {
            DefaultStyleKey = typeof(CustomPopUpMediaTransportControl);
        }

        protected override void OnApplyTemplate()
        {
            TopTextBlocks = GetTemplateChild("TopTextBlocks") as Grid;
            TopStoryboard = GetTemplateChild("TopStoryboard") as Storyboard;
            MusicThumbnail = GetTemplateChild("MusicThumbnail") as Image;
            TopArtistTextBlock = GetTemplateChild("TopArtistTextBlock") as TextBlock;
            TopMusicTextBlock = GetTemplateChild("TopMusicTextBlock") as TextBlock;
            MusicTitle = GetTemplateChild("MusicTitle") as TextBlock;
            PreviousTrackButton = GetTemplateChild("PreviousTrackButton") as Button;
            PlayPauseButton = GetTemplateChild("PlayPauseButton") as Button;
            NextTrackButton = GetTemplateChild("NextTrackButton") as Button;
            TilePageButton = GetTemplateChild("TilePageButton") as Button;
            TopStoryboard.Begin();

            PlayPauseButton.IsEnabled = true;

            PreviousTrackButton.Click += PreviousTrackButton_Click;
            PlayPauseButton.Click += PlayPauseButton_Click;
            NextTrackButton.Click += NextTrackButton_Click;
            TilePageButton.Click += TilePageButton_Click;

            base.OnApplyTemplate();
        }

        private void TilePageButton_Click(object sender, RoutedEventArgs e)
        {
            TilePageButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void NextTrackButton_Click(object sender, RoutedEventArgs e)
        {
            NextButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            PlayButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void PreviousTrackButton_Click(object sender, RoutedEventArgs e)
        {
            PreviousButtonClicked?.Invoke(this, EventArgs.Empty);
        }



        public MediaItemViewModel Model
        {
            get { return (MediaItemViewModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(MediaItemViewModel), typeof(CustomPopUpMediaTransportControl), new PropertyMetadata(null, onMediaModelChanged));



        private static void onMediaModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CustomPopUpMediaTransportControl)d).MetaDataChanged(e.NewValue as MediaItemViewModel);

        }

        private void MetaDataChanged(MediaItemViewModel model)
        {
            //if(model != null)
            //{
            //    if (MusicThumbnail != null)
            //    {
            //        MusicThumbnail.Source = model.MediaItem.Music.Thumbnail.Image;
            //    }
            //    if (TopMusicTextBlock != null)
            //    {
            //        TopMusicTextBlock.Text = "Now Playing: " + model.MediaItem.Music.Title;
            //    }
            //    if (TopArtistTextBlock != null)
            //    {
            //        //TopArtistTextBlock.Text = "Artist: " + model.MediaItem.Music.Artist.Name;
            //        TopArtistTextBlock.Text = "Artist: ";
            //    }
            //    if (MusicTitle != null)
            //    {
            //        MusicTitle.Text = model.MediaItem.Music.Title;
            //    }
            //}
            
        }
    }
}
