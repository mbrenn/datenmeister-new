using DatenMeister.EMOF.Interface.Common;

namespace DatenMeister.EMOF.Interface.Identifiers
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
