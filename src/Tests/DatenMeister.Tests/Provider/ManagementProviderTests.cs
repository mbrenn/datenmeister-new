using System.Linq;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;
using DatenMeister.Provider.ExtentManagement;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
{
    [TestFixture]
    public class ManagementProviderTests
    {
        [Test]
        public void TestSettingOfUri()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();
            var uriExtent = scope.WorkspaceLogic.GetManagementWorkspace()
                .FindExtent(WorkspaceNames.UriExtentWorkspaces);

            var userExtent = uriExtent?.element("#Management_dm%3A%2F%2F%2F__internal%2Fforms%2Fuser");
            Assert.That(userExtent, Is.Not.Null);

            userExtent!.set(_DatenMeister._Management._Extent.uri, "dm:///newusers");

            Assert.That(userExtent.get(_DatenMeister._Management._Extent.uri), Is.EqualTo("dm:///newusers"));

            var newUsers = scope.WorkspaceLogic.GetManagementWorkspace().extent
                .FirstOrDefault(x => (x as IUriExtent)?.contextURI() == "dm:///newusers");

            Assert.That(
                newUsers,
                Is.Not.Null);

            var extentManager = scope.Resolve<ExtentManager>();
            var newUserConfiguration = extentManager.GetLoadConfigurationFor((newUsers as IUriExtent)!);
            Assert.That(newUserConfiguration, Is.Not.Null);
            Assert.That(
                newUserConfiguration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig
                    .extentUri), Is.EqualTo("dm:///newusers"));
        }

        [Test]
        public void TestRemovalOfExtentViaProvider()
        {
            var (_, workspaceLogic, _, _, extent) = GetInitializedWorkspace();

            var firstWorkspace = extent.elements().OfType<IElement>().FirstOrDefault();
            Assert.That(firstWorkspace, Is.Not.Null);
            Assert.That(firstWorkspace.getOrDefault<string>(_DatenMeister._Management._Workspace.id),
                Is.EqualTo("Data"));

            var extents =
                firstWorkspace.getOrDefault<IReflectiveCollection>(_DatenMeister._Management._Workspace.extents);
            Assert.That(extents.Count(), Is.EqualTo(1));
            var foundExtent = extents.ElementAt(0) as IElement;
            Assert.That(foundExtent, Is.Not.Null);
            Assert.That(foundExtent.get<string>(_DatenMeister._Management._Extent.uri), Is.EqualTo("dm:///test"));

            var workspace = workspaceLogic.GetWorkspace("Data");
            Assert.That(workspace, Is.Not.Null);
            Assert.That(workspace!.extent.OfType<IUriExtent>().Any(x => x.contextURI() == "dm:///test"), Is.True);

            // Now remove the extent via the object helper and check the deletion
            ObjectHelper.DeleteObject(foundExtent);

            // And check that it is deleted
            workspace = workspaceLogic.GetWorkspace("Data");
            Assert.That(workspace, Is.Not.Null);
            Assert.That(workspace!.extent.OfType<IUriExtent>().Any(x => x.contextURI() == "dm:///test"), Is.False);

            extents =
                firstWorkspace.getOrDefault<IReflectiveCollection>(_DatenMeister._Management._Workspace.extents);
            Assert.That(extents.Count(), Is.EqualTo(0));
        }

        private static (ScopeStorage scopeStorage, WorkspaceLogic workspaceLogic,
            ExtentStorageData.LoadedExtentInformation loadedExtent, ExtentOfWorkspaceProvider provider, MofUriExtent
            managementExtent) GetInitializedWorkspace()
        {
            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(WorkspaceLogic.InitDefault());
            var workspaceLogic = new WorkspaceLogic(scopeStorage);

            var mapper = new ProviderToProviderLoaderMapper();
            scopeStorage.Add(mapper);
            mapper.AddMapping(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig,
                manager => new InMemoryProviderLoader());

            var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
            var loadConfig = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loadConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.name, "dm:///test");
            loadConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loadConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId, "Data");
            var loadedExtent = extentManager.LoadExtent(loadConfig, ExtentCreationFlags.LoadOrCreate);
            Assert.That(loadedExtent.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));

            var provider = new ExtentOfWorkspaceProvider(workspaceLogic, scopeStorage);
            var extent = new MofUriExtent(provider, "dm:///management", null);
            return (scopeStorage, workspaceLogic, loadedExtent, provider, extent);
        }

        [Test]
        public void TestPropertiesViaProviderAccess()
        {
            var (scopeStorage, workspaceLogic, loadedExtent, provider, extent) = GetInitializedWorkspace();
            loadedExtent.Extent!.set("name", "Brenn");

            var firstWorkspace = extent.elements().OfType<IElement>().FirstOrDefault();
            Assert.That(firstWorkspace, Is.Not.Null);
            Assert.That(firstWorkspace.getOrDefault<string>(_DatenMeister._Management._Workspace.id),
                Is.EqualTo("Data"));

            var extents =
                firstWorkspace.getOrDefault<IReflectiveCollection>(_DatenMeister._Management._Workspace.extents);
            var foundExtent = extents.ElementAt(0) as IElement;
            var properties = foundExtent.getOrDefault<IElement>(_DatenMeister._Management._Extent.properties);
            Assert.That(properties, Is.Not.Null);
            Assert.That(properties.getOrDefault<string>("name"), Is.EqualTo("Brenn"));
            Assert.That(properties.getOrDefault<string>(MofUriExtent.UriPropertyName), Is.EqualTo("dm:///test"));
        }

        [Test]
        public void TestUrlsOfWorkspaceAndExtentAndProperties()
        {
            var (scopeStorage, workspaceLogic, loadedExtent, provider, extent) = GetInitializedWorkspace();
            loadedExtent.Extent!.set("name", "Brenn");
            var plugin = new ManagementProviderPlugin(workspaceLogic, scopeStorage);
            plugin.Start(PluginLoadingPosition.AfterInitialization);

            var data = workspaceLogic.FindItem(
                ExtentManagementUrlHelper.GetUrlOfWorkspace(
                    workspaceLogic.GetDataWorkspace()));
            Assert.That(data, Is.Not.Null);
            Assert.That(data.getOrDefault<string>(_DatenMeister._Management._Workspace.id), Is.EqualTo("Data"));

            var extentData = workspaceLogic.FindItem(
                ExtentManagementUrlHelper.GetUrlOfExtent(
                    workspaceLogic.GetDataWorkspace(), loadedExtent.Extent));
            Assert.That(extentData, Is.Not.Null);
            Assert.That(extentData.getOrDefault<string>(_DatenMeister._Management._Extent.uri),
                Is.EqualTo("dm:///test"));

            var extentProperties = workspaceLogic.FindItem(
                ExtentManagementUrlHelper.GetUrlOfExtentsProperties(
                    workspaceLogic.GetDataWorkspace(), loadedExtent.Extent));
            Assert.That(extentProperties, Is.Not.Null);
            Assert.That(extentProperties.getOrDefault<string>("name"), Is.EqualTo("Brenn"));
        }
    }
}