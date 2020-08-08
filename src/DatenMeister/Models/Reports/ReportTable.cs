using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Reports
{
    public class ReportTable : ReportElement
    {
        public IElement? viewNode { get; set; }

        public IElement? form { get; set; }
    }
}