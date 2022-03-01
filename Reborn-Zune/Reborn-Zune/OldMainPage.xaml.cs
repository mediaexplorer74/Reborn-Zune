using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Reborn_Zune.Model;
using Reborn_Zune.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Windows.Graphics.Display;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Effects;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

#pragma warning disable 0169
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OldMainPage : Page
    {
        private MainViewModel viewModel;

        public OldMainPage()
        {
            this.InitializeComponent();

            viewModel = new MainViewModel(Dispatcher);
            //MainMediaElement.SetMediaPlayer(viewModel._player);
            //TileMediaElement.SetMediaPlayer(viewModel._player);
            TitleBarSetting();

           
            
        }
        private static void TitleBarSetting()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.InactiveBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = "#00000000".ToColor();
            titleBar.ButtonForegroundColor = Colors.DarkGray;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
            titleBar.ButtonHoverBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
            titleBar.ButtonPressedBackgroundColor = "#f5f5f5".ToColor();
            titleBar.ButtonPressedForegroundColor = Colors.Black;
        }

        //private void TileRoot_Loaded(object sender, RoutedEventArgs e)
        //{
        //    TileRoot.Visibility = Visibility.Collapsed;
        //    var displayInformation = DisplayInformation.GetForCurrentView();
        //    var screenSize = new Size((int)displayInformation.ScreenWidthInRawPixels,
        //                              (int)displayInformation.ScreenHeightInRawPixels);
        //    Vector2 sizeLightBounds = new Vector2((float)screenSize.Width, (float)screenSize.Height);
        //    _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        //    _root = ElementCompositionPreview.GetElementVisual(tilesPanel);
        //    _pointLight = _compositor.CreatePointLight();
        //    _secondPointLight = _compositor.CreatePointLight();
        //    _pointLight.Offset = new Vector3(-2500f, -2500f, 300f);
        //    _secondPointLight.Offset = new Vector3(-2500f, -2500f, 300f);
        //    _pointLight.Intensity = 1.5f;
        //    _secondPointLight.Intensity = 1.5f;
        //    _ambientLight = _compositor.CreateAmbientLight();
        //    _ambientLight.Intensity = 0.25f;
        //    _ambientLight.Color = "#d3d3d3".ToColor();
        //    IGraphicsEffect graphicsEffect = new CompositeEffect()
        //    {
        //        Mode = CanvasComposite.DestinationIn,
        //        Sources =
        //                    {
        //                        new CompositeEffect()
        //                        {
        //                            Mode = CanvasComposite.Add,
        //                            Sources =
        //                            {
        //                                new CompositionEffectSourceParameter("ImageSource"),
        //                                new SceneLightingEffect()
        //                                {
        //                                    AmbientAmount = 0,
        //                                    DiffuseAmount = 30f,
        //                                    SpecularAmount = 0,
        //                                    NormalMapSource = new CompositionEffectSourceParameter("NormalMap"),
        //                                }
        //                            }
        //                        },
        //                        new CompositionEffectSourceParameter("NormalMap"),
        //                    }
        //    };

        //    _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);
        //    _pointLight.CoordinateSpace = _root;
        //    _pointLight.Targets.Add(_root);
        //    _secondPointLight.CoordinateSpace = _root;
        //    _secondPointLight.Targets.Add(_root);
        //    _ambientLight.Targets.Add(_root);

        //    //panel.ItemsSource = _items;

        //    #region First light animation
        //    Vector3KeyFrameAnimation lightPositionAnimation;
        //    lightPositionAnimation = _compositor.CreateVector3KeyFrameAnimation();
        //    lightPositionAnimation.InsertKeyFrame(.0f, new Vector3(200f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    lightPositionAnimation.InsertKeyFrame(.16f, new Vector3(825f, 70f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    lightPositionAnimation.InsertKeyFrame(.32f, new Vector3(2100f, 40f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    lightPositionAnimation.InsertKeyFrame(.48f, new Vector3(825f, 70f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    lightPositionAnimation.InsertKeyFrame(.64f, new Vector3(200f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    lightPositionAnimation.InsertKeyFrame(.8f, new Vector3(200f, 100f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    lightPositionAnimation.InsertKeyFrame(1f, new Vector3(200f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    lightPositionAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
        //    lightPositionAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
        //    lightPositionAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

        //    ColorKeyFrameAnimation lightColorAnimation;
        //    lightColorAnimation = _compositor.CreateColorKeyFrameAnimation();
        //    lightColorAnimation.InsertKeyFrame(0f, "#865c2a".ToColor());
        //    lightColorAnimation.InsertKeyFrame(.2f, "#955a21".ToColor());
        //    lightColorAnimation.InsertKeyFrame(.4f, "#6c891b".ToColor());
        //    lightColorAnimation.InsertKeyFrame(.6f, "#62962c".ToColor());
        //    lightColorAnimation.InsertKeyFrame(.8f, "#6c401b".ToColor());
        //    lightColorAnimation.InsertKeyFrame(1f, "#865c2a".ToColor());
        //    lightColorAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
        //    lightColorAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
        //    lightColorAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

        //    _pointLight.StartAnimation("Offset", lightPositionAnimation);
        //    _pointLight.StartAnimation("Color", lightColorAnimation);
        //    #endregion

        //    #region Second light animation
        //    Vector3KeyFrameAnimation secondLightPositionAnimation;
        //    secondLightPositionAnimation = _compositor.CreateVector3KeyFrameAnimation();
        //    secondLightPositionAnimation.InsertKeyFrame(.0f, new Vector3(2300f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    secondLightPositionAnimation.InsertKeyFrame(.16f, new Vector3(1800f, 900f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    secondLightPositionAnimation.InsertKeyFrame(.32f, new Vector3(400f, 1200f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    secondLightPositionAnimation.InsertKeyFrame(.48f, new Vector3(1800f, 900f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    secondLightPositionAnimation.InsertKeyFrame(.64f, new Vector3(2300f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    secondLightPositionAnimation.InsertKeyFrame(.8f, new Vector3(2300f, 100f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    secondLightPositionAnimation.InsertKeyFrame(1f, new Vector3(2300f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //    secondLightPositionAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
        //    secondLightPositionAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
        //    secondLightPositionAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

        //    ColorKeyFrameAnimation secondLightColorAnimation;
        //    secondLightColorAnimation = _compositor.CreateColorKeyFrameAnimation();
        //    secondLightColorAnimation.InsertKeyFrame(0f, "#81268b".ToColor());
        //    secondLightColorAnimation.InsertKeyFrame(.2f, "#68446c".ToColor());
        //    secondLightColorAnimation.InsertKeyFrame(.4f, "#368843".ToColor());
        //    secondLightColorAnimation.InsertKeyFrame(.6f, "#315b8a".ToColor());
        //    secondLightColorAnimation.InsertKeyFrame(.8f, "#7d2185".ToColor());
        //    secondLightColorAnimation.InsertKeyFrame(1f, "#81268b".ToColor());
        //    secondLightColorAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
        //    secondLightColorAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
        //    secondLightColorAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

        //    _secondPointLight.StartAnimation("Offset", secondLightPositionAnimation);
        //    _secondPointLight.StartAnimation("Color", secondLightColorAnimation);
        //    #endregion
        //}
    }
}
