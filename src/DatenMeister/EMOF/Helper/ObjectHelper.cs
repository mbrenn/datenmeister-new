using DatenMeister.EMOF.Interface.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.EMOF.Helper
{
    /// <summary>
    /// Includes several methods to support the handling of objects
    /// </summary>
    public static class ObjectHelper
    {
        public static Dictionary<object, object> AsDictionary(
            this IObject value, 
            IEnumerable<object> properties)
        {
            var result = new Dictionary<object, object>();

            foreach (var property in properties)
            {
                if (value.isSet(property))
                {
                    result[property] = value.get(property);
                }
            }

            return result;
        }

        public static Dictionary<string, string> AsStringDictionary(
            this IObject value,
            IEnumerable<object> properties)
        {
            var result = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                if (value.isSet(property))
                {
                    result[property.ToString()] = value.get(property).ToString();
                }
            }

            return result;
        }
    }
}
