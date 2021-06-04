namespace DatenMeister.HtmlEngine
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
        public bool ConvertSpaceToNbsp { get; set; }

        /// <summary>
        /// Gets or sets a flag that spaces are converted to nbsp
        /// </summary>
        public bool ConvertNewLineToBr { get; set; } = true;

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
            var value = Value ?? string.Empty;
            if (ConvertSpaceToNbsp)
            {
                value = value.Replace(" ", "&nbsp;");
            }

            if (ConvertNewLineToBr)
            {
                value = value.Replace("\n", "<br />");
            }

            return value;
        }

        public override string ToString()
            => GetHtmlString();
    }
}