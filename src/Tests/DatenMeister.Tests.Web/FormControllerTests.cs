#nullable enable
using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Forms;
using DatenMeister.Forms;
using DatenMeister.Modules.ZipCodeExample.Model;
using DatenMeister.Types;
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
            var (zipExtent, formsController, _) = CreateZipExtent();
            var foundForm = formsController.GetDefaultFormForExtent(
                WorkspaceNames.WorkspaceData,
                zipExtent.contextURI(),
                ViewModes.Default);
            Assert.That(foundForm, Is.Not.Null);
            Assert.That(foundForm.Value.IndexOf("tab", StringComparison.Ordinal) != -1);
        }
        
        
        [Test]
        public void TestDefaultForItem()
        {
            var (zipExtent, formsController, _) = CreateZipExtent();
            var firstElement = zipExtent.elements().First() as IElement;
            Assert.That(firstElement, Is.Not.Null);
            var foundForm = formsController.GetDefaultFormForItem(
                WorkspaceNames.WorkspaceData,
                firstElement!.GetUri()!,
                ViewModes.Default);
            
            Assert.That(foundForm, Is.Not.Null);
            Assert.That(foundForm.Value, Is.Not.Null);
            Assert.That(foundForm!.Value!.IndexOf("tab", StringComparison.Ordinal) != -1);
        }

        [Test]
        public void TestDetailFormForDefaultButtons()
        {
            var (zipExtent, formsController, formsInternal) = CreateZipExtent();
            var firstElement = zipExtent.elements().First() as IElement;
            Assert.That(firstElement, Is.Not.Null);
            var foundForm = formsInternal.GetDefaultFormForItemInternal(
                WorkspaceNames.WorkspaceData,
                firstElement!.GetUri()!,
                ViewModes.Default)!;

            Assert.That(foundForm, Is.Not.Null);
            var detailForm = FormMethods.GetDetailForms(foundForm).FirstOrDefault();
            Assert.That(detailForm, Is.Not.Null);
            var fields = detailForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field);
            var foundFields =
                fields.OfType<IElement>()
                    .Where(
                        x =>
                            x.getOrDefault<string>(_DatenMeister._Forms._ActionFieldData.actionName) ==
                            BasicNavigationForFormsAndItemsPlugin.NavigationItemDelete)
                    .ToList();
            
            Assert.That(foundFields.Any(), Is.True);
            Assert.That(foundFields.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void TestListFormForDefaultButtons()
        {
            var (zipExtent, formsController, formsInternal) = CreateZipExtent();
            var foundForm = formsInternal.GetDefaultFormForExtentInternal(
                WorkspaceNames.WorkspaceData,
                zipExtent.contextURI(),
                ViewModes.Default)!;

            Assert.That(foundForm, Is.Not.Null);
            var listForm = FormMethods.GetListForms(foundForm).FirstOrDefault();
            Assert.That(listForm, Is.Not.Null);
            var fields = listForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field);
            
            var foundFields =
                fields.OfType<IElement>()
                    .Where(
                        x =>
                            x.getOrDefault<string>(_DatenMeister._Forms._ActionFieldData.actionName) ==
                            BasicNavigationForFormsAndItemsPlugin.NavigationExtentsListDeleteItem)
                    .ToList();
            
            Assert.That(foundFields.Any(), Is.True);
            Assert.That(foundFields.Count, Is.EqualTo(1));
            
            foundFields =
                fields.OfType<IElement>()
                    .Where(
                        x =>
                            x.getOrDefault<string>(_DatenMeister._Forms._ActionFieldData.actionName) ==
                            BasicNavigationForFormsAndItemsPlugin.NavigationExtentsListViewItem)
                    .ToList();
            
            Assert.That(foundFields.Any(), Is.True);
            Assert.That(foundFields.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestExtentFormForMetaClass()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var localType = new LocalTypeSupport(dm.WorkspaceLogic, dm.ScopeStorage);
            var zipCode = localType.GetMetaClassFor(typeof(ZipCode))!;
            Assert.That(zipCode, Is.Not.Null);

            var zipCodeMetaUrl = zipCode.GetUri()!;
            Assert.That(zipCodeMetaUrl, Is.Not.Null);

            var controller = new FormsControllerInternal(dm.WorkspaceLogic, dm.ScopeStorage);
            var form = (controller.GetDefaultFormForMetaClassInternal(zipCodeMetaUrl) as IElement)!;
            
            Assert.That(form, Is.Not.Null);
            Assert.That(form.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__ExtentForm), Is.True);
            var detailForm = FormMethods.GetDetailForms(form).FirstOrDefault();
            Assert.That(detailForm, Is.Not.Null);
            var fields = detailForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field);
            Assert.That(fields.Count(), Is.GreaterThan(3));

            var positionLat = fields
                .OfType<IElement>()
                .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) ==
                                     nameof(ZipCode.positionLat));
            Assert.That(positionLat,Is.Not.Null);
        }

        [Test]
        public void TestExtentFormByUri()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();

            var controller = new FormsControllerInternal(dm.WorkspaceLogic, dm.ScopeStorage);
            var form = controller.GetInternal(WorkspaceNames.UriExtentInternalForm + "#DatenMeister.Extent.Properties");
            Assert.That(form, Is.Not.Null);

            var detailForm = FormMethods.GetDetailForms(form).FirstOrDefault();
            Assert.That(detailForm, Is.Not.Null);

            var fields = detailForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field);
            Assert.That(fields, Is.Not.Null);
            Assert.That(fields.OfType<IElement>().Any(x=>x.getOrDefault<string>(_DatenMeister._Forms._FieldData.title) == "DefaultTypePackage"), Is.True);
        }

        
        /// <summary>
        /// Creates the zipExtent and also the FormsController
        /// </summary>
        /// <returns>Tuple of zipExtent and corresponding Forms Controller</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static (IUriExtent zipExtent, FormsController formsController, FormsControllerInternal internalFormController) CreateZipExtent()
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
            return (zipExtent!, formsController, new FormsControllerInternal(dm.WorkspaceLogic, dm.ScopeStorage));
        }
    }
}