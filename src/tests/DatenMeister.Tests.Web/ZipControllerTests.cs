using System;
using System.Linq;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.WebServer.Controller;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class ZipControllerTests
    {
        [Test]
        public void TestZipControllerCreation()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();

            var zipController = new ZipController(dm.WorkspaceLogic, dm.ScopeStorage);

            var extentsData = dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)
                              ?? throw new InvalidOperationException("No management workspace found");
            Assert.That(extentsData.extent.Count(), Is.EqualTo(0));

            zipController.CreateZipExample(new ZipController.CreateZipExampleParam
            {
                Workspace = WorkspaceNames.WorkspaceData
            });
            
            extentsData = dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)
                          ?? throw new InvalidOperationException("No management workspace found");
            Assert.That(extentsData.extent.Count(), Is.EqualTo(1));
        }
    }
}