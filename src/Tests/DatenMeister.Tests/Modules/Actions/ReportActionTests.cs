using System.IO;
using DatenMeister.Actions;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Tests.Modules.Reports;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class ReportActionTests
    {
        public static (WorkspaceLogic workspaceLogic,
            ScopeStorage scopeStorage,
            MofUriExtent extent,
            MofFactory factory,
            IElement reportInstance,
            ActionLogic actionLogic) CreateReportInstance(IElement reportInstanceMetaClass)
        {
            var (scopeStorage, workspaceLogic) = HtmlReportTests.PrepareWorkspaceLogic();

            var inMemoryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(inMemoryProvider, "dm:///test", scopeStorage);
            workspaceLogic.GetDataWorkspace().AddExtent(extent);

            /* Creates the working object */
            var factory = new MofFactory(extent);
            var element = factory.create(null);
            (element as ICanSetId)!.Id = "TheOne";
            element.set("name", "Brenn");
            element.set("prename", "Martin");
            element.set("age", 19);
            extent.elements().add(element);

            /* Creates the report definition */
            var reportDefinition = factory.create(_DatenMeister.TheOne.Reports.__ReportDefinition);
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.name, "Report Definition");
            extent.elements().add(reportDefinition);

            /* Create the report paragraph and its corresponding view node */
            var reportParagraph = factory.create(_DatenMeister.TheOne.Reports.Elements.__ReportParagraph);
            reportParagraph.set(_DatenMeister._Reports._Elements._ReportParagraph.evalProperties,
                "if (i.age>18)\r\n v.paragraph=\"over18\"\r\n else\r\n v.paragraph=\"under18\"\r\n end");

            var dynamicViewNode = factory.create(_DatenMeister.TheOne.DataViews.__DynamicSourceNode);
            dynamicViewNode.set(_DatenMeister._DataViews._DynamicSourceNode.name, "input");
            extent.elements().add(dynamicViewNode);
            reportParagraph.set(_DatenMeister._Reports._Elements._ReportParagraph.viewNode, dynamicViewNode);

            /* Attached it to the report definition */
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.elements, new[] {reportParagraph});

            /* Creates the report instance */
            var reportInstance = factory.create(reportInstanceMetaClass);
            extent.elements().add(reportInstance);
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.name, "Report");

            var source = factory.create(_DatenMeister.TheOne.Reports.__ReportInstanceSource);
            source.set(_DatenMeister._Reports._ReportInstanceSource.name, "input");
            source.set(_DatenMeister._Reports._ReportInstanceSource.path, "dm:///test#TheOne");
            source.set(_DatenMeister._Reports._ReportInstanceSource.workspaceId, "Data");
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.sources, new[] {source});
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.reportDefinition, reportDefinition);

            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);
            scopeStorage.Add(ActionLogicState.GetDefaultLogicState());

            return (workspaceLogic, scopeStorage, extent, factory, reportInstance, actionLogic);
        }

        [Test]
        public void TestHtmlReport()
        {
            var (_, _, _, factory, reportInstance, actionLogic) =
                CreateReportInstance(_DatenMeister.TheOne.Reports.__HtmlReportInstance);

            var tempFileName = Path.GetTempFileName();

            var action = factory.create(_DatenMeister.TheOne.Actions.Reports.__HtmlReportAction);

            action.set(_DatenMeister._Actions._Reports._HtmlReportAction.filePath, tempFileName);
            action.set(_DatenMeister._Actions._Reports._HtmlReportAction.reportInstance, reportInstance);

            actionLogic.ExecuteAction(action).Wait();

            var fileContent = File.ReadAllText(tempFileName);

            Assert.That(fileContent.Contains("over18"), Is.True);
        }

        [Test]
        public void TestAdocReport()
        {
            var (_, _, _, factory, reportInstance, actionLogic) =
                CreateReportInstance(_DatenMeister.TheOne.Reports.__AdocReportInstance);

            var tempFileName = Path.GetTempFileName();

            var action = factory.create(_DatenMeister.TheOne.Actions.Reports.__AdocReportAction);


            action.set(_DatenMeister._Actions._Reports._HtmlReportAction.filePath, tempFileName);
            action.set(_DatenMeister._Actions._Reports._HtmlReportAction.reportInstance, reportInstance);

            actionLogic.ExecuteAction(action).Wait();

            var fileContent = File.ReadAllText(tempFileName);

            Assert.That(fileContent.Contains("over18"), Is.True);
        }
    }
}