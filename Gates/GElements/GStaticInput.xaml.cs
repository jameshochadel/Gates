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
    public sealed partial class GStaticInput : UserControl
    {
        public bool InputValue
        {
            get
            {
                if (_inputValue == 1)
                    return true;
                else
                    return false;
            }
        }
        private int _inputValue
        {
            get;
            set;
        }

        public GStaticInput()
        {
            _inputValue = 1;
            this.DataContext = this;
            this.InitializeComponent();
        }

        private void MenuFlyout_click(object sender, RoutedEventArgs e)
        {
            if (sender == Input0)
            {
                _inputValue = 0;
            }
            else if (sender == Input1)
            {
                _inputValue = 1;
            }
        }
    }
}
