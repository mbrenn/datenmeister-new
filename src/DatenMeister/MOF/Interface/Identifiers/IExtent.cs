using DatenMeister.MOF.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Interface.Identifiers
{
    /// <summary>
    /// Defines the interface for the Extent as defined in MOF Specification 2.5, Chapter 10.2
    /// </summary>
    public interface IExtent
    {
        bool useContainment();

        IReflectiveSequence elements();
    }
}
