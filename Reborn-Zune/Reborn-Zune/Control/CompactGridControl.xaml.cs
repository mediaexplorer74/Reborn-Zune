using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Reborn_Zune.Control : User Control 

// Reborn_Zune.Control namespace
namespace Reborn_Zune.Control
{

    // CompactGridControl class
    public sealed partial class CompactGridControl : UserControl
    {

        // ThumbanilSource property
        public ObservableCollection<BitmapImage> ThumbanilSource
        {
            get { return (ObservableCollection<BitmapImage>)GetValue(ThumbanilSourceProperty); }
            set { SetValue(ThumbanilSourceProperty, value); }
        }

        // ThumbanilSourceProperty property
        // Using a DependencyProperty as the backing store for ThumbanilSource.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbanilSourceProperty =
            DependencyProperty.Register("ThumbanilSource", typeof(ObservableCollection<BitmapImage>), 
                typeof(CompactGridControl), 
                new PropertyMetadata(new ObservableCollection<BitmapImage>(), onThumbnailSourceChanged));



        // onThumbnailSourceChanged
        private static void onThumbnailSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CompactGridControl)d).onThumbnailSourceChanged
                (e.NewValue as ObservableCollection<BitmapImage>);
        }//onThumbnailSourceChanged end

        private void onThumbnailSourceChanged(ObservableCollection<BitmapImage> source)
        {
            gridView.ItemsSource = source;
        }


        // CompactGridControl
        public CompactGridControl()
        {
            this.InitializeComponent();
            gridView.ItemsSource = new List<string>
            {
                "a",
                "a",
                "a",
                "a",
                "a",
                "a",
                "a",
                "a",
                "a",
            };

        }//CompactGridControl end

    }//CompactGridControl class end

}//Reborn_Zune.Control namespace end
