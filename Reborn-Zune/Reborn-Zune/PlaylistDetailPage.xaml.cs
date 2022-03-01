using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Reborn_Zune.Model;
using Reborn_Zune.Utilities;
using Reborn_Zune.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaylistDetailPage : Page
    {
        public MainViewModel MainVM { get; set; }
        private Compositor _compositor;
        private Visual _floatingVisual;
        private Visual _playButtonVisual;
        private Visual _addToButtonVisual;
        private Visual _exitButtonVisual;
        private Visual _musicListVisual;
        private Visual _albumYearTextBlockVisual;
        private Visual _albumNameTextBlockVisual;
        private Visual _titleTextBlockVisual;
        private ConnectedAnimation animation;

        public PlaylistDetailPage()
        {
            this.InitializeComponent();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _floatingVisual = PlayerFloating.GetVisual();
            _playButtonVisual = PlayButton.GetVisual();
            _addToButtonVisual = AddToButton.GetVisual();
            _exitButtonVisual = ExitButton.GetVisual();
            _musicListVisual = list.GetVisual();
            _albumYearTextBlockVisual = AlbumYearTextBlock.GetVisual();
            _albumNameTextBlockVisual = AlbumNameTextBlock.GetVisual();
            _titleTextBlockVisual = TitleTextBlock.GetVisual();



           
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainVM = e.Parameter as MainViewModel;
            if (!MainVM.DetailViewModel.Animate)
            {
                //Wave 1st
                var animGroup1 = _compositor.CreateAnimationGroup();
                var fadeIn1 = _compositor.CreateScalarKeyFrameAnimation();
                fadeIn1.Target = "Opacity";
                fadeIn1.Duration = TimeSpan.FromMilliseconds(500);
                fadeIn1.InsertKeyFrame(0, 0);
                fadeIn1.InsertKeyFrame(1, 1);
                fadeIn1.DelayTime = TimeSpan.FromMilliseconds(400);
                var offSet1 = _compositor.CreateVector3KeyFrameAnimation();
                offSet1.Target = "Translation";
                offSet1.Duration = TimeSpan.FromMilliseconds(400);
                offSet1.InsertKeyFrame(0f, new Vector3(-10, 0, 0));
                offSet1.InsertKeyFrame(1f, new Vector3(0, 0, 0));
                offSet1.DelayTime = TimeSpan.FromMilliseconds(400);

                animGroup1.Add(fadeIn1);
                animGroup1.Add(offSet1);

                //Wave 2nd
                var animGroup2 = _compositor.CreateAnimationGroup();
                var fadeIn2 = _compositor.CreateScalarKeyFrameAnimation();
                fadeIn2.Target = "Opacity";
                fadeIn2.Duration = TimeSpan.FromMilliseconds(500);
                fadeIn2.InsertKeyFrame(0, 0);
                fadeIn2.InsertKeyFrame(1, 1);
                fadeIn2.DelayTime = TimeSpan.FromMilliseconds(650);
                var offSet2 = _compositor.CreateVector3KeyFrameAnimation();
                offSet2.Target = "Translation";
                offSet2.Duration = TimeSpan.FromMilliseconds(400);
                offSet2.InsertKeyFrame(0f, new Vector3(-10, 0, 0));
                offSet2.InsertKeyFrame(1f, new Vector3(0, 0, 0));
                offSet2.DelayTime = TimeSpan.FromMilliseconds(550);
                animGroup2.Add(fadeIn2);
                animGroup2.Add(offSet2);


                //Wave 3rd
                var animGroup3 = _compositor.CreateAnimationGroup();
                var fadeIn3 = _compositor.CreateScalarKeyFrameAnimation();
                fadeIn3.Target = "Opacity";
                fadeIn3.Duration = TimeSpan.FromMilliseconds(500);
                fadeIn3.InsertKeyFrame(0, 0);
                fadeIn3.InsertKeyFrame(1, 1);
                fadeIn3.DelayTime = TimeSpan.FromMilliseconds(800);
                var offSet3 = _compositor.CreateVector3KeyFrameAnimation();
                offSet3.Target = "Translation";
                offSet3.Duration = TimeSpan.FromMilliseconds(400);
                offSet3.InsertKeyFrame(0f, new Vector3(-10, 0, 0));
                offSet3.InsertKeyFrame(1f, new Vector3(0, 0, 0));
                offSet3.DelayTime = TimeSpan.FromMilliseconds(700);
                animGroup3.Add(fadeIn3);
                animGroup3.Add(offSet3);


                var fadeOut = _compositor.CreateScalarKeyFrameAnimation();
                fadeOut.Target = "Opacity";
                fadeOut.Duration = TimeSpan.FromMilliseconds(500);
                fadeOut.DelayTime = TimeSpan.FromMilliseconds(500);
                fadeOut.InsertKeyFrame(0, 1);
                fadeOut.InsertKeyFrame(1, 0);

                ElementCompositionPreview.SetImplicitShowAnimation(PlayButton, animGroup2);
                ElementCompositionPreview.SetImplicitShowAnimation(AddToButton, animGroup2);
                ElementCompositionPreview.SetImplicitShowAnimation(ExitButton, animGroup1);
                ElementCompositionPreview.SetImplicitShowAnimation(list, animGroup3);
                ElementCompositionPreview.SetImplicitShowAnimation(AlbumYearTextBlock, animGroup1);
                ElementCompositionPreview.SetImplicitShowAnimation(AlbumNameTextBlock, animGroup1);
                ElementCompositionPreview.SetImplicitShowAnimation(TitleTextBlock, animGroup1);


                ElementCompositionPreview.SetImplicitHideAnimation(PlayButton, fadeOut);
                ElementCompositionPreview.SetImplicitHideAnimation(AddToButton, fadeOut);
                ElementCompositionPreview.SetImplicitHideAnimation(ExitButton, fadeOut);
                ElementCompositionPreview.SetImplicitHideAnimation(list, fadeOut);
                ElementCompositionPreview.SetImplicitHideAnimation(AlbumYearTextBlock, fadeOut);
                ElementCompositionPreview.SetImplicitHideAnimation(AlbumNameTextBlock, fadeOut);
                ElementCompositionPreview.SetImplicitHideAnimation(TitleTextBlock, fadeOut);
            }
            else
            {
                _playButtonVisual.Opacity = 1f;
                _addToButtonVisual.Opacity = 1f;
                _exitButtonVisual.Opacity = 1f;
                _musicListVisual.Opacity = 1f;
                _albumNameTextBlockVisual.Opacity = 1f;
                _albumYearTextBlockVisual.Opacity = 1f;
                _titleTextBlockVisual.Opacity = 1f;
            }
            base.OnNavigatedTo(e);

            animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ca1");
            if (animation != null)
            {
                animation.TryStart(ThumbnailImage);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            MainVM.DetailViewModel.Animate = true;
            if(e.NavigationMode == NavigationMode.Back)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ca2", ThumbnailImage);
            }
            
        }

        private void ExitButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void PlayButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainVM.SetMediaList();
            if (MainVM.FloatingVisible == Visibility.Collapsed)
            {
                MainVM.FloatingVisible = Visibility.Visible;
                var offSetAnimation = _compositor.CreateVector3KeyFrameAnimation();
                offSetAnimation.InsertKeyFrame(0f, new Vector3(_floatingVisual.Offset.X, _floatingVisual.Offset.Y + 100, 0));
                offSetAnimation.InsertKeyFrame(1f, new Vector3(_floatingVisual.Offset.X, _floatingVisual.Offset.Y, 0));
                offSetAnimation.Duration = TimeSpan.FromMilliseconds(500);
                _floatingVisual.StartAnimation("Translation", offSetAnimation);
            }
        }


        private void CurrentPlayingThumbnail_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TilePage), MainVM, new DrillInNavigationTransitionInfo());
        }
        

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
            if (panel != null)
            {
                panel.Fade(1f, 200, 0).Start();
            }

            var parentAnimation = new ScaleAnimation() { To = "1.1", Duration = TimeSpan.FromMilliseconds(200) };
            parentAnimation.StartAnimation(sender as UIElement);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
            if (panel != null)
            {
                panel.Fade(0f, 200, 0).Start();
            }

            var parentAnimation = new ScaleAnimation() { To = "1", Duration = TimeSpan.FromMilliseconds(200) };
            parentAnimation.StartAnimation(sender as UIElement);
        }

        private void List_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (args.ItemContainer != null)
            {
                return;
            }

            ListViewItem container = (ListViewItem)args.ItemContainer ?? new ListViewItem();
            container.IsDoubleTapEnabled = true;
            container.DoubleTapped += Container_DoubleTapped;

            args.ItemContainer = container;
        }

        private void Container_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            int doubleClickedIdx = MainVM.PlayerViewModel.MediaList.MediaList.IndexOf((e.OriginalSource as FrameworkElement).DataContext as LocalMusicModel);
            if (MainVM.PlayerViewModel.MediaList.CurrentItemIndex != doubleClickedIdx)
            {
                MainVM.PlayerViewModel.SetCurrentItem(doubleClickedIdx);
            }
        }

        private void MenuFlyoutItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var datacontext = (sender as FrameworkElement).DataContext as LocalPlaylistModel;
            MainVM.LibraryViewModel.AddSongsToPlaylist(datacontext.Playlist.Name, MainVM.DetailViewModel.Musics.ToList());
        }
    }
}
