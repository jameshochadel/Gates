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
    public sealed partial class GPrimitive : UserControl
    {
        private GPrimitiveModel model { get; set; }

        public GPrimitive()
        {
            model = new GPrimitiveModel();
            this.DataContext = model;
            this.InitializeComponent();
        }

        public static DependencyProperty GateType = DependencyProperty.Register("GateType", typeof(string), typeof(GPrimitive), new PropertyMetadata("0"));
    }
}
