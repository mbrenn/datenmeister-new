using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Runtime.Workspaces.Data;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class WorkspaceTests
    {
        [Test]
        public void TestStoreAndLoadEmptyWorkspaces()
        {
            // Stores an empty workspace
            var workSpaceCollection = new WorkspaceCollection();
            var workspaceLoader = new WorkspaceLoader(workSpaceCollection, "data/workspaces.xml");
            workspaceLoader.Store();

            var newWorkSpaceCollection = new WorkspaceCollection();
            workspaceLoader = new WorkspaceLoader(newWorkSpaceCollection, "data/workspaces.xml");
            workspaceLoader.Load();

            Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestStoreAndLoadHavingTwoWorkspaces()
        {
            // Stores an empty workspace
            var workSpaceCollection = new WorkspaceCollection();
            workSpaceCollection.AddWorkspace(new Workspace<IExtent>("test", "Continue"));
            workSpaceCollection.AddWorkspace(new Workspace<IExtent>("another", "annotation"));
            var workspaceLoader = new WorkspaceLoader(workSpaceCollection, "data/workspaces.xml");
            workspaceLoader.Store();

            var newWorkSpaceCollection = new WorkspaceCollection();
            workspaceLoader = new WorkspaceLoader(newWorkSpaceCollection, "data/workspaces.xml");
            workspaceLoader.Load();

            Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(2));
            var first = newWorkSpaceCollection.Workspaces.FirstOrDefault(x => x.id == "test");
            Assert.That(first, Is.Not.Null);
            Assert.That(first.id, Is.EqualTo("test"));
            Assert.That(first.annotation, Is.EqualTo("Continue"));
            first = newWorkSpaceCollection.Workspaces.FirstOrDefault(x => x.id == "another");
            Assert.That(first, Is.Not.Null);
            Assert.That(first.id, Is.EqualTo("another"));
            Assert.That(first.annotation, Is.EqualTo("annotation"));

            Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(2));
        }

        /*[Test]
        public void TestIfMetaWorkspaceNeedsToExist()
        {
            // Stores an empty workspace
            var workSpaceCollection = new WorkspaceCollection();
            workSpaceCollection.AddWorkspace(new Workspace<IExtent>("test", "Continue"), "test");
            workSpaceCollection.AddWorkspace(new Workspace<IExtent>("test2", "Continue"), "test");
            workSpaceCollection.AddWorkspace(new Workspace<IExtent>("test3", "Continue"), "test2");
            Assert.Throws<InvalidOperationException>(() =>
                    {
                        workSpaceCollection.AddWorkspace(new Workspace<IExtent>("test4", "Continue"), "test5");
                    });
        }*/

        [Test]
        public void TestStoreAndLoadHavingTwoWorkspacesWithConflict()
        {
            // Stores an empty workspace
            var workSpaceCollection = new WorkspaceCollection();
            workSpaceCollection.AddWorkspace(new Workspace<IExtent>("test", "Continue"));
            workSpaceCollection.AddWorkspace(new Workspace<IExtent>("another", "annotation"));
            var workspaceLoader = new WorkspaceLoader(workSpaceCollection, "data/workspaces.xml");
            workspaceLoader.Store();

            var newWorkSpaceCollection = new WorkspaceCollection();
            newWorkSpaceCollection.AddWorkspace(new Workspace<IExtent>("test", "Continue"));
            workspaceLoader = new WorkspaceLoader(newWorkSpaceCollection, "data/workspaces.xml");
            workspaceLoader.Load();

            Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(2));
            var first = newWorkSpaceCollection.Workspaces.FirstOrDefault(x => x.id == "test");
            Assert.That(first, Is.Not.Null);
            Assert.That(first.id, Is.EqualTo("test"));
            Assert.That(first.annotation, Is.EqualTo("Continue"));
            first = newWorkSpaceCollection.Workspaces.FirstOrDefault(x => x.id == "another");
            Assert.That(first, Is.Not.Null);
            Assert.That(first.id, Is.EqualTo("another"));
            Assert.That(first.annotation, Is.EqualTo("annotation"));

            Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(2));
        }

    }
}