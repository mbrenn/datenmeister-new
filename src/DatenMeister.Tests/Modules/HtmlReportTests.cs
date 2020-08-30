using System.IO;
using System.Text;
using BurnSystems.Collections;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.DataViews;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Modules.Reports;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class HtmlReportTests
    {
        [Test]
        public void TestCssLoading()
        {
            var builder = new StringBuilder();
            var memory = new StringWriter(builder);
            var reporter= new HtmlReport(memory);
            
            reporter.SetDefaultCssStyle();
            Assert.That(reporter.CssStyleSheet, Is.Not.Empty);
            Assert.That(reporter.CssStyleSheet, Is.Not.Null);
        }

        [Test]
        public void TestHtmlParagraphWithDataSet()
        {
            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(WorkspaceLogic.InitDefault());
            var workspaceLogic = new WorkspaceLogic(scopeStorage);
            
            var inMemoryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(inMemoryProvider, "dm:///test");
            workspaceLogic.GetDataWorkspace().AddExtent(extent);
            
            /* Creates the working object */
            var factory = new MofFactory(extent);
            var element = factory.create(null);
            (element as ICanSetId)!.Id = "TheOne";
            element.set("name", "Brenn");
            element.set("prename", "Martin");
            extent.elements().add(element);

            /* Creates the report definition */
            var reportDefinition = factory.create(_Reports.TheOne.__ReportDefinition);
            reportDefinition.set(_Reports._ReportDefinition.name, "Report Definition");
            extent.elements().add(reportDefinition);

            /* Create the report paragraph and its corresponding view node */
            var reportParagraph = factory.create(_Reports.TheOne.__ReportParagraph);
            reportParagraph.set(_Reports._ReportParagraph.evalParagraph, "Hello {{i.name}} {{i.prename}}");

            var dynamicViewNode = factory.create(_DataViews.TheOne.__DynamicSourceNode);
            dynamicViewNode.set(_DataViews._DynamicSourceNode.name, "input");
            extent.elements().add(dynamicViewNode);
            reportParagraph.set(_Reports._ReportParagraph.viewNode, dynamicViewNode);
            
            /* Attached it to the report definition */
            reportDefinition.set(_Reports._ReportDefinition.elements, new[]{reportParagraph});
            
            /* Creates the report instance */
            var reportInstance = factory.create(_Reports.TheOne.__HtmlReportInstance);
            extent.elements().add(reportInstance);
            reportInstance.set(_Reports._HtmlReportInstance.name, "Report");

            var source = factory.create(_Reports.TheOne.__ReportInstanceSource);
            source.set(_Reports._ReportInstanceSource.source, "dm:///test#TheOne");
            source.set(_Reports._ReportInstanceSource.workspaceId,"Data");
            reportInstance.set(_Reports._HtmlReportInstance.sources, new[] {source});
            reportInstance.set(_Reports._HtmlReportInstance.reportDefinition, reportDefinition);
            
            /* Now create the report */
            var writer = new StringWriter();
            var htmlReport = new HtmlReportCreator(workspaceLogic, scopeStorage);
            htmlReport.GenerateReportByInstance(reportInstance, writer);

            Assert.That(writer.ToString().Contains("Hallo Brenn Martin"), Is.True, writer.ToString());
        }
    }
}