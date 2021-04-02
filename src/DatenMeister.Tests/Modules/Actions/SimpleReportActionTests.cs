using System;
using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Modules.Actions;
using DatenMeister.Tests.Modules.Reports;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class SimpleReportActionTests
    {
        [Test]
        public void TestCreateSimpleReportByDirectRootElement()
        {
            var (actionLogic, extent, factory) = GetActionLogic();

            /*
             * Performs the test
             */
            var action = factory.create(_DatenMeister.TheOne.Actions.Reports.__SimpleReportAction);
            var configuration = factory.create(_DatenMeister.TheOne.Reports.__SimpleReportConfiguration);
            var tempFileName = Path.GetTempFileName();
            
            configuration.set(_DatenMeister._Reports._SimpleReportConfiguration.rootElement, extent.contextURI());
            action.set(_DatenMeister._Actions._Reports._SimpleReportAction.configuration, configuration);
            action.set(
                _DatenMeister._Actions._Reports._SimpleReportAction.filePath,
                tempFileName);

            actionLogic.ExecuteAction(action).Wait();

            Assert.That(File.Exists(tempFileName), Is.True);
            var content = File.ReadAllText(tempFileName);
            Assert.That(content.Contains("name"));
            
            // Delete the file
            File.Delete(tempFileName);
        }

        [Test]
        public void TestCreateSimpleReportByIndirectRootElement()
        {
            var (actionLogic, extent, factory) = GetActionLogic();

            /*
             * Performs the test
             */
            var action = factory.create(_DatenMeister.TheOne.Actions.Reports.__SimpleReportAction);
            var configuration = factory.create(_DatenMeister.TheOne.Reports.__SimpleReportConfiguration);
            var tempFileName = Path.GetTempFileName();
            
            action.set(_DatenMeister._Actions._Reports._SimpleReportAction.path, "dm:///test");
            action.set(_DatenMeister._Actions._Reports._SimpleReportAction.workspaceId, "Data");
            action.set(_DatenMeister._Actions._Reports._SimpleReportAction.configuration, configuration);
            action.set(
                _DatenMeister._Actions._Reports._SimpleReportAction.filePath,
                tempFileName);

            actionLogic.ExecuteAction(action).Wait();

            Assert.That(File.Exists(tempFileName), Is.True);
            var content = File.ReadAllText(tempFileName);
            Assert.That(content.Contains("name"));
            
            // Delete the file
            File.Delete(tempFileName);
        }

        private static (ActionLogic actionLogic, MofUriExtent extent, MofFactory factory) GetActionLogic()
        {
            /*
             * Prepares the data
             */
            var (scopeStorage, workspaceLogic) = HtmlReportTests.PrepareWorkspaceLogic();
            scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
            scopeStorage.Add(WorkspaceLogic.InitDefault());
            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);

            var inMemoryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(inMemoryProvider, "dm:///test");
            workspaceLogic.GetDataWorkspace().AddExtent(extent);

            /* Creates the working object */
            var factory = new MofFactory(extent);
            var element = factory.create(null);
            (element as ICanSetId)!.Id = "TheOne";
            element.set("name", "Brenn");
            element.set("prename", "Martin");
            element.set("age", 19);
            extent.elements().add(element);
            return (actionLogic, extent, factory);
        }
    }
}