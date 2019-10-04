using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core.EMOF.Interface.Reflection
{
    /// <summary>
    /// This interface can be implemented by all objects which
    /// knows the extents, to which they are hosted to. This speeds
    /// up the lookup for datalayers and containing events and should be
    /// implemented by every object
    /// </summary>
    public interface IHasExtent
    {
        /// <summary>
        /// Gets the list extent where the object is stored.
        /// </summary>
        IExtent Extent { get; }
    }
}