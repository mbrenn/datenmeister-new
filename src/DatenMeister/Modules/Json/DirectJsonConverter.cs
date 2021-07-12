using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Modules.Json
{
    /// <summary>
    /// Converts an object or an element directly to a JSON element
    /// </summary>
    public class DirectJsonConverter
    {
        /// <summary>
        /// Defines the maximum recursion depth being allowed to converte the elements
        /// </summary>
        public int MaxRecursionDepth { get; set; } = 5;

        /// <summary>
        /// Converts the given element to a json object
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted value</returns>
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
        /// <param name="recursionDepth">Defines the resursion depth to be handled</param>
        private void ConvertToJson(StringBuilder builder, IObject value, string indentation, int recursionDepth = 0)
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
                builder.Append($"{newIndentation}\"{property}\": ");
                var propertyValue = value.get(property);
                
                ConvertValue(builder, propertyValue, newIndentation);

                komma = ",";
            }

            builder.AppendLine();
            builder.AppendLine($"{indentation}}}");
        }

        /// <summary>
        /// Converts the value
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="propertyValue"></param>
        /// <param name="newIndentation"></param>
        /// <param name="recursionDepth">Defines the recursion Depth</param>
        private void ConvertValue(StringBuilder builder, object? propertyValue, string newIndentation, int recursionDepth = 0)
        {
            if (recursionDepth >= MaxRecursionDepth)
            {
                builder.AppendLine($"[{NamedElementMethods.GetName(propertyValue)}]");
                return;
            }

            if (DotNetHelper.IsNull(propertyValue) || propertyValue == null)
            {
                builder.Append("null");
            }
            else if (DotNetHelper.IsOfPrimitiveType(propertyValue))
            {
                builder.Append($"\"{propertyValue}\"");
            }
            else if (DotNetHelper.IsOfMofObject(propertyValue))
            {
                ConvertToJson(builder, (propertyValue as IObject)!, newIndentation, recursionDepth + 1);
            }
            else if (DotNetHelper.IsOfEnumeration(propertyValue)
                     && propertyValue is IEnumerable enumeration)
            {
                Debug.Assert(enumeration != null, "enumeration != null");

                builder.Append("[");
                var komma = string.Empty;
                foreach (var innerValue in enumeration!)
                {
                    builder.AppendLine(komma);
                    ConvertValue(builder, innerValue, "    " + newIndentation, recursionDepth + 1);
                    komma = ",";
                }

                builder.Append($"{newIndentation}]");
            }
            else
            {
                builder.Append($"\"{propertyValue!}\"");
            }
        }
    }
}