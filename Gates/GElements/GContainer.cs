using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Gates.GElements
{
    public sealed class GContainer : GComponent
    {
        public GContainer()
        {
            this.DefaultStyleKey = typeof(GContainer);
        }

        /// <summary>
        /// Change the input of the GElement that this GPrimitive outputs to.
        /// Future note: Add "if" statement to prevent calling Propagate() unless
        /// the value set is different from the value that was already in place.
        /// </summary>
        /// <param name="inputElement">The element sending the updated output</param>
        /// <param name="value">The new value</param>
        public override async void SetInput(GElement inputElement, bool value)
        {

        }

        /// <summary>
        /// Set the output value of this GPrimitive, and notify the GElement
        /// connected to the output that its value has changed. 
        /// I kept this separate from the property itself in case an output would
        /// ever need to be changed without the results propagating. 
        /// </summary>
        /// <param name="newOutput">The new output value</param>
        public override void SetOutput(bool newOutput)
        {

        }

        public override void Propagate()
        {

        }

        public event EventHandler<int> OutputChanged;

        /// <summary>
        /// Create an event in the case that one of this GElement's outputs is changed. 
        /// </summary>
        /// <param name="numOutput">The number of the output that has changed</param>
        protected void OnOutputChanged(int numOutput)
        {
            EventHandler<int> handler = OutputChanged;
            if (handler != null)
            {
                handler(this, numOutput);
            }
        }
    }
}
