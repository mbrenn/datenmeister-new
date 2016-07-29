using System;
using DatenMeister.EMOF.Interface.Reflection;

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
        /// <param name="element">Element to be added</param>
        /// <param name="type">Type to be added</param>
        void Add(IElement element, Type type);

        IElement ToElement(Type type);

        Type ToType(IElement element);
    }
}