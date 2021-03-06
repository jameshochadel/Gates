﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Gates.GElements
{
    public class GPrimitiveModel
    {
        #region Properties

        /// <summary>
        /// The type of basic logic gate this primitive represents.
        /// 0 = And, 1 = Or, 2 = Nand, 3 = Nor, 4 = Xor, 5 = Invert.
        /// </summary>
        private int _gateType { get; set; }
        public int GateType
        {
            get
            {
                return _gateType;
            }
            set
            {
                if (value > 5)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    _gateType = value;
                }
            }
        }

        /// <summary>
        /// The number of inputs to this GPrimitive.
        /// </summary>
        public int NumInputs
        {
            get
            {
                return NumInputs;
            }

            set
            {
                if (GateType == 5 && value != 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else if (GateType == 4 && value != 2)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else if (value <= 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    NumInputs = value;

                    // Update the _inputValues array's size to reflect new number of inputs
                    bool[] newInputs = new bool[value];
                    int i = 0;

                    foreach (bool x in InputCache)
                    {
                        newInputs[i] = x;
                        i++;
                    }
                    InputCache = newInputs;
                }
            }
        }

        // Replace with InputValuesContainer
        /// <summary>
        /// Keeps track of the current inputs to this GPrimitive.
        /// </summary>
        public bool[] InputCache { get; set; }

        /// <summary>
        /// An array of the IElements whose outputs link to this GPrimitive's inputs.
        /// </summary>
        public GElement[] InputElements { get; set; }

        /// <summary>
        /// The output of this primitive.
        /// </summary>
        public bool OutputValue { get; set; }

        public ImageSource PrimitiveBackground { get; set; }
        #endregion

        #region Constructors
        public GPrimitiveModel()
        {
            UpdateGateType(0);
        }

        public GPrimitiveModel(int gateType)
        {
            GateType = gateType;
            UpdateGateType(GateType);
        }
        #endregion // constructors

        /// <summary>
        /// Change the input of this GPrimitive. May remove in favor of event handler
        /// Future note: Add "if" statement to prevent calling Propagate() unless
        /// the value set is different from the value that was already in place.
        /// </summary>
        /// <param name="inputElement">The element sending the updated output</param>
        /// <param name="value">The new value</param>
        public async void SetInput(GElement inputElement, bool value)
        {
            // Map the sending GElement to its input
            for (int i = 0; i < InputElements.Length; i++) // Todo: replace with input values container code
            {
                // Once found, set the value 
                if (InputElements[i] == inputElement)
                {
                    bool succeeded;
                    try
                    {
                        InputCache[i] = value;
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
            if (OutputValue == !newOutput)
            {
                OutputValue = !OutputValue;
                OnOutputChanged();
                // outputElement.setInput(OutputValue, this);
            }

        }

        private void Propagate()
        {
            bool result = false;

            switch (GateType)
            {
                case 0: // AND
                    result = InputCache[0] & InputCache[1];
                    if (NumInputs > 2)
                    {
                        for (int i = 2; i < InputCache.Length; i++)
                        {
                            result = result & InputCache[i];
                        }
                    }
                    break;
                case 1: // OR
                    result = InputCache[0] | InputCache[1];
                    if (NumInputs > 2)
                    {
                        for (int i = 2; i < InputCache.Length; i++)
                        {
                            result = result | InputCache[i];
                        }
                    }
                    break;
                case 2: // NAND
                    result = InputCache[0] & InputCache[1];
                    if (NumInputs > 2)
                    {
                        for (int i = 2; i < InputCache.Length; i++)
                        {
                            result = result & InputCache[i];
                        }
                    }
                    result = !result;
                    break;
                case 3: // NOR
                    result = InputCache[0] | InputCache[1];
                    if (NumInputs > 2)
                    {
                        for (int i = 2; i < InputCache.Length; i++)
                        {
                            result = result | InputCache[i];
                        }
                    }
                    result = !result;
                    break;
                case 4: // XOR (currently only supports 2 inputValues)
                    result = InputCache[0] ^ InputCache[1];
                    break;
                case 5: // Inverter
                    result = !InputCache[0];
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
        public void UpdateGateType(int gateType)
        {
            if (GateType == 5 && gateType != 5)
            {

            }
            else if (GateType != 5 && gateType == 5)
            {

            }
            GateType = gateType;
            BitmapImage image = new BitmapImage();
            image.UriSource = new Uri(String.Format("ms-appx:/Assets/{0}.png", GateType.ToString()));
            PrimitiveBackground = image;
        }

        private void OnImageLoaded() // replace with OnLoaded and OnTypeChanged?
        {

        }
    }
}
