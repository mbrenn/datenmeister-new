using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DatenMeister.Actions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class StoreExtentTests
    {
        [Test]
        public async Task StoreExtent()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            
            var actionLogic = new ActionLogic(dm.WorkspaceLogic, dm.ScopeStorage);
            var extentManager = new ExtentManager(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);
            
            var temporaryStorage = DatenMeisterTests.GetPathForTemporaryStorage("test.xmi");
            
            // Performs the first loading of the extent
            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath, temporaryStorage);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(
                _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId, 
                WorkspaceNames.WorkspaceData);
            var loadedInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(
                loadedInfo.LoadingState,
                Is.EqualTo(ExtentLoadingState.Loaded),
                loadedInfo.FailLoadingMessage);
            
            // First Store
            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__StoreExtentAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._StoreExtentAction.workspaceId] = WorkspaceNames.WorkspaceData,
                    [_DatenMeister._Actions._StoreExtentAction.extentUri] = "dm:///test"
                });
            
            actionLogic.ExecuteAction(action).Wait();
            
            // Checks that there is no XYZ
            Assert.That(
                File.ReadAllText(temporaryStorage).Contains("XYZ"),
                Is.False);

            // Creates an item with XYZ
            var factory = new MofFactory(loadedInfo.Extent!);
            var item = factory.create(null);
            item.set("name", "XYZ");

            loadedInfo.Extent!.elements().add(item);
            
            // Second store with the item
            actionLogic.ExecuteAction(action).Wait();

            // Checks, that the item is now stored
            Assert.That(
                File.ReadAllText(temporaryStorage).Contains("XYZ"),
                Is.True);
        }
    }
}