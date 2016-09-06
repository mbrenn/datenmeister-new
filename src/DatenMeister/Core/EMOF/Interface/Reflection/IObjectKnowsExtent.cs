using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core.EMOF.Interface.Reflection
{
    /// <summary>
    /// This interface can be implemented by all objects which
    /// knows the extents, to which they are hosted to. This speeds
    /// up the lookup for datalayers and containing events and should be 
    /// implemented by every object
    /// </summary>
    public interface IObjectKnowsExtent
    {
        /// <summary>
        /// Gets the list of all extents where the object is stored.
        /// If the interface is implemented, the list needs to be complete
        /// </summary>
        IEnumerable<IExtent> Extents { get; }
    }
}