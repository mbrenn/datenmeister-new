using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests
{
    [TestFixture]
    public class WorkspaceTests
    {
        [Test]
        public void TestFindByUri()
        {
            var workspaceLogic = WorkspaceLogic.Create(new WorkspaceData());
            var workspace = new Workspace("data", "No annotation");
            workspaceLogic.AddWorkspace(workspace);

            var extent = new MofUriExtent(new InMemoryProvider(), "http://test/");
            var factory = new MofFactory(extent);
            var element = factory.create(null);
            extent.elements().add(element);

            workspaceLogic.AddExtent(workspace, extent);

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