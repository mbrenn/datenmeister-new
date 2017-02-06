using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    /// <summary>
    /// Defines a lookup from MOF element to dotnet type and vice
    /// versa. This class is used to figure out how the mapping between 
    /// DotNet types and MOF elements is performed, so the correct type
    /// is always created
    /// </summary>
    public interface IDotNetTypeLookup
    {
        /// <summary>
        /// Adds an association between type and element
        /// </summary>
        /// <param name="elementUri">Uri of the metaclass to be added</param>
        /// <param name="type">Type to be added</param>
        void Add(string elementUri, Type type);

        string ToElement(Type type);

        /// <summary>
        /// Finds the element by the uri and converts the element to a real .Net Type
        /// </summary>
        /// <param name="elementUri">Uri of the element to be converted</param>
        /// <returns>Converted element</returns>
        Type ToType(string elementUri);

        /// <summary>
        /// Gets the id of a certain element. The method is used to support the caching
        /// of the elements, so the same .Net element always have the same id
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>The returned id</returns>
        string GetId(object value);
    }
}