using System.Collections.Generic;

namespace DatenMeister.Models.Reports
{
    public class ReportDefinition
    {
        public string? name { get; set; }
        
        public string? title { get; set; }

        public List<ReportElement>? elements { get; set; }
    }
}