using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Models.AttachedExtent;
using DatenMeister.Modules.AttachedExtent;
using DatenMeister.Provider.InMemory;
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
    }
}