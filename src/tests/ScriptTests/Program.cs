﻿using DatenMeister.Models.Reports.Simple;

namespace ScriptTests
{
    class Program
    {
        static void Main()
        {
            var configuration = new SimpleReportConfiguration
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