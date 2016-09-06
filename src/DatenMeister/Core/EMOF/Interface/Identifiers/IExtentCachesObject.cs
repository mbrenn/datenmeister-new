using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Interface.Identifiers
{
    public interface IExtentCachesObject
    {
        /// <summary>
        /// Returns true, if the object is in the given extent. 
        /// This interface can be implemented to speed up access 
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>true, if object is in</returns>
        bool HasObject(IObject value);
    }
}