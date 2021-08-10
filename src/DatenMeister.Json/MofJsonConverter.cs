using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Web;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Json
{
    /// <summary>
    /// Converts an object or a mof element to a json element in which
    /// the metaclass and other meta information is available for the json interface.
    ///
    /// The corresponding JavaScript function Mof.ts::createObjectFromJson
    /// </summary>
    public class MofJsonConverter
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

            ConvertToJson(builder, value);

            return builder.ToString();
        }

        /// <summary>
        /// Converts the given element to a json object
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted value</returns>
        public static string ConvertToJsonWithDefaultParameter(IObject value)
        {
            return new MofJsonConverter().ConvertToJson(value);
        }

        /// <summary>
        /// Converts the given value to a Json object by using the string builder
        /// </summary>
        /// <param name="builder">The string builder to be used</param>
        /// <param name="value">Value to be converted</param>
        /// <param name="recursionDepth">Defines the recursion depth to be handled</param>
        private void ConvertToJson(StringBuilder builder, IObject value, int recursionDepth = 0)
        {
            if (value is MofObjectShadow asShadow)
            {
                // Element is of type MofObjectShadow and can just be referenced
                builder.Append($"{{\"r\": \"{HttpUtility.JavaScriptStringEncode(asShadow.Uri)}\"}}");
                return;
            }
            
            if (value is not IObjectAllProperties allProperties)
            {
                throw new ArgumentException("value is not of type IObjectAllProperties.");
            }

            builder.Append('{');

            // Creates the values
            var komma = string.Empty;
            
            builder.Append("\"v\": {");
            foreach (var property in allProperties.getPropertiesBeingSet())
            {
                builder.AppendLine(komma);
                builder.Append($"\"{HttpUtility.JavaScriptStringEncode(property)}\": ");
                var propertyValue = value.get(property);
                
                AppendValue(builder, propertyValue);

                komma = ",";
            }

            builder.Append("}");

            // Creates the metaclass
            if (value is IElement asElement)
            {
                var item = ItemWithNameAndId.Create(asElement.getMetaClass());
                if (item != null)
                {
                    builder.Append(", \"m\": {");
                    
                    builder.Append("\"name\": ");
                    AppendValue(builder, item.name, 0);
                    builder.Append(", \"id\": ");
                    AppendValue(builder, item.id, 0);
                    builder.Append(", \"extentUri\": ");
                    AppendValue(builder, item.extentUri, 0);
                    builder.Append(", \"fullName\": ");
                    AppendValue(builder, item.fullName, 0);

                    builder.Append("}");
                }
            }
            
            // Creates the uri
            var uri = value.GetUri();
            if (uri != null)
            {
                builder.Append(", \"u\": ");
                AppendValue(builder, uri);
            }

            builder.Append("}");
            
            builder.AppendLine();
        }

        /// <summary>
        /// Converts the value
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="propertyValue"></param>
        /// <param name="recursionDepth">Defines the recursion Depth</param>
        private void AppendValue(StringBuilder builder, object? propertyValue, int recursionDepth = 0)
        {
            if (recursionDepth >= MaxRecursionDepth)
            {
                builder.AppendLine($"[{HttpUtility.JavaScriptStringEncode(NamedElementMethods.GetName(propertyValue))}]");
                return;
            }

            if (DotNetHelper.IsNull(propertyValue) || propertyValue == null)
            {
                builder.Append("null");
            }
            else if (DotNetHelper.IsOfPrimitiveType(propertyValue))
            {
                builder.Append($"\"{HttpUtility.JavaScriptStringEncode(propertyValue!.ToString())}\"");
            }
            else if (DotNetHelper.IsOfMofObject(propertyValue))
            {
                ConvertToJson(builder, (propertyValue as IObject)!, recursionDepth + 1);
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
                    AppendValue(builder, innerValue, recursionDepth + 1);
                    komma = ",";
                }

                builder.Append("]");
            }
            else
            {
                builder.Append($"\"{propertyValue!}\"");
            }
        }
    }
}