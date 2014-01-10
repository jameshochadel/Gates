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

namespace Gates.GElements
{
    /// <summary>
    /// GComponents are discrete GElements which do some sort of data processing. 
    /// Wires are not GComponents, for example, because they only transfer data
    /// without performing any operations on it.
    /// </summary>
    public abstract class GComponent : GElement
    {

    }
}
