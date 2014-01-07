using Gates.GElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gates
{
    /// <summary>
    /// Base class for all circuit elements in Gates. 
    /// The full connected set of GElements in any given Gates file can
    /// be thought of as a doubly-linked list with branching.
    /// </summary>
    abstract class GElement
    {
        /// <summary>
        /// The number of inputs to this GPrimitive.
        /// </summary>
        private int _numInputs { get; set; }
        public int numInputs
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
        private bool[] _inputValues { get; set; }
        public bool[] inputValues
        {
            get
            {
                return _inputValues;
            }
            set
            {
                _inputValues = value;
            }
        }

        /// <summary>
        /// Set the value of a given input for this component.
        /// The input to be set is determined by comparing the sending GElement
        /// to the list of this GPrimitive's inputs. 
        /// </summary>
        /// <param name="inputElement">The element sending the input</param>
        /// <param name="value">The value to be set</param>
        internal void setInput(bool inputElement, GPrimitive value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the output value of this GPrimitive, and notify the GElement
        /// connected to the output that its value has changed. 
        /// I kept this separate from the property itself in case an output would
        /// ever need to be changed without the results propagating. 
        /// </summary>
        /// <param name="newOutput">The new output value</param>
        public void setOutputAndUpdate(bool newOutput)
        {

        }

        /// <summary>
        /// From the given inputs, determine the output.
        /// Note: Currently includes no code for timing delays needed by simulation model
        /// </summary>
        public void Propagate()
        {

        }
    }
}
