using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Json
{
    /// <summary>
    /// Converts an object or an element directly to a JSON element
    /// </summary>
    public class DirectJsonConverter
    {
        public string ConvertToJson(IObject value)
        {
            var builder = new StringBuilder();

            ConvertToJson(builder, value, string.Empty);

            return builder.ToString();
        }

        /// <summary>
        /// Converts the given value to a Json object by using the string builder
        /// </summary>
        /// <param name="builder">The string builder to be used</param>
        /// <param name="value">Value to be converted</param>
        /// <param name="indentation">The Indentation</param>
        private void ConvertToJson(StringBuilder builder, IObject value, string indentation)
        {
            var allProperties = value as IObjectAllProperties;
            if (allProperties == null)
            {
                throw new ArgumentException("value is not of type IObjectAllProperties.");
            }

            builder.Append($"{indentation}{{");

            var newIndentation = indentation + "    ";
            var komma = string.Empty;

            foreach (var property in allProperties.getPropertiesBeingSet())
            {
                builder.AppendLine(komma);
                builder.Append($"{newIndentation}{property}: ");
                var propertyValue = value.get(property);
                ConvertValue(builder, propertyValue, newIndentation);

                komma = ",";
            }

            builder.AppendLine();
            builder.AppendLine($"{indentation}}}");
        }

        private void ConvertValue(StringBuilder builder, object propertyValue, string newIndentation)
        {
            if (DotNetHelper.IsNull(propertyValue))
            {
                builder.Append("null");
            }
            else if (DotNetHelper.IsOfPrimitiveType(propertyValue))
            {
                builder.Append($"\"{propertyValue}\"");
            }
            else if (DotNetHelper.IsOfMofObject(propertyValue))
            {
                ConvertToJson(builder, propertyValue as IObject, newIndentation);
            }
            else if (DotNetHelper.IsOfEnumeration(propertyValue))
            {
                var enumeration = propertyValue as IEnumerable;
                Debug.Assert(enumeration != null, "enumeration != null");

                builder.Append("[");
                var komma = string.Empty;
                foreach (var innerValue in enumeration)
                {
                    builder.AppendLine(komma);
                    ConvertValue(builder, innerValue, "    " + newIndentation);
                    komma = ",";
                }

                builder.Append($"{newIndentation}]");
            }
        }
    }
}