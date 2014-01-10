using Gates.GElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Gates
{
    /// <summary>
    /// Base class for all circuit elements in Gates. 
    /// The full connected set of GElements in any given Gates file can
    /// be thought of as a doubly-linked list with branching.
    /// Inherits from the generic XAML Control.
    /// </summary>
    public abstract class GElement : Control
    {
        /// <summary>
        /// The number of inputs to this GPrimitive.
        /// </summary>
        private int _numInputs { get; set; }
        public virtual int numInputs
        {
            get
            {
                return _numInputs;
            }

            set
            {
                _numInputs = value;
            }
        }

        /// <summary>
        /// Keeps track of the current inputs to this GPrimitive.
        /// </summary>
        private bool[] _inputCache { get; set; }
        public bool[] inputCache
        {
            get
            {
                return _inputCache;
            }
            set
            {
                _inputCache = value;
            }
        }

        /// <summary>
        /// Set the value of a given input for this component.
        /// The input to be set is determined by comparing the sending GElement
        /// to the list of this GElement's inputs. 
        /// </summary>
        /// <param name="inputElement">The element sending the input</param>
        /// <param name="value">The value to be set</param>
        public abstract void SetInput(GElement inputElement, bool value);

        /// <summary>
        /// Set the output value of this GElement. Generates an event announcing the change.
        /// I kept this separate from the property itself in case an output would
        /// ever need to be changed without the results propagating. 
        /// </summary>
        /// <param name="newOutput">The new output value</param>
        public abstract void SetOutput(GElement outputElement, bool newOutput);

        /// <summary>
        /// From the given inputs, determine the output.
        /// Note: Currently includes no code for timing delays needed by simulation model
        /// </summary>
        public abstract void Propagate();
    }
}
