﻿using DatenMeister.Core;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using NUnit.Framework;

namespace DatenMeister.Tests
{
    [TestFixture]
    public class WorkspaceTests
    {
        [Test]
        public void TestFindByUri()
        {
            var workspace = new Workspace("data", "No annotation");

            var extent = new MofUriExtent("http://test/");
            var factory = new MofFactory();
            var element = factory.create(null);
            extent.elements().add(element);

            workspace.AddExtent(extent);

            var elementAsMofElement = (MofElement) element;
            var guid = elementAsMofElement.Id;

            // Now check, if everything is working
            var found = extent.element("http://test/#" + guid);
            Assert.That(found, Is.EqualTo(element));

            var anotherFound = workspace.FindElementByUri("http://test/#" + guid);
            Assert.That(anotherFound, Is.EqualTo(element));
        }

        [Test]
        public void TestWorkspaceConfiguration()
        {
            var workspace = new Workspace("data", "No annotation");
            Assert.That(workspace.id, Is.EqualTo("data"));
            Assert.That(workspace.annotation, Is.EqualTo("No annotation"));
        }
    }
}