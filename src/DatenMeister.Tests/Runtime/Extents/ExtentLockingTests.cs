﻿using System.IO;
using System.Reflection;
using Autofac;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Locking;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Extents
{
    [TestFixture]
    public class ExtentLockingTests
    {
        [Test]
        public void TestSuccessfulLocking()
        {
            var settings = DatenMeisterTests.GetIntegrationSettings();
            settings.IsLockingActivated = true;
            
            var settings2 = DatenMeisterTests.GetIntegrationSettings();
            settings2.IsLockingActivated = true;
            settings2.DatabasePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "testing/datenmeister/data2");
            

            using (var dm = DatenMeisterTests.GetDatenMeisterScope(true, settings))
            {
                using (var dm2 = DatenMeisterTests.GetDatenMeisterScope(true, settings2))
                {
                    var extentSettings = new XmiStorageLoaderConfig
                    {
                        extentUri = "dm:///test",
                        filePath = Path.Combine(
                            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            "testing/datenmeister/x.xmi"),
                        workspaceId = "Data"
                    };
                    
                    var extentManager = dm.Resolve<ExtentManager>();
                    extentManager.UnlockProvider(extentSettings);

                    var provider1 = extentManager.LoadExtent(extentSettings, ExtentCreationFlags.LoadOrCreate);
                    Assert.That(provider1, Is.Not.Null);
                    
                    var extentManager2 = dm2.Resolve<ExtentManager>();
                    Assert.Throws<IsLockedException>(() =>
                    {
                        extentManager2.LoadExtent(extentSettings, ExtentCreationFlags.LoadOrCreate);
                    });
                    
                    extentManager.UnloadManager(true);
                    
                    var provider2= extentManager2.LoadExtent(extentSettings, ExtentCreationFlags.LoadOrCreate);
                    Assert.That(provider2, Is.Not.Null);

                    extentManager2.UnloadManager(true);
                }
            }
        }
    }
}