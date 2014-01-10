using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gates.Helper_Classes
{
    /// <summary>
    /// Meant to be a property of a GElement, this holds a list of outputs and their 
    /// last known outputValues. Given a GElement and a bool, the appropriate input can be set 
    /// in response to an OutputChanged event. 
    /// </summary>
    class InputValuesContainer
    {
        /// <summary>
        /// GElement containing this InputValuesContainer
        /// </summary>
        GElement parent = null;
        
        /// <summary>
        /// Maps a GElement to the input it's connected to on parent
        /// </summary>
        Dictionary<GElement, int> inputs = new Dictionary<GElement, int>();

        /// <summary>
        /// Contains the last known outputValues of each input, with indices corresponding to
        /// input numbers.
        /// </summary>
        private bool[] inputCache; // TODO: Code to resize this; steal from GPrimitive

        /// <summary>
        /// Create a new InputValuesContainer which must have a parent GElement
        /// </summary>
        /// <param name="parent">The GElement containing this InputValuesContainer</param>
        public InputValuesContainer(GElement parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Hash the GElement whose output has been updated to find which input
        /// it is connected to, then return or set the value associated with it.
        /// </summary>
        /// <param name="g">The GElement whose output was updated</param>
        /// <exception cref="KeyNotFoundException">Thrown if input GElement can't be found</exception>
        public bool this[GElement g]
        {
            get
            {
                int i = 0;
                if (inputs.TryGetValue(g, out i))
                {
                    return inputCache[i];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            set
            {
                int i = 0;
                if (inputs.TryGetValue(g, out i))
                {
                    if (value == !inputCache[i])
                    {
                        inputCache[i] = value;
                        parent.Propagate();
                    }
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        /// <summary>
        /// Add an input to the Dictionary.
        /// </summary>
        /// <param name="g">The GElement connected to the input</param>
        /// <param name="i">The number of the input</param>
        public void SetInput(GElement g, int i)
        {
            inputs.Add(g, i);
        }
    }
}
