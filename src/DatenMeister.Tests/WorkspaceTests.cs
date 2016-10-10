using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Workspaces;
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

            var extent = new InMemoryUriExtent("http://test/");
            var factory = new InMemoryFactory();
            var element = factory.create(null);
            extent.elements().add(element);

            workspace.AddExtent(extent);

            var elementAsMofElement = (InMemoryElement) element;
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