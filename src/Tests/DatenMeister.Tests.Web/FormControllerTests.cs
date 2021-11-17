﻿#nullable enable
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
            Assert.That(foundForm, Is.Not.Null);
            Assert.That(foundForm.Value.IndexOf("tab", StringComparison.Ordinal) != -1);
        }
        
        
        [Test]
        public void TestDefaultForItem()
        {
            var (zipExtent, formsController) = CreateZipExtent();
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
            var (zipExtent, formsController) = CreateZipExtent();
            var firstElement = zipExtent.elements().First() as IElement;
            Assert.That(firstElement, Is.Not.Null);
            var foundForm = formsController.GetDefaultFormForItemInternal(
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
            var (zipExtent, formsController) = CreateZipExtent();
            var foundForm = formsController.GetDefaultFormForExtentInternal(
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

        /// <summary>
        /// Creates the zipExtent and also the FormsController
        /// </summary>
        /// <returns>Tuple of zipExtent and corresponding Forms Controller</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static (IUriExtent zipExtent, FormsController formsController) CreateZipExtent()
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
            return (zipExtent!, formsController);
        }
    }
}