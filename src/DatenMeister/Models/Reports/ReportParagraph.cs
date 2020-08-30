using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Reports
{
    public class ReportParagraph : ReportElement
    {
        public string? paragraph { get; set; }

        /// <summary>
        /// Gets or sets the evaluation of Paragraphs
        /// </summary>
        public string evalParagraph { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the viewnode being used to evaluate the paragraph
        /// </summary>
        public IElement? viewNode { get; set; }
    }
}