using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;
using Gates;
using Gates.GElements;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Gates.GElements
{
    /// <summary>
    /// Simulate a basic logic gate.
    /// </summary>
    /// <remarks>
    /// When instantiating a new GPrimitive on the Editor canvas, be sure to subscribe its event 
    /// handlers to the proper events, or the GPrimitive won't do much.
    /// </remarks>
    public sealed partial class GPrimitive : UserControl // TODO: Inherit interface InputCapableControl and OutputCapableControl
    {
        // Model containing data about this GPrimitive
        private GPrimitiveModel model { get; set; }
        public GWireHandle ChildWireHandle;
        
        // Transform variables
        // All transform code adapted from http://fredbesterwitch.blogspot.com/2011/02/silverlight-drag-drop-snap-to-grid.html
        // Potential manipulation code replacement: http://msdn.microsoft.com/en-us/library/windows/apps/hh465387.aspx

        /// <summary>
        /// Create a GPrimitive with the default GateType (AND).
        /// </summary>
        public GPrimitive()
        {
            model = new GPrimitiveModel();
            this.DataContext = model;

            this.InitializeComponent();
            
            this.ManipulationMode = ManipulationModes.System | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            this.ManipulationStarting += new ManipulationStartingEventHandler(ElementManipulationStarting);
            this.ManipulationStarted += new ManipulationStartedEventHandler(ElementManipulationStarted);
            this.ManipulationDelta += new ManipulationDeltaEventHandler(ElementManipulationDelta);
            this.ManipulationCompleted += new ManipulationCompletedEventHandler(ElementManipulationCompleted);

            this.RenderTransform = null;
        }

        /// <summary>
        /// Create a GPrimitive of a specific GateType.
        /// </summary>
        /// <param name="gateType">The gate type</param>
        public GPrimitive(int gateType)
        {
            model = new GPrimitiveModel();
            model.GateType = gateType;
            this.DataContext = model;

            this.InitializeComponent();

            this.ManipulationMode = ManipulationModes.System | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            this.ManipulationStarting += new ManipulationStartingEventHandler(ElementManipulationStarting);
            this.ManipulationStarted += new ManipulationStartedEventHandler(ElementManipulationStarted);
            this.ManipulationDelta += new ManipulationDeltaEventHandler(ElementManipulationDelta);
            this.ManipulationCompleted += new ManipulationCompletedEventHandler(ElementManipulationCompleted);

            this.RenderTransform = null;
        }

        /// <summary>
        /// Code for manipulating the GPrimitive on the application canvas.
        /// </summary>
        #region Manipulation Methods

        void ElementManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            e.Handled = true;
        }

        void ElementManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.PrimitiveControlImage.Width += 20;
            this.PrimitiveControlImage.Height += 20;
            e.Handled = true;
        }

        void ElementManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            e.Handled = true;
        }

        void ElementManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            //e.Handled = true;
        }

        void ElementManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.PrimitiveControlImage.Width -= 20;
            this.PrimitiveControlImage.Height -= 20;
            //e.Handled = true; 

            // Fire GPrimitiveMoved event that lets associated GWires update their locations
        }
        #endregion

        /// <summary>
        /// The type of gate this GPrimitive represents
        /// </summary>
        /// <remarks>AND=0, OR=1, NAND=2, NOR=3, XAND=4, INVERT=5</remarks>
        public int GateType
        {
            get { return (int)GetValue(GateTypeProperty); }
            set { SetValue(GateTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GateType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GateTypeProperty =
            DependencyProperty.Register("GateType", typeof(int), typeof(GPrimitive), new PropertyMetadata(0, new PropertyChangedCallback(OnGateTypeChanged)));


        private static void OnGateTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var instance = d as GPrimitive;
            if (instance != null) {
                instance.model.UpdateGateType(instance.GateType);
            }
        }

        #region Non-drag user interaction event handlers
        /// <summary>
        /// Show the i/o clickzones on pointer enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GPrimitive_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // raise event that Editor.CurrentEditor is subscribed to so it may respond properly
            EventHandler<PointerRoutedEventArgs> handler = GPrimitive_PointerEntered;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<PointerRoutedEventArgs> GPrimitive_PointerEntered;

        /// <summary>
        /// Hide the i/o clickzones on pointer exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GPrimitive_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            EventHandler<PointerRoutedEventArgs> handler = GPrimitive_PointerExited;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<PointerRoutedEventArgs> GPrimitive_PointerExited;

        private void GPrimitive_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            EventHandler<TappedRoutedEventArgs> handler = GPrimitive_Tapped;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<TappedRoutedEventArgs> GPrimitive_Tapped;

        private void GPrimitive_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            EventHandler<RightTappedRoutedEventArgs> handler = GPrimitive_RightTapped;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<RightTappedRoutedEventArgs> GPrimitive_RightTapped;
        #endregion
    }
}
