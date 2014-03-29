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
    public sealed partial class GWire : UserControl
    {
        private GWireModel model;
        // Collection of the segments of the visual representation of the wire
        private string PathData { get; set; }

        public GWire()
        {
            model = new GWireModel();
            this.DataContext = model;
            WirePath.DataContext = this;

            this.InitializeComponent();
        }
        //HVH

        /// <summary>
        /// Create a GWire from the xy coordinates of two GPrimitives.
        /// Not really type safe, I guess. Perhaps in the future.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public GWire(double x1, double y1, double x2, double y2)
        {
            model = new GWireModel();
            this.DataContext = model;
            WirePath.DataContext = this;

            PathData = CalculatePathSegmentsFromParentCoordinates(x1, y1, x2, y2);

            this.InitializeComponent();
        }

        /// <summary>
        /// Encodes a string with Move & Draw commands describing the path
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public string CalculatePathSegmentsFromParentCoordinates(double x1, double y1, double x2, double y2)
        {
            string path = null;
            
            double pathHeight = Math.Abs(x1 - x2);;
            double pathWidth = Math.Abs(y1 - y2);;

            string nCoords = String.Format("{0},{1}", x1, y1);
            string h1Coords = (pathWidth / 2.0).ToString();
            string v1Coords = y2.ToString();
            string h2Coords = x2.ToString();

            StringWriter writer = new StringWriter();

            path = string.Format("N{0} h{1} v{2} h{3} z", nCoords, h1Coords, v1Coords, h2Coords);
            return path;
        }

        /// <summary>
        /// Update the view of the view when one of the parent controls moves.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void UpdateView(double x1, double y1, double x2, double y2)
        {

        }
    }
}
