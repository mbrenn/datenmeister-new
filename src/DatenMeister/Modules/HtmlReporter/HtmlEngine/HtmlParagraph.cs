using System.Net;

namespace DatenMeister.Modules.HtmlReporter.HtmlEngine
{
    public class HtmlParagraph
    {
        /// <summary>
        /// Defines the css class being used for that paragraph
        /// </summary>
        public  string CssClass { get; set; }

        /// <summary>
        /// Gets or sets the paragraph to be stored 
        /// </summary>
        public object Paragraph { get; }        
        
        public HtmlParagraph(object paragraph)
        {
            Paragraph = paragraph;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(CssClass))
            {
                return $"<p>{Paragraph}</p>";
            }
            else
            {
                return $"<p class=\"{WebUtility.HtmlEncode(CssClass)}\">{Paragraph}</p>";
            }
        }
    }
}