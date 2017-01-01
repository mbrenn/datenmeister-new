﻿using DatenMeister.Core;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml;
using NUnit.Framework;

namespace DatenMeister.Tests.Core
{
    [TestFixture]
    public class DataLayerTests
    {
        [Test]
        public void TestDataLayers()
        {
            var data = WorkspaceLogic.InitDefault();
            var logic = new WorkspaceLogic(data);

            var dataExtent = new UriExtent(new InMemoryProvider(), "Data");
            var typeExtent = new UriExtent(new InMemoryProvider(), "Types");
            var umlExtent = new UriExtent(new InMemoryProvider(), "Uml");
            var unAssignedExtent = new UriExtent(new InMemoryProvider(), "Unassigned");

            data.Data.AddExtent(dataExtent);
            data.Types.AddExtent(typeExtent);
            data.Uml.AddExtent(umlExtent);

            Assert.That(logic.GetWorkspaceOfExtent(dataExtent), Is.EqualTo(data.Data));
            Assert.That(logic.GetWorkspaceOfExtent(typeExtent), Is.EqualTo(data.Types));
            Assert.That(logic.GetWorkspaceOfExtent(umlExtent), Is.EqualTo(data.Uml));
            Assert.That(logic.GetWorkspaceOfExtent(unAssignedExtent), Is.EqualTo(data.Data));
            Assert.That(data.Data.MetaWorkspace, Is.EqualTo(data.Types));
        }

        [Test]
        public void TestDataLayersForItem()
        {
            var data = WorkspaceLogic.InitDefault();
            var logic = new WorkspaceLogic(data);

            var dataExtent = new UriExtent(new InMemoryProvider(), "Data");
            var umlExtent = new UriExtent(new InMemoryProvider(), "Uml");

            data.Data.AddExtent(dataExtent);
            data.Uml.AddExtent(umlExtent);

            var value = new InMemoryElement();
            var logicLayer = logic.GetWorkspaceOfObject(value);
            Assert.That(logicLayer, Is.SameAs(data.Data)); // Per Default, only the Data

            umlExtent.elements().add(value);
            logicLayer = logic.GetWorkspaceOfObject(value);
            Assert.That(logicLayer, Is.SameAs(data.Uml));
        }

        [Test]
        public void TestClassTreeUsage()
        {
            var data = WorkspaceLogic.InitDefault();
            var dataLayerLogic = new WorkspaceLogic(data);

            var strapper = Bootstrapper.PerformFullBootstrap(
                dataLayerLogic,
                data.Uml,
                BootstrapMode.Mof);

            var primitiveTypes = data.Uml.Create<FillThePrimitiveTypes, _PrimitiveTypes>();
            Assert.That(primitiveTypes, Is.Not.Null );
            Assert.That(primitiveTypes.__Real, Is.Not.Null);
            Assert.That(primitiveTypes.__Real, Is.Not.TypeOf<object>());
            
            var primitiveTypes2 = data.Uml.Create<FillThePrimitiveTypes, _PrimitiveTypes>();
            Assert.That(primitiveTypes2, Is.SameAs(primitiveTypes));
        }
    }
}