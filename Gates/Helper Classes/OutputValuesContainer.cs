using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gates.Helper_Classes
{
    /// <summary>
    /// Holds a list of outputs and creates an event (OutputChanged) when one is changed.
    /// </summary>
    class OutputValuesContainer
    {
        private bool[] values;

        /// <summary>
        /// Get & set the appropriate outputs.
        /// When an output is set, an event is created to notify GElements subscribing
        /// to that output that it has changed and that they need to update themselves.
        /// </summary>
        /// <param name="i">The output to change</param>
        /// <returns>The output requested</returns>
        public bool this[int i]
        {
            get
            {
                return values[i];
            }
            set
            {
                if (value == !values[i])
                {
                    values[i] = value;
                    OnOutputChanged(i);
                }
            }
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
