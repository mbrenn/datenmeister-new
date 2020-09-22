using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Reports
{
    public class ReportTable : ReportElement
    {
        public string? cssClass { get; set; }
        
        public IElement? viewNode { get; set; }

        public IElement? form { get; set; }

        public string? evalProperties { get; set; }
    }
}