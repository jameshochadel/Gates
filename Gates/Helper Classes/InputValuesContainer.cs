using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gates.Helper_Classes
{
    class InputValuesContainer
    {
        GElement parent = null;
        
        Dictionary<GElement, int> inputs = new Dictionary<GElement, int>();

        private bool[] values;

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
        /// it is connected to, then return or set the value associated with it
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
                    return values[i];
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
                    if (value == !values[i])
                    {
                        values[i] = value;
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
        public void setInput(GElement g, int i)
        {
            inputs.Add(g, i);
        }
    }
}
