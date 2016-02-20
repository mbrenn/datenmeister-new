using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.EMOF.Interface.Reflection
{
    /// <summary>
    /// This interface shall be implemented by all objects, which are allocated to an extent. 
    /// </summary>
    public interface IObjectHasExtent
    {
        IExtent GetContainingExtent();
    }
}