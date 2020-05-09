using System;
using DatenMeister.Modules.Reports;

namespace ScriptTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new SimpleReportConfiguration()
            {
                typeMode = ReportTableForTypeMode.PerType,
                descendentMode = DescendentMode.Inline,
                showDescendents = true,
                showMetaClasses = true,
                showFullName = true,
                showRootElement = true
            };

            ReportTests.TestReportIssues(configuration);
            
            ReportTests.TestReportZipCode(configuration);

            configuration.typeMode = ReportTableForTypeMode.AllTypes;

            ReportTests.TestReportIssues(configuration);
        }
    }
}