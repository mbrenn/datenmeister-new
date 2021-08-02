﻿using System.Linq;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Runtime.Workspaces.Data;
using DatenMeister.DependencyInjection;
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
            var workSpaceCollection = WorkspaceLogic.GetEmptyLogic();
            var workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
            workspaceLoader.Store();

            var newWorkSpaceCollection = WorkspaceLogic.GetEmptyLogic();
            workspaceLoader = new WorkspaceLoader(
                workSpaceCollection,
                new ScopeStorage().Add(
                    new WorkspaceLoaderConfig
                    (
                        DatenMeisterTests.GetPathForTemporaryStorage("workspaces.xml")
                    )));
            workspaceLoader.Load();

            Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestStoreAndLoadDefaultWorkspaces()
        {
            // Stores an empty workspace
            var workSpaceCollection = WorkspaceLogic.GetDefaultLogic();
            var workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
            var nr = workSpaceCollection.Workspaces.Count();
            workspaceLoader.Store();

            var newWorkSpaceCollection = WorkspaceLogic.GetDefaultLogic();
            workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
            workspaceLoader.Load();

            Assert.That(newWorkSpaceCollection.Workspaces.Count(), Is.EqualTo(nr));
        }

        [Test]
        public void TestStoreAndLoadHavingTwoWorkspaces()
        {
            // Stores an empty workspace
            var workSpaceCollection = WorkspaceLogic.GetEmptyLogic();
            workSpaceCollection.AddWorkspace(new Workspace("test", "Continue"));
            workSpaceCollection.AddWorkspace(new Workspace("another", "annotation"));
            var workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
            workspaceLoader.Store();

            var newWorkSpaceCollection = WorkspaceLogic.GetEmptyLogic();
            workspaceLoader = CreateWorkspaceLoader(newWorkSpaceCollection);
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

        [Test]
        public void TestStoreAndLoadHavingTwoWorkspacesWithConflict()
        {
            // Stores an empty workspace
            var workSpaceCollection = WorkspaceLogic.GetEmptyLogic();
            workSpaceCollection.AddWorkspace(new Workspace("test", "Continue"));
            workSpaceCollection.AddWorkspace(new Workspace("another", "annotation"));
            var workspaceLoader = CreateWorkspaceLoader(workSpaceCollection);
            workspaceLoader.Store();

            var newWorkSpaceCollection = WorkspaceLogic.GetEmptyLogic();
            newWorkSpaceCollection.AddWorkspace(new Workspace("test", "Continue"));
            workspaceLoader = CreateWorkspaceLoader(newWorkSpaceCollection);
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

        /// <summary>
        /// Creates a configured workspaceloader with defined filepath
        /// </summary>
        /// <param name="workSpaceCollection">Workspace Logic to be used</param>
        /// <returns>The created loader</returns>
        private static WorkspaceLoader CreateWorkspaceLoader(IWorkspaceLogic workSpaceCollection)
        {
            var workspaceLoader = new WorkspaceLoader(
                workSpaceCollection,
                new ScopeStorage().Add(
                    new WorkspaceLoaderConfig(DatenMeisterTests.GetPathForTemporaryStorage("workspaces.xml")
                    )));
            return workspaceLoader;
        }

    }
}