using System.Collections.Generic;

namespace DatenMeister.EMOF.Interface.Reflection
{
    /// <summary>
    ///     Defines the interface for some additional reflection methods to
    ///     ease access with typeless objects.
    ///     This interface is not MOF-compliant and shall only be used when really
    ///     needed for typeless objects
    /// </summary>
    public interface IObjectAllProperties
    {
        /// <summary>
        ///     Returns an enumeration of all Properties which are currently set in the object
        /// </summary>
        /// <returns>Enumeration of all properties being set</returns>
        IEnumerable<string> getPropertiesBeingSet();
    }
}