﻿using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime;
using DatenMeister.Extent.Manager.ExtentStorage;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class ItemsControllerTests
    {
        [Test]
        public void TestParentItems()
        {
            var dm = DatenMeisterTests.GetDatenMeisterScope();
            var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);

            var createdExtent = extentManager.CreateAndAddXmiExtent("dm:///temp", "./test.xmi");
            createdExtent.Extent!.set("name", "Test Extent");

            var factory = new MofFactory(createdExtent.Extent);
            var item1 = factory.create(null).SetProperty("name", "item1").SetId("1");
            var item2 = factory.create(null).SetProperty("name", "item2").SetId("2");
            var item3 = factory.create(null).SetProperty("name", "item3").SetId("3");
            var item4 = factory.create(null).SetProperty("name", "item4").SetId("4");
            var item5 = factory.create(null).SetProperty("name", "item5").SetId("5");
            var item6 = factory.create(null).SetProperty("name", "item6").SetId("6");
            var item7 = factory.create(null).SetProperty("name", "item7").SetId("7");

            var propertyName = DefaultClassifierHints.GetDefaultPackagePropertyName(item1);
            
            item1.set(propertyName, item3);
            item2.set(propertyName, new[] { item4, item5 });
            createdExtent.Extent.elements().add(item1);
            createdExtent.Extent.elements().add(item2);
            
            item4.set(propertyName, new[] { item6, item7 });
            
            
        }
        
    }
}