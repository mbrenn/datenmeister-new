using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Models;
using DatenMeister.Modules.Reports;
using DatenMeister.Modules.Reports.Html;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Reports
{
    [TestFixture]
    public class HtmlReportLoopTests
    {
        [Test]
        public void TestLoops()
        {
            var (scopeStorage, workspaceLogic) = HtmlReportTests.PrepareWorkspaceLogic();

            var inMemoryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(inMemoryProvider, "dm:///test");
            workspaceLogic.GetDataWorkspace().AddExtent(extent);

            /* Creates the working object */
            var factory = new MofFactory(extent);
            HtmlReportTableTests.AddData(extent, factory);

            /* Creates the report definition */
            var reportDefinition = factory.create(_DatenMeister.TheOne.Reports.__ReportDefinition);
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.name, "Report Definition");
            extent.elements().add(reportDefinition);

            /* Sets the viewnode */
            var dynamicViewNode = factory.create(_DatenMeister.TheOne.DataViews.__DynamicSourceNode);
            dynamicViewNode.set(_DatenMeister._DataViews._DynamicSourceNode.name, "input");
            extent.elements().add(dynamicViewNode);

            /* Create the report paragraph and its corresponding view node */
            var reportLoop = factory.create(_DatenMeister.TheOne.Reports.Elements.__ReportLoop);

            var reportParagraph = factory.create(_DatenMeister.TheOne.Reports.Elements.__ReportParagraph);
            reportParagraph.set(
                _DatenMeister._Reports._Elements._ReportParagraph.evalProperties,
                "if (i.age>18)\r\n v.paragraph=\"over18\"\r\n else\r\n v.paragraph=\"under18\"\r\n end");

            var reportParagraph2 = factory.create(_DatenMeister.TheOne.Reports.Elements.__ReportParagraph);
            reportParagraph2.set(
                _DatenMeister._Reports._Elements._ReportParagraph.evalParagraph,
                "Name: {{i.name}}");

            reportLoop.set(_DatenMeister._Reports._Elements._ReportLoop.elements, new[] {reportParagraph, reportParagraph2});
            reportLoop.set(_DatenMeister._Reports._Elements._ReportLoop.viewNode, new[] {dynamicViewNode});

            /* Attached it to the report definition */
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.elements, new[] {reportLoop});

            /* Creates the report instance */
            var reportInstance = factory.create(_DatenMeister.TheOne.Reports.__HtmlReportInstance);
            extent.elements().add(reportInstance);
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.name, "Report");

            var source = factory.create(_DatenMeister.TheOne.Reports.__ReportInstanceSource);
            source.set(_DatenMeister._Reports._ReportInstanceSource.name, "input");
            source.set(_DatenMeister._Reports._ReportInstanceSource.source, "dm:///test");
            source.set(_DatenMeister._Reports._ReportInstanceSource.workspaceId, "Data");
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.sources, new[] {source});
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.reportDefinition, reportDefinition);

            /* Now create the report over 18 */
            var writer = new StringWriter();
            var htmlReport = new HtmlReportCreator(writer);
            var htmlReportLogic = new ReportLogic(workspaceLogic, scopeStorage, htmlReport);
            htmlReportLogic.GenerateReportByInstance(reportInstance);

            var reportText = writer.ToString();
            
            Assert.That(reportText.Contains("over18"), Is.True, reportText);
            Assert.That(reportText.Contains("under18"), Is.True, reportText);
            Assert.That(reportText.Contains("Mother"), Is.True, reportText);
            Assert.That(reportText.Contains("Father"), Is.True, reportText);
            Assert.That(reportText.Contains("Child1"), Is.True, reportText);
        }

    }
}