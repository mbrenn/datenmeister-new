using System.Net;
using BurnSystems;

namespace DatenMeister.HtmlEngine
{
    /// <summary>
    /// Defines a small helper class to be sure that only well-defined types avoiding
    /// html-injection are used within the HtmlEngine context.
    /// Any inherited class must be able to handle html injection.
    /// </summary>
    public class HtmlElement
    {
        /// <summary>
        /// Gets or sets the css class for the table
        /// </summary>
        public string CssClass { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        protected string AttributeString =>
            (string.IsNullOrEmpty(CssClass) ? string.Empty : $" class=\"{WebUtility.HtmlEncode(CssClass)}\"") +
            (string.IsNullOrEmpty(Id) ? string.Empty : $" id=\"{WebUtility.HtmlEncode(Id)}\"");

        /// <summary>
        /// Initializes a new instance of the HtmlElement class
        /// </summary>
        protected HtmlElement()
        {
        }

        /// <summary>
        /// Gets a random id for a new string
        /// </summary>
        /// <returns>The random id</returns>
        public static string GetRandomId() =>
            StringManipulation.RandomString(16);

        /// <summary>
        /// Defines the implicit operator to convert a string to an HtmlElement class.
        /// Here, the string is taken and converted to an HtmlRawString containing the escaped variant of the string.
        /// </summary>
        /// <param name="value">String value to be used here</param>
        /// <returns>The HtmlRawString</returns>
        public static implicit operator HtmlElement(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new HtmlRawString("");
            }
            
            return new HtmlRawString(WebUtility.HtmlEncode(value).Replace("\r\n", "<br />"));
        }
    }
}