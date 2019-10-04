namespace DatenMeister.Modules.HtmlReporter.HtmlEngine
{
    public class HtmlRawString : HtmlElement
    {
        /// <summary>
        /// Gets the content of the string being used as a value for the html
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets or sets a flag that spaces are converted to nbsp
        /// </summary>
        public bool ConvertSpaceToNBSP { get; set; }

        public HtmlRawString(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the content of the class as a string that can be directly
        /// injected into the html file.
        /// </summary>
        /// <returns>The Html snippet honoring the settings within the
        /// file</returns>
        public string GetHtmlString()
        {
            if (ConvertSpaceToNBSP)
            {
                return Value.Replace(" ", "&nbsp;");
            }

            return Value;
        }

        public override string ToString()
        {
            return GetHtmlString();
        }
    }
}