using System.Collections.Generic;

namespace DatenMeister.Models.Reports
{
    public class ReportInstance
    {
        public string name { get; set; } = string.Empty;

        public List<ReportInstanceSource>? sources { get; set; }
    }
}