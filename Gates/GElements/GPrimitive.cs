using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// <summary>
    /// A basic component of a circuit, such as an AND gate or inverter. 
    /// When a GElement connected to an input changes its output value, that Component
    /// is responsible for updating the proper input of this GPrimitive, which in turn
    /// calls the Propagate() method to calculate its own outputs and continue updating 
    /// the rest of the circuit. 
    /// Future changes to class may involve replacing current recursive method of
    /// updating GElements with a system of event handlers.
    /// </summary>
    public sealed class GPrimitive : GComponent
    {
        #region Properties

        /// <summary>
        /// The type of basic logic gate this primitive represents.
        /// 0 = And, 1 = Or, 2 = Nand, 3 = Nor, 4 = Xor, 5 = Invert.
        /// </summary>
        private int _type {get; set;}
        public int type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value > 5)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    _type = value;
                }
            }
        }

        /// <summary>
        /// The number of inputs to this GPrimitive.
        /// </summary>
        private int _numInputs { get; set; }
        public override int numInputs
        {
            get 
            { 
                return _numInputs; 
            }

            set 
            {
                if (type == 5 && value != 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else if (type == 4 && value != 2)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else if (value <= 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    _numInputs = value;

                    // Update the _inputValues array's size to reflect new number of inputs
                    bool[] newInputs = new bool[value];
                    int i = 0;

                    foreach (bool x in inputCache)
                    {
                        newInputs[i] = x;
                        i++;
                    }
                    inputCache = newInputs;
                }
            }
        }

        // Replace with InputValuesContainer
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
        /// An array of the IElements whose outputs link to this GPrimitive's inputs.
        /// </summary>
        private GElement[] _inputElements { get; set; }
        public GElement[] inputElements
        {
            get
            {
                return _inputElements;
            }
            set
            {
                _inputElements = value;
            }
        }

        /// <summary>
        /// The output of this primitive.
        /// </summary>
        private bool _outputValue { get; set; }
        public bool outputValue
        {
            get
            {
                return _outputValue;
            }
            set
            {
                _outputValue = value;
            }
        }

        #endregion

        /// <summary>
        /// Create a GPrimitive object from an integer representing the desired type.
        /// </summary>
        /// <param name="type">The desired type 
        /// (0 = And, 1 = Or, 2 = Nand, 3 = Nor, 4 = Xor, 5 = Invert.)</param>
        public GPrimitive()
        {
            this.DefaultStyleKey = typeof(GPrimitive);
            this.type = 1;
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
            // Map the sending GElement to its input
            for (int i = 0; i < inputElements.Length; i++)
            {
                // Once found, set the value 
                if (inputElements[i] == inputElement)
                {
                    bool succeeded;
                    try
                    {
                        inputCache[i] = value;
                        this.Propagate();
                        succeeded = true;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        succeeded = false;
                        // I really don't know what I can productively do here so I'm doing the msg thing below
                    }
                    if (!succeeded)
                    {
                        Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog("Tried to assign value to nonexistant input");
                        await msg.ShowAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Set the output value of this GPrimitive, and notify the GElement
        /// connected to the output that its value has changed. 
        /// I kept this separate from the property itself in case an output would
        /// ever need to be changed without the results propagating. 
        /// </summary>
        /// <param name="newOutput">The new output value</param>
        public void SetOutput(bool newOutput)
        {
            if (outputValue == !newOutput)
            {
                outputValue = !outputValue;
                OnOutputChanged();
                // outputElement.setInput(outputValue, this);
            }
            
        }

        protected override void Propagate()
        {
            bool result = false;

            switch (type)
            {
                case 0: // AND
                    result = inputCache[0] & inputCache[1];
                    if (numInputs > 2)
                    {
                        for (int i = 2; i < inputCache.Length; i++)
                        {
                            result = result & inputCache[i];
                        }
                    }
                    break;
                case 1: // OR
                    result = inputCache[0] | inputCache[1];
                    if (numInputs > 2)
                    {
                        for (int i = 2; i < inputCache.Length; i++)
                        {
                            result = result | inputCache[i];
                        }
                    }
                    break;
                case 2: // NAND
                    result = inputCache[0] & inputCache[1];
                    if (numInputs > 2)
                    {
                        for (int i = 2; i < inputCache.Length; i++)
                        {
                            result = result & inputCache[i];
                        }
                    }
                    result = !result;
                    break;
                case 3: // NOR
                    result = inputCache[0] | inputCache[1];
                    if (numInputs > 2)
                    {
                        for (int i = 2; i < inputCache.Length; i++)
                        {
                            result = result | inputCache[i];
                        }
                    }
                    result = !result;
                    break;
                case 4: // XOR (currently only supports 2 inputValues)
                    result = inputCache[0] ^ inputCache[1];
                    break;
                case 5: // Inverter
                    result = !inputCache[0];
                    break;
            }
            SetOutput(result);
        }

        public event EventHandler OutputChanged;

        /// <summary>
        /// Create an event handler in the case that this GPrimitive's output
        /// is changed. 
        /// </summary>
        private void OnOutputChanged()
        {
            EventHandler handler = OutputChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
        
        /// <summary>
        /// Update the view when the type is changed
        /// </summary>
        /// <param name="type"></param>
        private void TypeChanged(int type)
        {
            switch(type)
            {
                case 0:
                    break;
                case 1:
                    break;
            }
        }

        private void OnImageLoaded() // replace with OnLoaded and OnTypeChanged?
        {

        }
    }
}
