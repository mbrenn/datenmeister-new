﻿using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Extents
{
    [TestFixture]
    public class ExtentLockingTests
    {
        [Test]
        public async Task TestSuccessfulLocking()
        {
            var settings = DatenMeisterTests.GetIntegrationSettings();
            settings.IsLockingActivated = true;

            var settings2 = DatenMeisterTests.GetIntegrationSettings();
            settings2.IsLockingActivated = true;
            settings2.DatabasePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "testing/datenmeister/data2");

            await using (var dm = await DatenMeisterTests.GetDatenMeisterScope(true, settings))
            {
                await using (var dm2 = await DatenMeisterTests.GetDatenMeisterScope(true, settings2))
                {
                    try
                    {
                        var extentSettings =
                            InMemoryObject.CreateEmpty(
                                _DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
                        extentSettings.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri,
                            "dm:///test");
                        extentSettings.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath,
                            Path.Combine(
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                                "testing/datenmeister/x.xmi"));
                        extentSettings.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId,
                            "Data");

                        var extentManager = dm.Resolve<ExtentManager>();
                        extentManager.UnlockProvider(extentSettings);

                        var provider1 = await extentManager.LoadExtent(extentSettings, ExtentCreationFlags.LoadOrCreate);
                        Assert.That(provider1, Is.Not.Null);
                        Assert.That(provider1.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
                        Assert.That(provider1.Extent, Is.Not.Null);

                        var extentManager2 = dm2.Resolve<ExtentManager>();
                        var information = await extentManager2.LoadExtent(extentSettings, ExtentCreationFlags.LoadOrCreate);
                        Assert.That(information.LoadingState, Is.EqualTo(ExtentLoadingState.Failed));
                        await extentManager2.RemoveExtent(information);

                        await extentManager.UnloadManager(true);

                        var provider2 = await extentManager2.LoadExtent(extentSettings, ExtentCreationFlags.LoadOrCreate);
                        Assert.That(provider2, Is.Not.Null);
                        Assert.That(provider2.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
                        Assert.That(provider2.Extent, Is.Not.Null);

                        await extentManager2.UnloadManager(true);
                    }
                    catch (Exception exc)
                    {
                        Assert.Fail($"An exception flew: {exc}");
                    }
                }
            }
        }
    }
}