using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Helper
{
    /// <summary>
    ///     Includes several methods to support the handling of objects
    /// </summary>
    public static class ObjectHelper
    {
        public static Dictionary<object, object> AsDictionary(
            this IObject value,
            IEnumerable<object> properties)
        {
            var result = new Dictionary<object, object>();

            foreach (var property in properties
                .Where(property => value.isSet(property)))
            {
                result[property] = value.get(property);
            }

            return result;
        }

        public static Dictionary<string, string> AsStringDictionary(
            this IObject value,
            IEnumerable<object> properties)
        {
            var result = new Dictionary<string, string>();

            foreach (var property in properties
                .Where(property => value.isSet(property)))
            {
                var propertyValue = value.get(property).ToString();
                result[property.ToString()] = propertyValue ?? "null";
            }

            return result;
        }
    }
}