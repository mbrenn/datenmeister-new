using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core.EMOF.Interface.Reflection
{
    public interface ISetKnownExtents
    {

        /// <summary>
        /// Adds the element to the given extent
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        void AddToExtent(IExtent extent);

        /// <summary>
        /// Removes the element from the extent
        /// </summary>
        /// <param name="extent">Extent to be queried</param>
        void RemoveFromExtent(IExtent extent);
    }
}