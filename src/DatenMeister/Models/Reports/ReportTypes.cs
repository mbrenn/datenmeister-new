using System;
using System.Collections.Generic;
using DatenMeister.Models.Reports.Adoc;
using DatenMeister.Models.Reports.Html;

namespace DatenMeister.Models.Reports
{
    public class ReportTypes
    {
        public static IEnumerable<Type> GetTypes()
        {
            return new[]
            {
                typeof(ReportDefinition),
                typeof(ReportElement),
                typeof(ReportHeadline),
                typeof(ReportParagraph),
                typeof(ReportTable),
                typeof(ReportInstanceSource),
                typeof(ReportInstance),
                typeof(AdocReportInstance),
                typeof(HtmlReportInstance),
                
                typeof(Simple.DescendentMode),
                typeof(Simple.ReportTableForTypeMode),
                typeof(Simple.SimpleReportConfiguration),
                
            };
        }
    }
}