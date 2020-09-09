using System.Net;

namespace DatenMeister.Modules.HtmlExporter.HtmlEngine
{
    /// <summary>
    /// Defines a small helper class to be sure that only well-defined types avoiding
    /// html-injection are used within the HtmlEngine context.
    /// Any inherited class must be able to handle html injection.
    /// </summary>
    public class HtmlElement
    {
        /// <summary>
        /// Initializes a new instance of the HtmlElement class
        /// </summary>
        protected HtmlElement()
        {
        }

        /// <summary>
        /// Defines the implicit operator to convert a string to an HtmlElement class.
        /// Here, the string is taken and converted to an HtmlRawString containing the escaped variant of the string.
        /// </summary>
        /// <param name="value">String value to be used here</param>
        /// <returns>The HtmlRawString</returns>
        public static implicit operator HtmlElement(string value)
        {
            if (value == null)
            {
                return new HtmlRawString("");
            }
            
            return new HtmlRawString(WebUtility.HtmlEncode(value).Replace("\r\n", "<br />"));
        }
    }
}