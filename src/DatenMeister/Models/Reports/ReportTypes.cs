using System;
using System.Collections.Generic;

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
                typeof(HtmlReportInstance),
                
                typeof(Simple.DescendentMode),
                typeof(Simple.ReportTableForTypeMode),
                typeof(Simple.SimpleReportConfiguration),
                
            };
        }
    }
}