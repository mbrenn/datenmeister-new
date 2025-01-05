using Autofac;
using DatenMeister.Actions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Forms.MassImport;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Provider.ExtentManagement;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace DatenMeister.Tests.Modules.Extent.Forms
{
    [TestFixture]
    internal class MassImportTests
    {
        private const string ImportText = "id,name,prename,age\r\n1,Brenn,Martin,42\r\n2,Ulrich,Megator,50\r\n3,,Ramontisch,12\r\n4,,,30\r\n5,Swift";

        public struct MassImportSetup
        {
            public IUriExtent targetExtent; // Defines the InMemoryExtent supporting easy execution of the test

            public IDatenMeisterScope dm; // The scope itself
        }

        public static async Task<MassImportSetup> Init()
        {
            var dm = await DatenMeisterTests.GetDatenMeisterScope();

            // Create the Target Extent

            var extentManager = dm.Resolve<ExtentManager>();

            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
                WorkspaceNames.WorkspaceData);

            var loadedExtentInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(loadedExtentInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));

            return new MassImportSetup
            {
                targetExtent = loadedExtentInfo.Extent!,
                dm = dm
            };
        }

        public void PrepareDemoData(MassImportSetup setup)
        {
            var factory = MofFactory.CreateByExtent(setup.targetExtent);

            var item2 = factory.create(null);
            var item5 = factory.create(null);
            var item3 = factory.create(null);

            item2.set("id", 2);
            item2.set("name", "Herbert");
            item2.set("prename", "Der Große");
            item2.set("age", 12);   
            item5.set("id", 5);
            item5.set("prename", "Taylor");
            item5.set("age", 28);
            item5.set("location", "USA");
            item3.set("id", 3);
            item3.set("name", "Rambo");
            item3.set("prename", "Ramon");
            item3.set("age", 23);
            item3.set("location", "Hessen");

            setup.targetExtent.elements().add(item2);
            setup.targetExtent.elements().add(item5);
            setup.targetExtent.elements().add(item3);
        }

        [Test]
        public async Task TestMassImportDirectly()
        {
            var start = await Init();
            PrepareDemoData(start);

            var importText = ImportText;

            var massImportLogic = new MassImportLogic(start.dm.WorkspaceLogic, start.dm.ScopeStorage);
            massImportLogic.PerformMassImport(start.targetExtent, importText);
            CheckImportedTargetExtent(start);

        }

        [Test]
        public async Task TestMassImportViaAction()
        {
            var start = await Init();
            PrepareDemoData(start);

            var action = InMemoryObject.CreateEmpty(DatenMeister.Extent.Forms.Model._Root.TheOne.__MassImportDefinitionAction);
            action.set(DatenMeister.Extent.Forms.Model._Root._MassImportDefinitionAction.text, ImportText);

            var managementExtent = ExtentManagementHelper.GetExtentForWorkspaces(start.dm.WorkspaceLogic);
            var testExtentItem = 
                ExtentManagementHelper.GetExtentElement(
                    managementExtent, 
                    start.targetExtent.GetWorkspace()!.id, 
                    start.targetExtent.GetUri()!);
            action.set(DatenMeister.Extent.Forms.Model._Root._MassImportDefinitionAction.item, testExtentItem);

            var actionHandler = start.dm.Resolve<ActionLogic>();
            await actionHandler.ExecuteAction(action);

            CheckImportedTargetExtent(start);
        }

        private static void CheckImportedTargetExtent(MassImportSetup start)
        {
            var items = start.targetExtent.elements().OfType<IElement>().ToList();

            Assert.That(items.Count, Is.EqualTo(5));

            var item1 = items.FirstOrDefault(x => x.getOrDefault<int>("id") == 1);
            var item2 = items.FirstOrDefault(x => x.getOrDefault<int>("id") == 2);
            var item3 = items.FirstOrDefault(x => x.getOrDefault<int>("id") == 3);
            var item4 = items.FirstOrDefault(x => x.getOrDefault<int>("id") == 4);
            var item5 = items.FirstOrDefault(x => x.getOrDefault<int>("id") == 5);

            Assert.That(item1, Is.Not.Null);
            Assert.That(item2, Is.Not.Null);
            Assert.That(item3, Is.Not.Null);
            Assert.That(item4, Is.Not.Null);
            Assert.That(item5, Is.Not.Null);

            Assert.That(item1.getOrDefault<string>("name"), Is.EqualTo("Brenn"));
            Assert.That(item1.getOrDefault<string>("prename"), Is.EqualTo("Martin"));
            Assert.That(item1.getOrDefault<int>("age"), Is.EqualTo(42));
            Assert.That(item1.getOrDefault<string>("location"), Is.Null);

            Assert.That(item2.getOrDefault<string>("name"), Is.EqualTo("Ulrich"));
            Assert.That(item2.getOrDefault<string>("prename"), Is.EqualTo("Megator"));
            Assert.That(item2.getOrDefault<int>("age"), Is.EqualTo(50));
            Assert.That(item2.getOrDefault<string>("location"), Is.Null);

            Assert.That(item3.getOrDefault<string>("name"), Is.EqualTo("Rambo"));
            Assert.That(item3.getOrDefault<string>("prename"), Is.EqualTo("Ramontisch"));
            Assert.That(item3.getOrDefault<int>("age"), Is.EqualTo(12));
            Assert.That(item3.getOrDefault<string>("location"), Is.EqualTo("Hessen"));

            Assert.That(item4.getOrDefault<string>("name"), Is.Null);
            Assert.That(item4.getOrDefault<string>("prename"), Is.Null);
            Assert.That(item4.getOrDefault<int>("age"), Is.EqualTo(30));
            Assert.That(item4.getOrDefault<string>("location"), Is.Null);

            Assert.That(item5.getOrDefault<string>("prename"), Is.EqualTo("Taylor"));
            Assert.That(item5.getOrDefault<string>("name"), Is.EqualTo("Swift"));
            Assert.That(item5.getOrDefault<int>("age"), Is.EqualTo(28));
            Assert.That(item5.getOrDefault<string>("location"), Is.EqualTo("USA"));
        }
    }
}
