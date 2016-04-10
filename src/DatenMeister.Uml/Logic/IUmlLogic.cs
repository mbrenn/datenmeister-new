using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Logic
{
    public interface IUmlLogic
    {
        /// <summary>
        /// Gets the property of an object by looking up the property within the Uml structure
        /// </summary>
        /// <param name="element">Element, whose property shall be queried</param>
        /// <param name="mapping">This function shall return the function as looked up</param>
        /// <returns>The found property within the element</returns>
        string GetProperty(IElement element, Func<_UML, object> mapping);

        /// <summary>
        /// Gets an enumeration of all classes that can be created within an extent
        /// </summary>
        /// <param name="extent">Extent, where a new class could be created</param>
        /// <returns>Enumeration of IElement</returns>
        IEnumerable<IElement> GetAllClassesThatCanBeCreated(IExtent extent);
    }
}