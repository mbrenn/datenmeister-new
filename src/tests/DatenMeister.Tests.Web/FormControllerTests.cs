#nullable enable
using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms;
using DatenMeister.Modules.Json;
using DatenMeister.WebServer.Controller;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class FormControllerTests
    {
        [Test]
        public void TestDefaultForExtent()
        {
            var (zipExtent, formsController) = CreateZipExtent();
            var foundForm = formsController.GetDefaultFormForExtent(
                WorkspaceNames.WorkspaceData,
                zipExtent.contextURI(),
                ViewModes.Default);
            Assert.That(foundForm.Value.IndexOf("tab") != -1);
        }
        
        
        [Test]
        public void TestDefaultForItem()
        {
            var (zipExtent, formsController) = CreateZipExtent();
            var firstElement = zipExtent.elements().First() as IElement;
            var foundForm = formsController.GetDefaultFormForItem(
                WorkspaceNames.WorkspaceData,
                firstElement.GetUri(),
                ViewModes.Default);
            Assert.That(foundForm.Value.IndexOf("tab") != -1);
        }

        /// <summary>
        /// Creates the zipExtent and also the FormsController
        /// </summary>
        /// <returns>Tuple of zipExtent and corresponding Forms Controller</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static (IUriExtent? zipExtent, FormsController formsController) CreateZipExtent()
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

            var zipExtent = dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!
                .extent
                .OfType<IUriExtent>()
                .FirstOrDefault();

            var formsController = new FormsController(dm.WorkspaceLogic, dm.ScopeStorage);
            return (zipExtent, formsController);
        }
    }
}