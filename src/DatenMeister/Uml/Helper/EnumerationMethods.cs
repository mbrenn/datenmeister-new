using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Runtime;

namespace DatenMeister.Uml.Helper
{
    /// <summary>
    /// Defines the helper methods applicable to the Enumeration
    /// </summary>
    public class EnumerationMethods
    {
        /// <summary>
        /// Gets the enumeration values as the complete objects
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static IReflectiveCollection GetEnumValueObjects(IObject enumeration)
        {
            var result =
                enumeration.getOrDefault<IReflectiveCollection>(_UML._SimpleClassifiers._Enumeration.ownedLiteral);
            return result;
        }

        /// <summary>
        /// Gets the enumeration values themselves
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetEnumValues(IObject enumeration)
        {
            var values = GetEnumValueObjects(enumeration);
            if (values == null)
            {
                return Array.Empty<string>();
            }

            return values.OfType<IObject>()
                .Select(x => x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name));
        }
    }
}