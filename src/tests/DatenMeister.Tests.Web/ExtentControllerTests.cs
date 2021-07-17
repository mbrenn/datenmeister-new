using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.WebServer.Controller;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class ExtentControllerTests
    {
        [Test]
        public void TestAddAndDeleteExtent()
        {
            var dm = DatenMeisterTests.GetDatenMeisterScope();

            var extentController = new ExtentController(dm.WorkspaceLogic, dm.ScopeStorage);
            
            Assert.That(dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!.extent.Count(),
                Is.EqualTo(0));
            extentController.CreateXmi(
                WorkspaceNames.WorkspaceData,
                new ExtentController.CreateXmiExtentParams
                {
                    ExtentUri = "dm:///test",
                    FilePath = "./test.xmi"
                });
            
            Assert.That(dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!.extent.Count(),
                Is.EqualTo(1));
            Assert.That(
                dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!.extent.OfType<IUriExtent>()
                    .Any(x => x.contextURI() == "dm:///test"),
                Is.True);

            extentController.DeleteExtent(new ExtentController.DeleteExtentParams
            {
                Workspace = WorkspaceNames.WorkspaceData,
                ExtentUri = "dm:///test"
            });
            
            Assert.That(dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!.extent.Count(),
                Is.EqualTo(0));
        }
    }
}