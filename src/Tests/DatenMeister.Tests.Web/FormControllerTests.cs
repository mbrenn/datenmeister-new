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
            var foundForm = formsController.GetCollectionFormForExtent(
                WorkspaceNames.WorkspaceData,
                zipExtent.contextURI(),
                ViewModes.Default);
            Assert.That(foundForm, Is.Not.Null);
            Assert.That(foundForm!.Value!.IndexOf("tab", StringComparison.Ordinal) != -1);
        }

        [Test]
        public void TestDefaultForItem()
        {
            var (zipExtent, formsController, _) = CreateZipExtent();
            var firstElement = zipExtent.elements().First() as IElement;
            Assert.That(firstElement, Is.Not.Null);
            var foundForm = formsController.GetObjectFormForItem(
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
            var foundForm = formsInternal.GetObjectFormForItemInternal(
                WorkspaceNames.WorkspaceData,
                firstElement!.GetUri()!,
                ViewModes.Default)!;

            Assert.That(foundForm, Is.Not.Null);
            var detailForm = FormMethods.GetRowForms(foundForm).FirstOrDefault();
            Assert.That(detailForm, Is.Not.Null);
            var fields = detailForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field);
            var foundFields =
                fields.OfType<IElement>()
                    .Where(
                        x =>
                            x.getOrDefault<string>(_DatenMeister._Forms._ActionFieldData.actionName) ==
                            ItemsFormsPlugin.NavigationItemDelete)
                    .ToList();

            Assert.That(foundFields.Any(), Is.True);
            Assert.That(foundFields.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestTableFormForDefaultButtons()
        {
            var (zipExtent, formsController, formsInternal) = CreateZipExtent();
            var foundForm = formsInternal.GetCollectionFormForExtentInternal(
                WorkspaceNames.WorkspaceData,
                zipExtent.contextURI(),
                ViewModes.Default)!;

            Assert.That(foundForm, Is.Not.Null);
            var listForm = FormMethods.GetTableForms(foundForm).FirstOrDefault();
            Assert.That(listForm, Is.Not.Null);
            var fields = listForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field);

            var foundFields =
                fields.OfType<IElement>()
                    .Where(
                        x =>
                            x.getOrDefault<string>(_DatenMeister._Forms._ActionFieldData.actionName) ==
                            ItemsFormsPlugin.NavigationExtentsListDeleteItem)
                    .ToList();

            Assert.That(foundFields.Any(), Is.True);
            Assert.That(foundFields.Count, Is.EqualTo(1));

            foundFields =
                fields.OfType<IElement>()
                    .Where(
                        x =>
                            x.getOrDefault<string>(_DatenMeister._Forms._ActionFieldData.actionName) ==
                            ItemsFormsPlugin.NavigationExtentsListViewItem)
                    .ToList();

            Assert.That(foundFields.Any(), Is.True);
            Assert.That(foundFields.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestWorkspaceFormForViewExtentButtonInTableForm()
        {
            var (zipExtent, formsController, formsInternal) = CreateZipExtent();
            var foundForm = formsInternal.GetObjectFormForItemInternal(
                WorkspaceNames.WorkspaceManagement,
                WorkspaceNames.UriExtentWorkspaces + "#Data",
                ViewModes.Default)!;

            Assert.That(foundForm, Is.Not.Null);
            var listForm = FormMethods.GetTableForms(foundForm).FirstOrDefault();
            Assert.That(listForm, Is.Not.Null);
            var fields = listForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field);

            // Check that field is at first five positions
            var firstField =
                fields.OfType<IElement>()
                    .Take(5)
                    .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._ActionFieldData.actionName) ==
                                         ExtentFormPlugin.NavigationExtentNavigateTo);
            Assert.That(firstField, Is.Not.Null);
        }

        [Test]
        public void TestObjectFormForMetaClass()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var localType = new LocalTypeSupport(dm.WorkspaceLogic, dm.ScopeStorage);
            var zipCode = localType.GetMetaClassFor(typeof(ZipCode))!;
            Assert.That(zipCode, Is.Not.Null);

            var zipCodeMetaUrl = zipCode.GetUri()!;
            Assert.That(zipCodeMetaUrl, Is.Not.Null);

            var controller = new FormsControllerInternal(dm.WorkspaceLogic, dm.ScopeStorage);
            var form = (controller.GetObjectFormForMetaClassInternal(zipCodeMetaUrl) as IElement)!;

            Assert.That(form, Is.Not.Null);
            Assert.That(form.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__ObjectForm), Is.True);
            var rowForm = FormMethods.GetRowForms(form).FirstOrDefault();
            Assert.That(rowForm, Is.Not.Null);
            var fields = rowForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._TableForm.field);
            Assert.That(fields.Count(), Is.GreaterThan(3));

            var positionLat = fields
                .OfType<IElement>()
                .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) ==
                                     nameof(ZipCode.positionLat));
            Assert.That(positionLat, Is.Not.Null);
        }

        [Test]
        public void TestFormByUri()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();

            var controller = new FormsControllerInternal(dm.WorkspaceLogic, dm.ScopeStorage);
            var form = controller.GetInternal(WorkspaceNames.UriExtentInternalForm + "#DatenMeister.Extent.Properties");
            Assert.That(form, Is.Not.Null);

            var detailForm = FormMethods.GetRowForms(form).FirstOrDefault();
            Assert.That(detailForm, Is.Not.Null);

            var fields = detailForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field);
            Assert.That(fields, Is.Not.Null);
            Assert.That(
                fields.OfType<IElement>().Any(x =>
                    x.getOrDefault<string>(_DatenMeister._Forms._FieldData.title) == "DefaultTypes"), Is.True);
        }


        /// <summary>
        /// Creates the zipExtent and also the FormsController
        /// </summary>
        /// <returns>Tuple of zipExtent and corresponding Forms Controller</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static (
            IUriExtent zipExtent,
            FormsController formsController,
            FormsControllerInternal internalFormController) CreateZipExtent()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();

            var zipController = new ZipController(dm.WorkspaceLogic, dm.ScopeStorage);

            var extentsData = dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)
                              ?? throw new InvalidOperationException("No management workspace found");
            
            var result = zipController.CreateZipExample(new ZipController.CreateZipExampleParam
            {
                Workspace = WorkspaceNames.WorkspaceData
            });

            Assert.That(result.Value, Is.Not.Null);

            var zipExtent = dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!
                .extent
                .OfType<IUriExtent>()
                .FirstOrDefault(x=>x.contextURI() == result.Value!.ExtentUri);

            var formsController = new FormsController(dm.WorkspaceLogic, dm.ScopeStorage);
            return (zipExtent!, formsController, new FormsControllerInternal(dm.WorkspaceLogic, dm.ScopeStorage));
        }
    }
}