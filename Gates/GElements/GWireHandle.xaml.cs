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
        private TransformGroup transformGroup;
        private MatrixTransform previousTransform;
        private CompositeTransform compositeTransform;
        private bool forceManipulationsToEnd;

        public GWireHandle()
        {
            this.InitializeComponent();

            this.ManipulationMode = ManipulationModes.System | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            this.ManipulationStarting += new ManipulationStartingEventHandler(ElementManipulationStarting);
            this.ManipulationStarted += new ManipulationStartedEventHandler(ElementManipulationStarted);
            this.ManipulationDelta += new ManipulationDeltaEventHandler(ElementManipulationDelta);
            this.ManipulationCompleted += new ManipulationCompletedEventHandler(ElementManipulationCompleted);
            this.ManipulationInertiaStarting += new ManipulationInertiaStartingEventHandler(ElementManipulationInertiaStarting);

            forceManipulationsToEnd = true;
            this.RenderTransform = null;
            InitManipulationTransforms();
        }

        /// <summary>
        /// Code for manipulating the GPrimitive on the application canvas.
        /// </summary>
        #region Manipulation Methods
        private void InitManipulationTransforms()
        {
            //Initialize the transforms
            transformGroup = new TransformGroup();
            compositeTransform = new CompositeTransform();
            previousTransform = new MatrixTransform() { Matrix = Matrix.Identity };

            transformGroup.Children.Add(previousTransform);
            transformGroup.Children.Add(compositeTransform);

            this.RenderTransform = transformGroup;
        }

        void ElementManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            forceManipulationsToEnd = false;
            e.Handled = true;
        }

        void ElementManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        void ElementManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            e.Handled = true;
        }

        void ElementManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (forceManipulationsToEnd)
            {
                e.Complete();
                return;
            }
            //Set the new transform values based on user action
            previousTransform.Matrix = transformGroup.Value;
            compositeTransform.TranslateX = e.Delta.Translation.X /*/ Editor.CircuitCanvasScrollViewer.ZoomFactor*/;
            compositeTransform.TranslateY = e.Delta.Translation.Y /*/ scrollViewer.ZoomFactor*/;
            e.Handled = true;
        }

        void ElementManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;
        }
        #endregion
    }
}
