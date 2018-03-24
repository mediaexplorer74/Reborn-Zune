﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Windows.Graphics.Display;
using Windows.Graphics.Effects;
using Windows.UI.Composition;
using Windows.UI.Composition.Effects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private ObservableCollection<UIElement> _items;
        //private const float lightDepth = 300f;
        //private const int animationDelay = 600;
        //private const int animationDuration = 70;
        //private CompositionEffectFactory _effectFactory;
        //private Random _random = new Random();
        private Compositor _compositor;
        private PointLight _pointLight;
        //private PointLight _secondPointLight;
        private Visual _root;

        public MainPage()
        {
            this.InitializeComponent();
            ObservableCollection<String> _Artists = new ObservableCollection<string>
            {
                "GALA",
                "Rich Brian",
                "タニザワトモフミ",
                "卫兰",
                "宋冬野",
                "张学友",
                "朴树",
                "林俊杰",
                "程璧",
                "逃跑计划",
                "陈奕迅",
                "陈粒",
                "陈长林",
                "马頔",
                "骆集益",
                "黑豹乐队"
            };
            ObservableCollection<BitmapImage> _Alumbs = new ObservableCollection<BitmapImage>
            {
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
                new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            };
            ArtistList.ItemsSource = _Artists;
            AlbumList.ItemsSource = _Alumbs;
           
        }

        //var displayInformation = DisplayInformation.GetForCurrentView();
        //var screenSize = new Size((int)displayInformation.ScreenWidthInRawPixels,
        //                          (int)displayInformation.ScreenHeightInRawPixels);
        //Vector2 sizeLightBounds = new Vector2((float)screenSize.Width, (float)screenSize.Height);
        //_compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        //_root = ElementCompositionPreview.GetElementVisual(rootPanel);
        //_pointLight = _compositor.CreatePointLight();
        //_secondPointLight = _compositor.CreatePointLight();
        //_pointLight.Offset = new Vector3(-2500f, -2500f, 300f);
        //_secondPointLight.Offset = new Vector3(-2500f, -2500f, 300f);
        //_pointLight.Intensity = 1.3f;
        //_secondPointLight.Intensity = 1.3f;
        //IGraphicsEffect graphicsEffect = new CompositeEffect()
        //{
        //    Mode = CanvasComposite.DestinationIn,
        //    Sources =
        //                {
        //                    new CompositeEffect()
        //                    {
        //                        Mode = CanvasComposite.Add,
        //                        Sources =
        //                        {
        //                            new CompositionEffectSourceParameter("ImageSource"),
        //                            new SceneLightingEffect()
        //                            {
        //                                AmbientAmount = 0,
        //                                DiffuseAmount = .5f,
        //                                SpecularAmount = 0,
        //                                NormalMapSource = new CompositionEffectSourceParameter("NormalMap"),
        //                            }
        //                        }
        //                    },
        //                    new CompositionEffectSourceParameter("NormalMap"),
        //                }
        //};

        //_effectFactory = _compositor.CreateEffectFactory(graphicsEffect);
        ////_pointLight.CoordinateSpace = _root;
        ////_pointLight.Targets.Add(_root);
        ////_secondPointLight.CoordinateSpace = _root;
        ////_secondPointLight.Targets.Add(_root);

        //panel.ItemsSource = _items;

        //#region First light animation
        //Vector3KeyFrameAnimation lightPositionAnimation;
        //lightPositionAnimation = _compositor.CreateVector3KeyFrameAnimation();
        //lightPositionAnimation.InsertKeyFrame(.0f, new Vector3(200f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //lightPositionAnimation.InsertKeyFrame(.16f, new Vector3(825f, 70f, lightDepth), _compositor.CreateLinearEasingFunction());
        //lightPositionAnimation.InsertKeyFrame(.32f, new Vector3(2100f, 40f, lightDepth), _compositor.CreateLinearEasingFunction());
        //lightPositionAnimation.InsertKeyFrame(.48f, new Vector3(825f, 70f, lightDepth), _compositor.CreateLinearEasingFunction());
        //lightPositionAnimation.InsertKeyFrame(.64f, new Vector3(200f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //lightPositionAnimation.InsertKeyFrame(.8f, new Vector3(200f, 100f, lightDepth), _compositor.CreateLinearEasingFunction());
        //lightPositionAnimation.InsertKeyFrame(1f, new Vector3(200f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //lightPositionAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
        //lightPositionAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
        //lightPositionAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

        //ColorKeyFrameAnimation lightColorAnimation;
        //lightColorAnimation = _compositor.CreateColorKeyFrameAnimation();
        //lightColorAnimation.InsertKeyFrame(0f, "#865c2a".ToColor());
        //lightColorAnimation.InsertKeyFrame(.2f, "#955a21".ToColor());
        //lightColorAnimation.InsertKeyFrame(.4f, "#6c891b".ToColor());
        //lightColorAnimation.InsertKeyFrame(.6f, "#62962c".ToColor());
        //lightColorAnimation.InsertKeyFrame(.8f, "#6c401b".ToColor());
        //lightColorAnimation.InsertKeyFrame(1f, "#865c2a".ToColor());
        //lightColorAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
        //lightColorAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
        //lightColorAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

        //_pointLight.StartAnimation("Offset", lightPositionAnimation);
        //_pointLight.StartAnimation("Color", lightColorAnimation);
        //#endregion

        //#region Second light animation
        //Vector3KeyFrameAnimation secondLightPositionAnimation;
        //secondLightPositionAnimation = _compositor.CreateVector3KeyFrameAnimation();
        //secondLightPositionAnimation.InsertKeyFrame(.0f, new Vector3(2300f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //secondLightPositionAnimation.InsertKeyFrame(.16f, new Vector3(1800f, 900f, lightDepth), _compositor.CreateLinearEasingFunction());
        //secondLightPositionAnimation.InsertKeyFrame(.32f, new Vector3(400f, 1200f, lightDepth), _compositor.CreateLinearEasingFunction());
        //secondLightPositionAnimation.InsertKeyFrame(.48f, new Vector3(1800f, 900f, lightDepth), _compositor.CreateLinearEasingFunction());
        //secondLightPositionAnimation.InsertKeyFrame(.64f, new Vector3(2300f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //secondLightPositionAnimation.InsertKeyFrame(.8f, new Vector3(2300f, 100f, lightDepth), _compositor.CreateLinearEasingFunction());
        //secondLightPositionAnimation.InsertKeyFrame(1f, new Vector3(2300f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
        //secondLightPositionAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
        //secondLightPositionAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
        //secondLightPositionAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

        //ColorKeyFrameAnimation secondLightColorAnimation;
        //secondLightColorAnimation = _compositor.CreateColorKeyFrameAnimation();
        //secondLightColorAnimation.InsertKeyFrame(0f, "#81268b".ToColor());
        //secondLightColorAnimation.InsertKeyFrame(.2f, "#68446c".ToColor());
        //secondLightColorAnimation.InsertKeyFrame(.4f, "#368843".ToColor());
        //secondLightColorAnimation.InsertKeyFrame(.6f, "#315b8a".ToColor());
        //secondLightColorAnimation.InsertKeyFrame(.8f, "#7d2185".ToColor());
        //secondLightColorAnimation.InsertKeyFrame(1f, "#81268b".ToColor());
        //secondLightColorAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
        //secondLightColorAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
        //secondLightColorAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

        //_secondPointLight.StartAnimation("Offset", secondLightPositionAnimation);
        //_secondPointLight.StartAnimation("Color", secondLightColorAnimation);
        //#endregion
    }
}
