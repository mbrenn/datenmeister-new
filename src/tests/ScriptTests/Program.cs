using DatenMeister.Models;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using static DatenMeister.Models._DatenMeister._Reports;

namespace ScriptTests
{
    class Program
    {
        static void Main()
        {
            var configuration =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Reports.__SimpleReportConfiguration)
                    .SetProperty(_SimpleReportConfiguration.typeMode, ___ReportTableForTypeMode.PerType)
                    .SetProperty(_SimpleReportConfiguration.descendentMode, ___DescendentMode.Inline)
                    .SetProperty(_SimpleReportConfiguration.showDescendents, true)
                    .SetProperty(_SimpleReportConfiguration.showMetaClasses, true)
                    .SetProperty(_SimpleReportConfiguration.showFullName, true)
                    .SetProperty(_SimpleReportConfiguration.showRootElement, true);

            ReportTests.TestReportIssues(configuration);
            
            ReportTests.TestReportZipCode(configuration);

            configuration.set(_SimpleReportConfiguration.typeMode, ___ReportTableForTypeMode.AllTypes);

            ReportTests.TestReportIssues(configuration);
        }
    }
}