﻿using System;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.AttachedExtent;
using DatenMeister.Modules.AttachedExtent;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class AttachedExtentTests
    {
        [Test]
        public void TestRetrievalOfConfiguration()
        {
            // Prepare some stuff
            var workspaceLogic = new WorkspaceLogic(new WorkspaceData());
            var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test");
            var factory = new MofFactory(extent);
            var type = factory.create(_UML.TheOne.StructuredClassifiers.__Class);
            extent.elements().add(type);

            // Create the attachedconfiguration
            var attachedConfiguration = new AttachedExtentConfiguration
            {
                name = "Test",
                referencedExtent = "dm:///other",
                referencedWorkspace = "Data",
                referenceProperty = "referenceid",
                referenceType = type
            };

            extent.set(
                AttachedExtentHandler.AttachedExtentProperty, 
                DotNetConverter.ConvertToMofObject(extent, attachedConfiguration));
            
            // Now test the attached handler
            var attachedHandler = new AttachedExtentHandler(workspaceLogic);
            var foundConfiguration = attachedHandler.GetConfiguration(extent);
            Assert.That(foundConfiguration, Is.Not.Null);
            Assert.That(foundConfiguration.name, Is.EqualTo("Test"));
            Assert.That(foundConfiguration.referencedExtent, Is.EqualTo("dm:///other"));
            Assert.That(foundConfiguration.referencedWorkspace, Is.EqualTo("Data"));
            Assert.That(foundConfiguration.referenceProperty, Is.EqualTo("referenceid"));
            Assert.That(foundConfiguration.referenceType, Is.EqualTo(type));
        }
        
        [Test]
        public void TestSetupCreation()
        {
            var setup = CreateTestSetup();
            Assert.That(setup, Is.Not.Null);
            Assert.That(setup.WorkspaceLogic, Is.Not.Null);
            Assert.That(setup.AttachedExtentHandler, Is.Not.Null);
            Assert.That(setup.OriginalExtent, Is.Not.Null);
            Assert.That(setup.AttachedExtent, Is.Not.Null);
            Assert.That(setup.AttachedExtent2, Is.Not.Null);
        }

        [Test] 
        public void TestGetOriginalExtent()
        {
            var setup = CreateTestSetup();
            var originalExtent = setup.AttachedExtentHandler.GetOriginalExtent(setup.AttachedExtent);
            Assert.That(originalExtent, Is.Not.Null);
            Assert.That(originalExtent.contextURI(), Is.EqualTo(setup.OriginalExtent.contextURI()));
        }


        [Test]
        public void TestGetAttachedExtent()
        {
            var setup = CreateTestSetup();
            var attachedExtents =
                setup.AttachedExtentHandler.FindAttachedExtents(setup.OriginalExtent)
                    .ToList();

            Assert.That(attachedExtents, Is.Not.Null);
            Assert.That(attachedExtents.Count, Is.EqualTo(2));
            Assert.That(attachedExtents.Any(x => x.contextURI() == setup.AttachedExtent.contextURI()));
            Assert.That(attachedExtents.Any(x => x.contextURI() == setup.AttachedExtent2.contextURI()));
        }

        [Test]
        public void TestAttachedItems()
        {
            var setup = CreateTestSetup();
            var originalItem1 = MofFactory.Create(setup.OriginalExtent, null);
            var originalItem2 = MofFactory.Create(setup.OriginalExtent, null);

            setup.OriginalExtent.elements().add(originalItem1);
            setup.OriginalExtent.elements().add(originalItem2);

            var attached1 = setup.AttachedExtentHandler.GetOrCreateAttachedItem(originalItem1, setup.AttachedExtent);
            var attached2 = setup.AttachedExtentHandler.GetOrCreateAttachedItem(originalItem2, setup.AttachedExtent);

            Assert.That(attached1, Is.Not.Null);
            Assert.That(attached2, Is.Not.Null);
            Assert.That(attached1.metaclass, Is.EqualTo(setup.ReferenceType));
            
            attached1.set("name", "Attached 1");
            attached2.set("name", "Attached 2");
            
            var attached3 = setup.AttachedExtentHandler.GetOrCreateAttachedItem(originalItem1, setup.AttachedExtent);
            Assert.That(attached3, Is.Not.Null);
            Assert.That(attached3.getOrDefault<string>("name"), Is.EqualTo("Attached 1"));
        }
        
        public class TestSetup
        {
            public AttachedExtentHandler AttachedExtentHandler { get; set; }
            
            public AttachedExtentConfiguration AttachedExtentConfiguration { get; set; }
            
            public IWorkspaceLogic WorkspaceLogic { get; set; }
            
            public IUriExtent TypeExtent { get; set; }
            
            public IElement ReferenceType { get; set; }
            
            public IUriExtent OriginalExtent { get; set; }
            
            public IUriExtent AttachedExtent { get; set; }
            
            public IUriExtent AttachedExtent2 { get; set; }
            
            public IUriExtent NonConnectedExtent { get; set; }
        }

        public TestSetup CreateTestSetup()
        {
            var testSetup = new TestSetup
            {
                WorkspaceLogic = new WorkspaceLogic(new WorkspaceData()),
                TypeExtent = new MofUriExtent(new InMemoryProvider(), "dm:///types"),
                OriginalExtent = new MofUriExtent(new InMemoryProvider(), "dm:///originalExtent"),
                AttachedExtent = new MofUriExtent(new InMemoryProvider(), "dm:///attachedExtent"),
                AttachedExtent2 = new MofUriExtent(new InMemoryProvider(), "dm:///attachedExtent2"),
                NonConnectedExtent = new MofUriExtent(new InMemoryProvider(), "dm:///nonconnectedExtent")
            };

            var type = MofFactory.Create(testSetup.TypeExtent, null);
            testSetup.TypeExtent.elements().add(type);
            type.set("name", "ReferenceType");
            testSetup.ReferenceType = type;
            
            var attachedHandler =
                testSetup.AttachedExtentHandler = new AttachedExtentHandler(testSetup.WorkspaceLogic);
                
            var dataWorkspace = new Workspace("Data");
            testSetup.WorkspaceLogic.AddWorkspace(dataWorkspace);
            testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.OriginalExtent);
            testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.AttachedExtent);
            testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.AttachedExtent2);
            testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.NonConnectedExtent);

            var config = testSetup.AttachedExtentConfiguration = new AttachedExtentConfiguration
            {
                referencedExtent = "dm:///originalExtent",
                referencedWorkspace = "Data",
                referenceProperty = "reference",
                referenceType = type
            };

            attachedHandler.SetConfiguration(testSetup.AttachedExtent, config);
            attachedHandler.SetConfiguration(testSetup.AttachedExtent2, config);
            return testSetup;
        }
    }
}