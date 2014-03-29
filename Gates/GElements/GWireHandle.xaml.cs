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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Gates.GElements
{
    public sealed partial class GWireHandle : UserControl
    {
        // Transform variables
        // All transform code adapted from Windows 8.1 SDK "AdvancedManipulations" demo.
        // Potential manipulation code replacement: http://msdn.microsoft.com/en-us/library/windows/apps/hh465387.aspx
        
        public GPrimitive ParentPrimitive;

        public GWireHandle()
        {
            this.InitializeComponent();

            this.ManipulationMode = ManipulationModes.System | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            this.ManipulationStarting += new ManipulationStartingEventHandler(ElementManipulationStarting);
            this.ManipulationStarted += new ManipulationStartedEventHandler(ElementManipulationStarted);
            this.ManipulationDelta += new ManipulationDeltaEventHandler(ElementManipulationDelta);
            this.ManipulationCompleted += new ManipulationCompletedEventHandler(ElementManipulationCompleted);

            this.RenderTransform = null;
            InitManipulationTransforms();
        }

        /// <summary>
        /// Code for manipulating the GPrimitive on the application canvas.
        /// </summary>
        #region Manipulation Methods
        private void InitManipulationTransforms()
        {

        }

        void ElementManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            
        }

        void ElementManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            
        }

        void ElementManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            
        }

        void ElementManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

        }
        #endregion
    }
}
