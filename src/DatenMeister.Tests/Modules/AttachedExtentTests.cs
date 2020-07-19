using System;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.AttachedExtent;
using DatenMeister.Modules.AttachedExtent;
using DatenMeister.Provider.InMemory;
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
        
        public class TestSetup
        {
            public AttachedExtentHandler AttachedExtentHandler { get; set; }
            public IWorkspaceLogic WorkspaceLogic { get; set; }
            
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
                OriginalExtent = new MofUriExtent(new InMemoryProvider(), "dm:///originalExtent"),
                AttachedExtent = new MofUriExtent(new InMemoryProvider(), "dm:///attachedExtent"),
                AttachedExtent2 = new MofUriExtent(new InMemoryProvider(), "dm:///attachedExtent2"),
                NonConnectedExtent = new MofUriExtent(new InMemoryProvider(), "dm:///nonconnectedExtent")
            };
            
            var attachedHandler =
                testSetup.AttachedExtentHandler = new AttachedExtentHandler(testSetup.WorkspaceLogic);
                
            var dataWorkspace = new Workspace("Data");
            testSetup.WorkspaceLogic.AddWorkspace(dataWorkspace);
            testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.OriginalExtent);
            testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.AttachedExtent);
            testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.AttachedExtent2);
            testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.NonConnectedExtent);
            
            var config = new AttachedExtentConfiguration();
            config.referencedExtent = "dm:///originalExtent";
            config.referencedWorkspace = "Data";
            config.referenceProperty = "reference";
            
            attachedHandler.SetConfiguration(testSetup.AttachedExtent, config);
            attachedHandler.SetConfiguration(testSetup.AttachedExtent2, config);
            return testSetup;
        }
    }
}