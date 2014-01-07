using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gates.GElements
{
    /// <summary>
    /// GComponents are discrete GElements which do some sort of data processing. 
    /// Wires are not GComponents, for example, because they only transfer data
    /// without performing any operations on it.
    /// </summary>
    abstract class GComponent : GElement
    {

    }
}
