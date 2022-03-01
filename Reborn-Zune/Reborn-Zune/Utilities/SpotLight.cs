using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Reborn_Zune.Utilities
{
    public sealed class SpotLight : XamlLight
    {
        // Register an attached property that lets you set a UIElement
        // or Brush as a target for this light type in markup.
        public static readonly DependencyProperty IsTargetProperty =
            DependencyProperty.RegisterAttached(
            "IsTarget",
            typeof(bool),
            typeof(SpotLight),
            new PropertyMetadata(null, OnIsTargetChanged)
        );

        public static void SetIsTarget(DependencyObject target, bool value)
        {
            target.SetValue(IsTargetProperty, value);
        }

        public static Boolean GetIsTarget(DependencyObject target)
        {
            return (bool)target.GetValue(IsTargetProperty);
        }

        // Handle attached property changed to automatically target and untarget UIElements and Brushes.
        private static void OnIsTargetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var isAdding = (bool)e.NewValue;

            if (isAdding)
            {
                if (obj is UIElement)
                {
                    XamlLight.AddTargetElement(GetIdStatic(), obj as UIElement);
                }
                else if (obj is Brush)
                {
                    XamlLight.AddTargetBrush(GetIdStatic(), obj as Brush);
                }
            }
            else
            {
                if (obj is UIElement)
                {
                    XamlLight.RemoveTargetElement(GetIdStatic(), obj as UIElement);
                }
                else if (obj is Brush)
                {
                    XamlLight.RemoveTargetBrush(GetIdStatic(), obj as Brush);
                }
            }
        }

        protected override void OnConnected(UIElement newElement)
        {
            if (CompositionLight == null)
            {
                // OnConnected is called when the first target UIElement is shown on the screen.
                // This lets you delay creation of the composition object until it's actually needed.
                var spotLight = Window.Current.Compositor.CreateSpotLight();
                spotLight.InnerConeColor = Colors.White;
                spotLight.InnerConeAngleInDegrees = 60;
                spotLight.Offset = new Vector3(46f, 46f, 45f);
                //spotLight.CoordinateSpace.Offset = new Vector3(newElement.CenterPoint.X / 2, newElement.CenterPoint.Y / 2, 0);
                CompositionLight = spotLight;
            }
        }

        protected override void OnDisconnected(UIElement oldElement)
        {
            // OnDisconnected is called when there are no more target UIElements on the screen.
            // The CompositionLight should be disposed when no longer required.
            // For SDK 15063, see Remarks in the XamlLight class documentation.
            if (CompositionLight != null)
            {
                CompositionLight.Dispose();
                CompositionLight = null;
            }
        }

        protected override string GetId()
        {
            return GetIdStatic();
        }

        private static string GetIdStatic()
        {
            // This specifies the unique name of the light.
            // In most cases you should use the type's FullName.
            return typeof(SpotLight).FullName;
        }
    }
}
