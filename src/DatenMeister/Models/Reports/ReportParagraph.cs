using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Reports
{
    public class ReportParagraph : ReportElement
    {
        public string? paragraph { get; set; }
        
        /// <summary>
        /// Gets or sets the cssClass
        /// </summary>
        public string? cssClass { get; set; }
        
        /// <summary>
        /// Gets or sets the viewnode being used to evaluate the paragraph
        /// </summary>
        public IElement? viewNode { get; set; }
        
        /// <summary>
        /// Gets or sets the formatting information being used for the paragraph
        /// </summary>
        public string evalProperties { get; set; } = string.Empty;
    }
}