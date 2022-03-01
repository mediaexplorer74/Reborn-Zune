using Reborn_Zune.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Reborn_Zune.Control
{
    public class VariableSizedWrapPanel : Panel
    {
        #region Constants

        private const double DragScaleDefault = 1.2;
        private const double NormalOpacity = 1.0;
        private const double DragOpacityDefault = 0.7;
        private const double OpacityMin = 0.1d;
        private const double DefaultItemWidth = 10.0;
        private const double DefaultItemHeight = 10.0;
        private const int ZIndexIntermediate = 1;
        private const int ZIndexDrag = 10;
        private static readonly TimeSpan DefaultInitialAnimationDuration = TimeSpan.FromMilliseconds(300);
        private static readonly TimeSpan DefaultFluidAnimationDuration = TimeSpan.FromMilliseconds(570);
        private static readonly TimeSpan DefaultOpacityAnimationDuration = TimeSpan.FromMilliseconds(300);
        private static readonly TimeSpan DefaultScaleAnimationDuration = TimeSpan.FromMilliseconds(400);

        #endregion

        #region Structures

        /// <summary>
        /// Structure to store the bit-normalized dimensions
        /// of the FluidWrapPanel's children.
        /// </summary>
        private struct BitSize
        {
            internal int Width;
            internal int Height;
        }

        /// <summary>
        /// Structure to store the location and the bit-normalized
        /// dimensions of the FluidWrapPanel's children.
        /// </summary>
        private struct BitInfo
        {
            internal long Row;
            internal long Col;
            internal int Width;
            internal int Height;

            /// <summary>
            /// Checks if the bit-normalized width and height
            /// are equal to 1.
            /// </summary>
            /// <returns>True if yes otherwise False</returns>
            internal bool IsUnitSize()
            {
                return (Width == 1) && (Height == 1);
            }

            /// <summary>
            /// Checks if the given location is within the 
            /// bit-normalized bounds
            /// </summary>
            /// <param name="row">Row</param>
            /// <param name="col">Column</param>
            /// <returns>True if yes otherwise False</returns>
            internal bool Contains(long row, long col)
            {
                return (row >= Row) && (row < Row + Height) &&
                       (col >= Col) && (col < Col + Width);
            }
        }

        #endregion

        #region Fields
        
        private bool _isOptimized;
        private Size _panelSize;
        private int _cellsPerLine;
        private int _maxCellRows;
        private int _maxCellCols;
        private Dictionary<UIElement, BitInfo> _fluidBits;
        private LayerVisual _backgroundLayerVisual;

        private Compositor _compositor;
        private Dictionary<UIElement, Visual> _fluidVisuals;
        private List<UIElement> _uninitializedFluidItems;
        private ImplicitAnimationCollection _implicitAnimations1st;
        private ImplicitAnimationCollection _implicitAnimations2nd;
        private ImplicitAnimationCollection _implicitAnimations3rd;
        private ImplicitAnimationCollection _implicitAnimations4th;
        private ImplicitAnimationCollection _implicitAnimations5th;
        private ImplicitAnimationCollection _implicitAnimations6th;
        private ImplicitAnimationCollection _implicitAnimations7th;
        private ImplicitAnimationCollection _implicitAnimations8th;
        private ImplicitAnimationCollection _implicitAnimations9th;
        private ImplicitAnimationCollection _implicitAnimations10th;
        private ImplicitAnimationCollection _implicitAnimations11th;
        private ImplicitAnimationCollection _implicitAnimations12th;
        private ImplicitAnimationCollection _implicitAnimations13th;
        private ImplicitAnimationCollection _implicitAnimations14th;


        #endregion

        #region Dependency Properties

        #region FluidItems

        /// <summary>
        /// FluidItems Read-Only Dependency Property
        /// </summary>
        public static readonly DependencyProperty FluidItemsProperty =
            DependencyProperty.Register("FluidItems", typeof(ObservableCollection<UIElement>), typeof(VariableSizedWrapPanel),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the FluidItems property. This dependency property 
        /// indicates the observable list of FluidWrapPanel's children.
        /// </summary>
        public ObservableCollection<UIElement> FluidItems
        {
            get => (ObservableCollection<UIElement>)GetValue(FluidItemsProperty);
            private set => SetValue(FluidItemsProperty, value);
        }

        #endregion

        #region IsComposing

        /// <summary>
        /// IsComposing Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsComposingProperty =
            DependencyProperty.Register("IsComposing", typeof(bool), typeof(VariableSizedWrapPanel), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the IsComposing property. This dependency property 
        /// indicates if the FluidWrapPanel is in Composing mode.
        /// </summary>
        public bool IsComposing
        {
            get => (bool)GetValue(IsComposingProperty);
            set => SetValue(IsComposingProperty, value);
        }

        #endregion

        #region ItemHeight

        /// <summary>
        /// ItemHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(VariableSizedWrapPanel),
                new PropertyMetadata(DefaultItemHeight, OnItemHeightChanged));

        private static void OnItemHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as VariableSizedWrapPanel;
            panel?.InvalidateMeasure();
        }

        /// <summary>
        /// Gets or sets the ItemHeight property. This dependency property 
        /// indicates the height of each item.
        /// </summary>
        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, CoerceItemHeight(value));
        }

        /// <summary>
        /// Coerces the ItemHeight to a valid positive value
        /// </summary>
        /// <param name="height">Height</param>
        /// <returns>Coerced Value</returns>
        private static double CoerceItemHeight(double height)
        {
            return height < 0.0 ? 0.0 : height;
        }

        #endregion

        #region ItemWidth

        /// <summary>
        /// ItemWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(VariableSizedWrapPanel),
                new PropertyMetadata(DefaultItemWidth, OnItemWidthChanged));

        private static void OnItemWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as VariableSizedWrapPanel;
            panel?.InvalidateMeasure();
        }

        /// <summary>
        /// Gets or sets the ItemWidth property. This dependency property 
        /// indicates the width of each item.
        /// </summary>
        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, CoerceItemWidth(value));
        }

        /// <summary>
        /// Coerces the ItemWidth to a valid positive value
        /// </summary>
        /// <param name="width">width</param>
        /// <returns>Coerced Value</returns>
        private static double CoerceItemWidth(double width)
        {
            return width < 0.0 ? 0.0 : width;
        }

        #endregion

        #region ItemsSource

        /// <summary>
        /// ItemsSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(VariableSizedWrapPanel),
                new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceChanged)));

        /// <summary>
        /// Gets or sets the ItemsSource property. This dependency property 
        /// indicates the bindable collection.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => (ObservableCollection<UIElement>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Handles changes to the ItemsSource property.
        /// </summary>
        /// <param name="d">FluidWrapPanel</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (VariableSizedWrapPanel)d;
            var newItemsSource = panel.ItemsSource;
 
            panel.OnItemsSourceChanged(newItemsSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ItemsSource property.
        /// </summary>
        /// <param name="newItemsSource">New Value</param>
        private void OnItemsSourceChanged(IEnumerable newItemsSource)
        {
            // Clear the previous items in the Children property
            ClearItemsSource();

            // Add the new children
            foreach (UIElement child in newItemsSource)
            {
                Children.Add(child);
            }

            // Refresh Layout
            //InvalidateMeasure();
        }

        #endregion

        #region ItemTemplate

        /// <summary>
        /// ItemTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(VariableSizedWrapPanel),
                new PropertyMetadata(null, OnItemTemplateChanged));

        /// <summary>
        /// Gets or sets the ItemTemplate property. This dependency property 
        /// indicates the data template that is used to display the content of 
        /// the FluidWrapPanel's children (if they derive from ContentControl).
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Handles changes to the ItemTemplate property.
        /// </summary>
        /// <param name="d">FluidWrapPanel</param>
		/// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (VariableSizedWrapPanel)d;
            panel.OnItemTemplateChanged();
        }

        /// <summary>
        /// Provides the class instance an opportunity to handle changes to the ItemTemplate property.
        /// </summary>
		private void OnItemTemplateChanged()
        {
            InvalidateMeasure();
        }

        #endregion
      
        #region OptimizeChildPlacement

        /// <summary>
        /// OptimizeChildPlacement Dependency Property
        /// </summary>
        public static readonly DependencyProperty OptimizeChildPlacementProperty =
            DependencyProperty.Register("OptimizeChildPlacement", typeof(bool), typeof(VariableSizedWrapPanel),
                new PropertyMetadata(true, OnOptimizeChildPlacementChanged));

        private static void OnOptimizeChildPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as VariableSizedWrapPanel;
            panel?.InvalidateMeasure();
        }

        /// <summary>
        /// Gets or sets the OptimizeChildPlacement property. This dependency property 
        /// indicates whether the placement of the children is optimized. 
        /// If set to true, the child is placed at the first available position from 
        /// the beginning of the FluidWrapPanel. 
        /// If set to false, each child occupies the same (or greater) row and/or column
        /// than the previous child.
        /// </summary>
        public bool OptimizeChildPlacement
        {
            get => (bool)GetValue(OptimizeChildPlacementProperty);
            set => SetValue(OptimizeChildPlacementProperty, value);
        }

        #endregion

        #region Orientation

        /// <summary>
        /// Orientation Dependency Property
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(VariableSizedWrapPanel),
                new PropertyMetadata(Orientation.Horizontal, OnOrientationChanged));

        /// <summary>
        /// Gets or sets the Orientation property. This dependency property 
        /// indicates the orientation of arrangement of items in the panel.
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Handles changes to the Orientation property.
        /// </summary>
        /// <param name="d">FluidWrapPanel</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (VariableSizedWrapPanel)d;
            panel.OnOrientationChanged();
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Orientation property.
        /// </summary>
        private void OnOrientationChanged()
        {
            // Refresh the layout
            InvalidateMeasure();
        }

        #endregion

        #endregion

        #region Construction / Initialization

        /// <summary>
        /// Default Ctor
        /// </summary>
        public VariableSizedWrapPanel()
        {
            FluidItems = new ObservableCollection<UIElement>();
            _fluidBits = new Dictionary<UIElement, BitInfo>();

            _fluidVisuals = new Dictionary<UIElement, Visual>();
            _uninitializedFluidItems = new List<UIElement>();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Handles the Measure pass during Layout
        /// </summary>
        /// <param name="availableSize">Available size</param>
        /// <returns>Total Size required to accommodate all the Children</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            // Compositor will be null the very first time
            if (_compositor == null)
            {
                InitializeComposition();
            }

            // Clear any previously uninitialized items
            _uninitializedFluidItems.Clear();

            // Clear visuals of children which are removed
            var removables = new List<UIElement>(_fluidVisuals.Keys.Where(c => !Children.Contains(c)));

            foreach (var child in removables)
            {
                _fluidVisuals.Remove(child);
                FluidItems.Remove(child);
            }

            removables.Clear();

            var availableItemSize = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
            Random rnd = new Random();
            ImplicitAnimationCollection[] animationGroupList = {_implicitAnimations1st,
                                                                _implicitAnimations2nd,
                                                                _implicitAnimations3rd,
                                                                _implicitAnimations4th,
                                                                _implicitAnimations5th,
                                                                _implicitAnimations6th,
                                                                _implicitAnimations7th,
                                                                _implicitAnimations8th,
                                                                _implicitAnimations9th,
                                                                _implicitAnimations10th,
                                                                _implicitAnimations11th,
                                                                _implicitAnimations12th,
                                                                _implicitAnimations13th,
                                                                _implicitAnimations14th};
            // Iterate through all the UIElements in the Children collection
            foreach (var child in Children.Where(c => c != null))
            {
                if ((ItemTemplate != null) && (child is ContentControl contentChild))
                {
                    contentChild.ContentTemplate = ItemTemplate;
                }

                // Ask the child how much size it needs
                child.Measure(availableItemSize);
                // Check if the child is already added to the fluidElements collection
                if (FluidItems.Contains(child))
                    continue;

                // If the FluidItems collection does not contain this child it means it is newly
                // added to the FluidWrapPanel and is not initialized yet
                // Add the child to the fluidElements collection
                FluidItems.Add(child);
                // Add the child to the UninitializedFluidItems
                _uninitializedFluidItems.Add(child);


                // Get the visual of the child
                var visual = child.GetVisual();
                visual.Opacity = 0f;
                int next = rnd.Next(animationGroupList.Length);
                var group = animationGroupList[next];
                visual.ImplicitAnimations = group;
                visual.CenterPoint = new Vector3((float)(child.DesiredSize.Width / 2), (float)(child.DesiredSize.Height / 2), 0);
                visual.Offset = new Vector3((float)-child.DesiredSize.Width, (float)-child.DesiredSize.Height, 0);
                _fluidVisuals[child] = visual;
                
            }

            // Unit size of a cell
            var cellSize = new Size(ItemWidth, ItemHeight);

            if ((availableSize.Width < 0.0d) || (availableSize.Width.IsZero())
                || (availableSize.Height < 0.0d) || (availableSize.Height.IsZero())
                || !FluidItems.Any())
            {
                return cellSize;
            }

            // Calculate how many unit cells can fit in the given width (or height) when the 
            // Orientation is Horizontal (or Vertical)
            _cellsPerLine = CalculateCellsPerLine(availableSize, cellSize, Orientation);

            // Convert the children's dimensions from Size to BitSize
            var childData = FluidItems.Select(child => new BitSize
            {
                Width = Math.Max(1, (int)Math.Floor((child.DesiredSize.Width / cellSize.Width) + 0.5)),
                Height = Math.Max(1, (int)Math.Floor((child.DesiredSize.Height / cellSize.Height) + 0.5))
            }).ToList();

            // If all the children have the same size as the cellSize then use optimized code
            // when a child is being dragged
            _isOptimized = !childData.Any(c => (c.Width != 1) || (c.Height != 1));

            int matrixWidth;
            int matrixHeight;
            if (Orientation == Orientation.Horizontal)
            {
                // If the maximum width required by a child is more than the calculated cellsPerLine, then
                // the matrix width should be the maximum width of that child
                matrixWidth = Math.Max(childData.Max(s => s.Width), _cellsPerLine);
                // For purpose of calculating the true size of the panel, the height of the matrix must
                // be set to the cumulative height of all the children
                matrixHeight = childData.Sum(s => s.Height);
            }
            else
            {
                // For purpose of calculating the true size of the panel, the width of the matrix must
                // be set to the cumulative width of all the children
                matrixWidth = childData.Sum(s => s.Width);
                // If the maximum height required by a child is more than the calculated cellsPerLine, then
                // the matrix height should be the maximum height of that child
                matrixHeight = Math.Max(childData.Max(s => s.Height), _cellsPerLine);
            }

            // Create FluidBitMatrix to calculate the size required by the panel
            var matrix = new FluidBitMatrix(matrixHeight, matrixWidth, Orientation);

            var startIndex = 0L;

            foreach (var child in childData)
            {
                var width = child.Width;
                var height = child.Height;

                if (matrix.TryFindRegion(startIndex, width, height, out var cell))
                {
                    matrix.SetRegion(cell, width, height);
                }
                else
                {
                    // If code reached here, it means that the child is too big to be accommodated
                    // in the matrix. Normally this should not occur!
                    throw new InvalidOperationException("Measure Pass: Unable to accommodate child in the panel!");
                }

                if (!OptimizeChildPlacement)
                {
                    // Update the startIndex so that the next child occupies a location which has 
                    // the same (or greater) row and/or column as this child
                    startIndex = (Orientation == Orientation.Horizontal) ? cell.Row : cell.Col;
                }
            }

            // Calculate the true size of the matrix
            var matrixSize = matrix.GetFilledMatrixDimensions();
            // Calculate the size required by the panel
            return new Size(matrixSize.Width * cellSize.Width, matrixSize.Height * cellSize.Height);
        }

        /// <summary>
        /// Handles the Arrange pass during Layout
        /// </summary>
        /// <param name="finalSize">Final Size of the control</param>
        /// <returns>Total size occupied by all the Children</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var cellSize = new Size(ItemWidth, ItemHeight);

            if ((finalSize.Width < 0.0d) || (finalSize.Width.IsZero())
                || (finalSize.Height < 0.0d) || (finalSize.Height.IsZero()))
            {
                finalSize = cellSize;
            }

            // Final size of the FluidWrapPanel
            _panelSize = finalSize;

            if (!FluidItems.Any())
            {
                return finalSize;
            }

            // Calculate how many unit cells can fit in the given width (or height) when the 
            // Orientation is Horizontal (or Vertical)
            _cellsPerLine = CalculateCellsPerLine(finalSize, cellSize, Orientation);
            // Convert the children's dimensions from Size to BitSize
            var childData = FluidItems.ToDictionary(child => child, child => new BitSize
            {
                Width = Math.Max(1, (int)Math.Floor((child.DesiredSize.Width / cellSize.Width) + 0.5)),
                Height = Math.Max(1, (int)Math.Floor((child.DesiredSize.Height / cellSize.Height) + 0.5))
            });

            // If all the children have the same size as the cellSize then use optimized code
            // when a child is being dragged
            _isOptimized = !childData.Values.Any(c => (c.Width != 1) || (c.Height != 1));

            // Calculate matrix dimensions
            int matrixWidth;
            int matrixHeight;
            if (Orientation == Orientation.Horizontal)
            {
                // If the maximum width required by a child is more than the calculated cellsPerLine, then
                // the matrix width should be the maximum width of that child
                matrixWidth = Math.Max(childData.Values.Max(s => s.Width), _cellsPerLine);
                // For purpose of calculating the true size of the panel, the height of the matrix must
                // be set to the cumulative height of all the children
                matrixHeight = childData.Values.Sum(s => s.Height);
            }
            else
            {
                // For purpose of calculating the true size of the panel, the width of the matrix must
                // be set to the cumulative width of all the children
                matrixWidth = childData.Values.Sum(s => s.Width);
                // If the maximum height required by a child is more than the calculated cellsPerLine, then
                // the matrix height should be the maximum height of that child
                matrixHeight = Math.Max(childData.Values.Max(s => s.Height), _cellsPerLine);
            }

            // Create FluidBitMatrix to calculate the size required by the panel
            var matrix = new FluidBitMatrix(matrixHeight, matrixWidth, Orientation);

            var startIndex = 0L;
            _fluidBits.Clear();

            foreach (var child in childData)
            {
                var width = child.Value.Width;
                var height = child.Value.Height;

                if (matrix.TryFindRegion(startIndex, width, height, out var cell))
                {
                    // Set the bits
                    matrix.SetRegion(cell, width, height);
                    // Arrange the child
                    child.Key.Arrange(new Rect(new Point(), child.Key.DesiredSize));
                    // Convert MatrixCell location to actual location
                    var pos = new Vector3((float)(cell.Col * cellSize.Width), (float)(cell.Row * cellSize.Height), 0);
                    // Get the Bit Information for this child
                    BitInfo info;
                    info.Row = cell.Row;
                    info.Col = cell.Col;
                    info.Width = width;
                    info.Height = height;
                    _fluidBits.Add(child.Key, info);

                    var visual = _fluidVisuals[child.Key];
                   _fluidVisuals[child.Key].Offset = pos;
                   
                }
                else
                {
                    // If code reached here, it means that the child is too big to be accommodated
                    // in the matrix. Normally this should not occur!
                    throw new InvalidOperationException("Arrange Pass: Unable to accommodate child in the panel!");
                }
            }

            // All the uninitialized fluid items have been initialized, so clear the list
            _uninitializedFluidItems.Clear();

            // Calculate the maximum cells along the width and height of the FluidWrapPanel
            _maxCellRows = (int)Math.Max(1, Math.Floor(_panelSize.Height / ItemHeight));
            _maxCellCols = (int)Math.Max(1, Math.Floor(_panelSize.Width / ItemWidth));

            return finalSize;
        }

        #endregion
        
        #region Helpers

        /// <summary>
        /// Initialize all Composition related stuff here (Compositor, Animations etc)
        /// </summary>
        private void InitializeComposition()
        {
            var rootVisual = ElementCompositionPreview.GetElementVisual(this);
            // Compositor
            _compositor = rootVisual.Compositor;

            _backgroundLayerVisual = _compositor.CreateLayerVisual();


            _implicitAnimations1st = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations2nd = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations3rd = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations4th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations5th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations6th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations7th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations8th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations9th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations10th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations11th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations12th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations13th = _compositor.CreateImplicitAnimationCollection();
            _implicitAnimations14th = _compositor.CreateImplicitAnimationCollection();

            #region First Wave Animation
            var _firstWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpacityAnimation1st = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpacityAnimation1st, (double)0);

            var scaleAnimation1st = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation1st, (double)0);

            _firstWaveAnimationGroup.Add(OpacityAnimation1st);
            _firstWaveAnimationGroup.Add(scaleAnimation1st);
            _implicitAnimations1st["Offset"] = _firstWaveAnimationGroup;
            #endregion

            #region Second Wave Animation
            var _secondWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var opacityAnimation2nd = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(opacityAnimation2nd, (double)80);
            var scaleAnimation2nd = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation2nd, (double)80);

            _secondWaveAnimationGroup.Add(opacityAnimation2nd);
            _secondWaveAnimationGroup.Add(scaleAnimation2nd);
            _implicitAnimations2nd["Offset"] = _secondWaveAnimationGroup;
            #endregion

            #region Third Wave Animation
            var _thirdWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpacityAnimation3rd = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpacityAnimation3rd, (double)160);

            var scaleAnimation3rd = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation3rd, (double)160);

            _thirdWaveAnimationGroup.Add(OpacityAnimation3rd);
            _thirdWaveAnimationGroup.Add(scaleAnimation3rd);
            _implicitAnimations3rd["Offset"] = _thirdWaveAnimationGroup;
            #endregion

            #region Fourth Wave Animation
            var _fourthWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpactityAnimation4th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpactityAnimation4th, (double)240);

            var scaleAnimation4th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation4th, (double)240);

            _fourthWaveAnimationGroup.Add(OpactityAnimation4th);
            _fourthWaveAnimationGroup.Add(scaleAnimation4th);
            _implicitAnimations4th["Offset"] = _fourthWaveAnimationGroup;
            #endregion

            #region Fifth Wave Animation
            var _fifthWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpacityAnimation5th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpacityAnimation5th, (double)320);

            var scaleAnimation5th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation5th, (double)320);

            _fifthWaveAnimationGroup.Add(OpacityAnimation5th);
            _fifthWaveAnimationGroup.Add(scaleAnimation5th);
            _implicitAnimations5th["Offset"] = _fifthWaveAnimationGroup;
            #endregion

            #region Sixth Wave Animation
            var _sixthWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpacityAnimation6th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpacityAnimation6th, (double)400);

            var scaleAnimation6th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation6th, (double)400);

            _sixthWaveAnimationGroup.Add(OpacityAnimation6th);
            _sixthWaveAnimationGroup.Add(scaleAnimation6th);
            _implicitAnimations6th["Offset"] = _sixthWaveAnimationGroup;
            #endregion

            #region Seventh Wave Animation
            var _seventhWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpactiyAnimation7th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpactiyAnimation7th, (double)480);

            var scaleAnimation7th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation7th, (double)480);

            _seventhWaveAnimationGroup.Add(OpactiyAnimation7th);
            _seventhWaveAnimationGroup.Add(scaleAnimation7th);
            _implicitAnimations7th["Offset"] = _seventhWaveAnimationGroup;
            #endregion

            #region Eighth Wave Animation
            var _eighthWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpactiyAnimation8th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpactiyAnimation8th, (double)560);

            var scaleAnimation8th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation8th, (double)560);

            _eighthWaveAnimationGroup.Add(OpactiyAnimation8th);
            _eighthWaveAnimationGroup.Add(scaleAnimation8th);
            _implicitAnimations8th["Offset"] = _eighthWaveAnimationGroup;
            #endregion

            #region 9th Wave Animation
            var _ninthWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpactiyAnimation9th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpactiyAnimation9th, (double)640);

            var scaleAnimation9th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation9th, (double)640);

            _ninthWaveAnimationGroup.Add(OpactiyAnimation9th);
            _ninthWaveAnimationGroup.Add(scaleAnimation9th);
            _implicitAnimations9th["Offset"] = _ninthWaveAnimationGroup;
            #endregion

            #region 10th Wave Animation
            var _tenthWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpactiyAnimation10th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpactiyAnimation10th, (double)720);

            var scaleAnimation10th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation10th, (double)720);

            _tenthWaveAnimationGroup.Add(OpactiyAnimation10th);
            _tenthWaveAnimationGroup.Add(scaleAnimation10th);
            _implicitAnimations10th["Offset"] = _tenthWaveAnimationGroup;
            #endregion

            #region 11th Wave Animation
            var _eleventhWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpactiyAnimation11th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpactiyAnimation11th, (double)800);

            var scaleAnimation11th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation11th, (double)800);

            _eleventhWaveAnimationGroup.Add(OpactiyAnimation11th);
            _eleventhWaveAnimationGroup.Add(scaleAnimation11th);
            _implicitAnimations11th["Offset"] = _eleventhWaveAnimationGroup;
            #endregion

            #region 12th Wave Animation
            var _twelvethWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpactiyAnimation12th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpactiyAnimation12th, (double)880);

            var scaleAnimation12th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation12th, (double)880);

            _twelvethWaveAnimationGroup.Add(OpactiyAnimation12th);
            _twelvethWaveAnimationGroup.Add(scaleAnimation12th);
            _implicitAnimations12th["Offset"] = _twelvethWaveAnimationGroup;
            #endregion

            #region 13th Wave Animation
            var _thirdteenthWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpactiyAnimation13th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpactiyAnimation13th, (double)960);

            var scaleAnimation13th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation13th, (double)960);

            _thirdteenthWaveAnimationGroup.Add(OpactiyAnimation8th);
            _thirdteenthWaveAnimationGroup.Add(scaleAnimation8th);
            _implicitAnimations13th["Offset"] = _thirdteenthWaveAnimationGroup;
            #endregion

            #region 14th Wave Animation
            var _fourteenthWaveAnimationGroup = _compositor.CreateAnimationGroup();

            var OpactiyAnimation14th = _compositor.CreateScalarKeyFrameAnimation();
            OpacityAnimationSetting(OpactiyAnimation14th, (double)1040);

            var scaleAnimation14th = _compositor.CreateVector3KeyFrameAnimation();
            ScaleAnimationSetting(scaleAnimation14th, (double)1040);

            _fourteenthWaveAnimationGroup.Add(OpactiyAnimation14th);
            _fourteenthWaveAnimationGroup.Add(scaleAnimation14th);
            _implicitAnimations14th["Offset"] = _fourteenthWaveAnimationGroup;
            #endregion

        }

        private void ScaleAnimationSetting(Vector3KeyFrameAnimation animation, double delay)
        {
            animation.InsertKeyFrame(0.0f, new Vector3(0.7f, 0.7f, 0f));
            animation.InsertKeyFrame(1.0f, new Vector3(1f, 1f, 0f), _compositor.CreateLinearEasingFunction());
            animation.Duration = TimeSpan.FromMilliseconds(600);
            animation.DelayTime = TimeSpan.FromMilliseconds(delay);
            animation.Target = "Scale";
        }

        private void OpacityAnimationSetting(ScalarKeyFrameAnimation animation, double delay)
        {
            animation.InsertKeyFrame(0.6f, 0.0f);
            animation.InsertKeyFrame(1.0f, 1.0f, _compositor.CreateLinearEasingFunction());
            animation.Duration = TimeSpan.FromMilliseconds(450);
            animation.DelayTime = TimeSpan.FromMilliseconds(delay);
            animation.DelayBehavior = AnimationDelayBehavior.SetInitialValueAfterDelay;
            animation.Target = "Opacity";
        }

        /// <summary>
        /// Removes all the children from the FluidWrapPanel
        /// </summary>
        private void ClearItemsSource()
        {
            _fluidVisuals.Clear();
            FluidItems.Clear();
            Children.Clear();
        }
       
        #endregion
        
        #region Static Helpers

        /// <summary>
        /// Calculates the number of child items that can be accommodated in a single line
        /// </summary>
        private static int CalculateCellsPerLine(Size panelSize, Size cellSize, Orientation panelOrientation)
        {
            var count = (panelOrientation == Orientation.Horizontal) ? panelSize.Width / cellSize.Width :
                panelSize.Height / cellSize.Height;
            return Math.Max(1, (int)Math.Floor(count));
        }

        #endregion
    }
}
