﻿using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using static DatenMeister.Core.Models._DatenMeister._Reports;

namespace ScriptTests
{
    class Program
    {
        static void Main()
        {
            var configuration =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Reports.__SimpleReportConfiguration)
                    .SetProperty(_SimpleReportConfiguration.typeMode, _Elements.___ReportTableForTypeMode.PerType)
                    .SetProperty(_SimpleReportConfiguration.descendentMode, ___DescendentMode.Inline)
                    .SetProperty(_SimpleReportConfiguration.showDescendents, true)
                    .SetProperty(_SimpleReportConfiguration.showMetaClasses, true)
                    .SetProperty(_SimpleReportConfiguration.showFullName, true)
                    .SetProperty(_SimpleReportConfiguration.showRootElement, true);

            ReportTests.TestReportIssues(configuration);
            
            ReportTests.TestReportZipCode(configuration);

            configuration.set(_SimpleReportConfiguration.typeMode, _Elements.___ReportTableForTypeMode.AllTypes);

            ReportTests.TestReportIssues(configuration);
        }
    }
}