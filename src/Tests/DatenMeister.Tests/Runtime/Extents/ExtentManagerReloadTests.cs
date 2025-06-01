using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Extents;

[TestFixture]
internal class ExtentManagerReloadTests
{
    [Test]
    public async Task TestReferenceXmi()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var extentManager = dm.Resolve<ExtentManager>();

        // Clean
        await extentManager.RemoveExtent(WorkspaceNames.WorkspaceData, "dm:///test");

        // Load
        var loaderConfig =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__XmlReferenceLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._XmlReferenceLoaderConfig.extentUri, "dm:///test");
        loaderConfig.set(_ExtentLoaderConfigs._XmlReferenceLoaderConfig.workspaceId,
            WorkspaceNames.WorkspaceData);
        loaderConfig.set(_ExtentLoaderConfigs._XmlReferenceLoaderConfig.filePath,
            Path.Combine(Environment.CurrentDirectory, "Examples\\xmi-temp-trx.xml"));

        File.Copy("Examples/xmi1.xml", "Examples\\xmi-temp-trx.xml", true);


        var loadedInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.LoadOrCreate);

        // Check
        Assert.That(loadedInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
        Assert.That(loadedInfo.Extent, Is.Not.Null);

        var first = loadedInfo.Extent!.elements().OfType<IElement>().FirstOrDefault();
        Assert.That(first, Is.Not.Null);
        Assert.That(first.getOrDefault<string>("name"), Is.EqualTo("M"));

        // Reload
        File.Copy("Examples/xmi2.xml", "Examples\\xmi-temp-trx.xml", true);
        await extentManager.ReloadExtent(loadedInfo.Extent);

        var first2 = loadedInfo.Extent!.elements().OfType<IElement>().FirstOrDefault();

        Assert.That(first2, Is.Not.Null);
        Assert.That(first2.getOrDefault<string>("name"), Is.EqualTo("Ma"));
    }
}