using System.Collections.Generic;

namespace DatenMeister.Models.Reports
{
    public class ReportInstance
    {
        public string? name { get; set; }

        public ReportDefinition? reportDefinition { get; set; }

        public List<ReportInstanceSource>? sources { get; set; }
    }
}