using Reborn_Zune.Utilities;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Reborn_Zune.Control
{
    public sealed partial class Tile : UserControl
    {
        private BitmapImage _bitmapImage;
        private Compositor _compositor;

        //public int Index
        public uint Index
        {
            get { return (uint)GetValue(IndexProperty); }//{ return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(Tile), new PropertyMetadata(0));

        public BitmapImage Thumbnail
        {
            get { return (BitmapImage)GetValue(ThumbnailProperty); }
            set { SetValue(ThumbnailProperty, value);
                _bitmapImage = value;
            }
        }

        // Using a DependencyProperty as the backing store for Thumbnail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbnailProperty =
            DependencyProperty.Register("Thumbnail", typeof(BitmapImage), typeof(Tile), new PropertyMetadata(null));


        public Tile()
        {
            this.InitializeComponent();
        }

        public async void UpdateThumbnail(Compositor _compositor, BitmapImage imgSource)
        {
            this._compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var tileVisual = ImgGrid.GetVisual();

            tileVisual.CenterPoint = new Vector3((float)Img.ActualWidth / 2, (float)Img.ActualHeight / 2, 0);
            tileVisual.RotationAxis = new Vector3(0f, 1f, 0f);


            //Flip Fade out animation group
            var fadeoutAnim = _compositor.CreateAnimationGroup();
            ScalarKeyFrameAnimation rotationAnimation = _compositor.CreateScalarKeyFrameAnimation();
            LinearEasingFunction linear = _compositor.CreateLinearEasingFunction();
            rotationAnimation.InsertKeyFrame(0f, 0f, linear);
            rotationAnimation.InsertKeyFrame(1f, 90f, linear);
            rotationAnimation.Duration = TimeSpan.FromMilliseconds(400);
            rotationAnimation.Target = "RotationAngleInDegrees";
            ScalarKeyFrameAnimation opacityAnimation = _compositor.CreateScalarKeyFrameAnimation();
            opacityAnimation.InsertKeyFrame(1f, 0);
            opacityAnimation.Duration = TimeSpan.FromMilliseconds(200);
            opacityAnimation.DelayTime = TimeSpan.FromMilliseconds(250);
            opacityAnimation.Target = "Opacity";
            fadeoutAnim.Add(rotationAnimation);
            fadeoutAnim.Add(opacityAnimation);

            tileVisual.StartAnimationGroup(fadeoutAnim);

            await Task.Delay(500);
            Thumbnail = imgSource;


            tileVisual.StopAnimationGroup(fadeoutAnim);
            var fadeInAnim = _compositor.CreateAnimationGroup();

            ScalarKeyFrameAnimation rotationAnim = _compositor.CreateScalarKeyFrameAnimation();
            rotationAnim.InsertKeyFrame(0f, 270, linear);
            rotationAnim.InsertKeyFrame(1f, 360, linear);
            rotationAnim.Duration = TimeSpan.FromMilliseconds(550);
            rotationAnim.DelayTime = TimeSpan.FromMilliseconds(200);
            rotationAnim.Target = "RotationAngleInDegrees";
            ScalarKeyFrameAnimation opacityAnim2 = _compositor.CreateScalarKeyFrameAnimation();
            opacityAnim2.InsertKeyFrame(0f, 0f);
            opacityAnim2.InsertKeyFrame(1f, 1f);
            opacityAnim2.Duration = TimeSpan.FromMilliseconds(600);
            opacityAnim2.DelayTime = TimeSpan.FromMilliseconds(250);
            opacityAnim2.Target = "Opacity";
            fadeoutAnim.Add(rotationAnim);
            fadeoutAnim.Add(opacityAnim2);
            tileVisual.StartAnimationGroup(fadeoutAnim);

        }
    }
}
