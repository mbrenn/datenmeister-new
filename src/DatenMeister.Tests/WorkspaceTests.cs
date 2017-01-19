using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
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

            var extent = new UriExtent(new InMemoryProvider(), "http://test/");
            var factory = new MofFactory(extent);
            var element = factory.create(null);
            extent.elements().add(element);

            workspace.AddExtent(extent);

            var uri = extent.uri(element);

            // Now check, if everything is working
            var found = extent.element(uri);
            Assert.That(found, Is.EqualTo(element));

            var anotherFound = workspace.FindElementByUri(uri);
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