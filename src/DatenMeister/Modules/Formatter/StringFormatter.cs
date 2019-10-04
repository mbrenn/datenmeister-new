using System;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Formatter
{
    /// <summary>
    /// Supports formatting functions that can be used to format objects into a
    /// string format
    /// </summary>
    public class StringFormatter
    {
        /// <summary>
        /// Takes the value and looks for all properties in double braced bracked {{ and }}
        /// </summary>
        /// <param name="value">Value to be used</param>
        /// <param name="text">Text to be parsed</param>
        /// <returns>The formatted value</returns>
        public string Format(IObject value, string text)
        {
            var builder = new StringBuilder();

            var currentPosition = 0;
            while (currentPosition < text.Length)
            {
                var posBracketOpen = text.IndexOf("{{", currentPosition, StringComparison.Ordinal);
                if (posBracketOpen == -1)
                {
                    builder.Append(text.Substring(currentPosition));
                    currentPosition = text.Length;
                }
                else
                {
                    var posBracketClosed = text.IndexOf("}}", posBracketOpen, StringComparison.Ordinal);
                    if (posBracketClosed == -1)
                    {
                        builder.Append(text.Substring(currentPosition));
                        currentPosition = text.Length;

                    }
                    else
                    {
                        builder.Append(text.Substring(
                            currentPosition,
                            posBracketOpen - currentPosition));

                        var innerVariable = text.Substring(
                            posBracketOpen + 2,
                            posBracketClosed - posBracketOpen - 2);
                        var textValue = value.getOrDefault<string>(innerVariable);
                        builder.Append(textValue);
                        currentPosition = posBracketClosed + 2;
                    }
                }
            }

            return builder.ToString();
        }
    }
}