using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// CustomMediaTransportControl : Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Reborn_Zune.Control
{

    // CustomMediaTransportControl class
    public sealed class CustomMediaTransportControl : MediaTransportControls
    {
        public event EventHandler<EventArgs> TilePageClicked;
        public event EventHandler<EventArgs> ListViewGridChecked;
        public event EventHandler<EventArgs> ListViewGridUnChecked;
        public event EventHandler<EventArgs> RepeatCheckBoxChecked;
        public event EventHandler<EventArgs> RepeatCheckBoxUnchecked;
        public event EventHandler<EventArgs> ShuffleCheckBoxChecked;
        public event EventHandler<EventArgs> ShuffleCheckBoxUnchecked;
        public event EventHandler<EventArgs> FullScreenButtonClicked;
        public event EventHandler<EventArgs> ExitButtonClicked;

        Image ThumbnailImage;
        TextBlock MusicTitle;
        Grid VolumeSliderGrid;
        StackPanel VolumeValue;
        Slider VolumeSlider;
        DockPanel MediaTransportControlsTimelineGrid;
        Button TilePageButton;
        CheckBox ListCheckBox;
        CheckBox RepeatCheckBox;
        CheckBox ShuffleCheckBox;
        Button FullScreenButton;
        Button ExitButton;


        // CustomMediaTransportControl
        public CustomMediaTransportControl()
        {
            DefaultStyleKey = typeof(CustomMediaTransportControl);

        }//CustomMediaTransportControl end

        // OnApplyTemplate
        protected override void OnApplyTemplate()
        {
            // This is where you would get your custom button and create an event handler for its click method.
            ThumbnailImage = GetTemplateChild("MusicThumbnail") as Image;
            MusicTitle = GetTemplateChild("MusicTitle") as TextBlock;
            VolumeSliderGrid = GetTemplateChild("VolumeSliderGrid") as Grid;
            VolumeValue = GetTemplateChild("VolumeValue") as StackPanel;
            VolumeSlider = GetTemplateChild("VolumeSlider") as Slider;
            MediaTransportControlsTimelineGrid = GetTemplateChild("MediaTransportControlsTimelineGrid") as DockPanel;
            TilePageButton = GetTemplateChild("TilePageButton") as Button;
            ListCheckBox = GetTemplateChild("ListCheckBox") as CheckBox;
            RepeatCheckBox = GetTemplateChild("RepeatCheckBox") as CheckBox;
            ShuffleCheckBox = GetTemplateChild("ShuffleCheckBox") as CheckBox;
            FullScreenButton = GetTemplateChild("FullScreenButton") as Button;
            ExitButton = GetTemplateChild("ExitButton") as Button;

            if (ListCheckBox != null)
            {
                ListCheckBox.Checked += ListCheckBox_Checked;
                ListCheckBox.Unchecked += ListCheckBox_Unchecked;
                ListCheckBox.IsChecked = true;
            }

            if (RepeatCheckBox != null)
            {
                RepeatCheckBox.Checked += RepeatCheckBox_Checked;
                RepeatCheckBox.Unchecked += RepeatCheckBox_Unchecked;
            }

            if (ShuffleCheckBox != null)
            {
                ShuffleCheckBox.Checked += ShuffleCheckBox_Checked;
                ShuffleCheckBox.Unchecked += ShuffleCheckBox_Unchecked;
            }

            if (FullScreenButton != null)
            {
                FullScreenButton.Click += FullScreenButton_Clicked;
            }

            if (ExitButton != null)
            {
                ExitButton.Click += ExitButton_Clicked;
            }

            VolumeSliderGrid.PointerEntered += Grid_PointerEntered;
            VolumeSliderGrid.PointerExited += Grid_PointerExited;
            MediaTransportControlsTimelineGrid.Loaded += DockPanel_Loaded;
            TilePageButton.Click += TilePageButton_Clicked;


            base.OnApplyTemplate();

        }//OnApplyTemplate end


        // ExitButton_Clicked
        private void ExitButton_Clicked(object sender, RoutedEventArgs e)
        {
            ExitButtonClicked?.Invoke(this, EventArgs.Empty);

        }//ExitButton_Clicked end


        // FullScreenButton_Clicked
        private void FullScreenButton_Clicked(object sender, RoutedEventArgs e)
        {
            FullScreenButtonClicked?.Invoke(this, EventArgs.Empty);

        }//FullScreenButton_Clicked end


        // ShuffleCheckBox_Unchecked
        private void ShuffleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ShuffleCheckBoxUnchecked?.Invoke(this, EventArgs.Empty);
        }//ShuffleCheckBox_Unchecked end



        private void ShuffleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ShuffleCheckBoxChecked?.Invoke(this, EventArgs.Empty);
        }

        private void RepeatCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RepeatCheckBoxUnchecked?.Invoke(this, EventArgs.Empty);
        }

        private void RepeatCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RepeatCheckBoxChecked?.Invoke(this, EventArgs.Empty);
        }

        private void ListCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ListViewGridUnChecked?.Invoke(this, EventArgs.Empty);
        }

        private void ListCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ListViewGridChecked?.Invoke(this, EventArgs.Empty);
        }

        private void TilePageButton_Clicked(object sender, RoutedEventArgs e)
        {
            TilePageClicked?.Invoke(this, EventArgs.Empty);
        }

        private async void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            await MediaTransportControlsTimelineGrid.Fade(1f, 400)
                            .Offset(-20f, 0f, 300)
                            .StartAsync();
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VolumeSlider.Scale(centerX: 5f,
                         centerY: 5f,
                         scaleX: 0.5f,
                         scaleY: 0.5f,
                         duration: 300,
                         delay: 3000)
                     .Fade(0, 300)
                     .StartAsync();

            VolumeValue.Scale(centerX: 0.8f,
                        centerY: 0.8f,
                        scaleX: 1f,
                        scaleY: 1f,
                        duration: 300, delay: 3)
                        .Fade(1, 300)
                        .StartAsync();
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VolumeValue.Scale(centerX: 0.8f,
                            centerY: 0.8f,
                            scaleX: 0.5f,
                            scaleY: 0.5f,
                            duration: 300)
                        .Fade(0, 300)
                        .StartAsync();

            VolumeSlider.Scale(centerX: 5f,
                        centerY: 5f,
                        scaleX: 1f,
                        scaleY: 1f,
                        duration: 300, delay: 3)
                        .Fade(1, 300)
                        .StartAsync();
        }

        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(CustomMediaTransportControl), new PropertyMetadata(String.Empty, onTitleChanged));

        private static void onTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CustomMediaTransportControl)d).TitlePropertyChanged(e.NewValue as String);
        }

        private void TitlePropertyChanged(string v)
        {
            MusicTitle.Text = v;
        }

        public BitmapSource Thumbnail
        {
            get { return (BitmapSource)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Thumbnail", typeof(BitmapSource), typeof(CustomMediaTransportControl), new PropertyMetadata(new BitmapImage(), onThumbnailChanged));

        private static void onThumbnailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            ((CustomMediaTransportControl)d).ThumbnailPropertyChanged(e.NewValue as BitmapSource);
        }

        private void ThumbnailPropertyChanged(BitmapSource bitmapSource)
        {
            ThumbnailImage.Source = bitmapSource;
        }

        public bool Repeat
        {
            get { return (bool)GetValue(RepeatProperty); }
            set { SetValue(RepeatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RepeatCheckBoxCheck.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RepeatProperty =
            DependencyProperty.Register("Repeat", typeof(bool), typeof(CustomMediaTransportControl), new PropertyMetadata(false, onRepeatCheckBoxChanged));

        private static void onRepeatCheckBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CustomMediaTransportControl)d).RepeatCheckBoxPropertyChanged(Convert.ToBoolean(e.NewValue));
        }

        private void RepeatCheckBoxPropertyChanged(bool v)
        {
            RepeatCheckBox.IsChecked = v;
        }

        public bool Shuffle
        {
            get { return (bool)GetValue(ShuffleProperty); }
            set { SetValue(ShuffleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Shuffle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShuffleProperty =
            DependencyProperty.Register("Shuffle", typeof(bool), typeof(CustomMediaTransportControl), new PropertyMetadata(false, onShuffleChanged));

        private static void onShuffleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CustomMediaTransportControl)d).ShuffleCheckBoxPropertyChanged(Convert.ToBoolean(e.NewValue));
        }

        private void ShuffleCheckBoxPropertyChanged(bool v)
        {
            ShuffleCheckBox.IsChecked = v;
        }
    }
}
