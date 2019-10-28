﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Modules.ViewFinder.Helper;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using MessageBox = System.Windows.MessageBox;

namespace DatenMeister.WPF.Modules.ViewManager
{
    /// <summary>
    /// Contains the factory for the view extensions
    /// </summary>
    public class FormManagerViewExtension : IViewExtensionFactory
    {
        /// <summary>
        /// Gets the view extension
        /// </summary>
        /// <param name="viewExtensionTargetInformation"></param>
        /// <returns></returns>
        public IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            var navigationGuest = viewExtensionTargetInformation.NavigationGuest;
            var navigationHost = viewExtensionTargetInformation.NavigationHost;
            
            var itemExplorerControl = navigationGuest as ItemExplorerControl;
            var detailFormControl = navigationGuest as DetailFormControl;
            var detailFormWindow = navigationHost as DetailFormWindow;

            if (navigationHost is IApplicationWindow)
            {
                yield return GetForApplicationWindow(viewExtensionTargetInformation);
            }

            if (detailFormWindow != null)
            {
                foreach (var viewExtension in GetForDetailWindow(
                    viewExtensionTargetInformation,
                    detailFormWindow))
                {
                    yield return viewExtension;
                }
            }

            if (itemExplorerControl != null)
            {
                foreach (var viewExtension in GetForItemExplorerControl(itemExplorerControl))
                {
                    yield return viewExtension;
                }
            }
        }

        private static IEnumerable<ViewExtension> GetForItemExplorerControl(
            ItemExplorerControl itemExplorerControl)
        {
            yield return new ExtentMenuButtonDefinition(
                "Show Form Definition",
                x =>
                {
                    var dlg = new ItemXmlViewWindow
                    {
                        /*SupportWriting = true,*/
                        Owner = Window.GetWindow(itemExplorerControl.NavigationHost.GetWindow()),
                        SupportWriting = false
                    };

                    dlg.UpdateContent(itemExplorerControl.EffectiveForm);

                    dlg.ShowDialog();
                },
                "",
                NavigationCategories.Form + ".Definition");

            yield return new ExtentMenuButtonDefinition(
                "Save Form Definition",
                x =>
                {
                    var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
                    var target = viewLogic.GetUserViewExtent();
                    var copier = new ObjectCopier(new MofFactory(target));

                    var copiedForm = copier.Copy(itemExplorerControl.EffectiveForm);
                    target.elements().add(copiedForm);

                    NavigatorForItems.NavigateToElementDetailView(itemExplorerControl.NavigationHost,
                        copiedForm);
                },
                "",
                NavigationCategories.Form + ".Definition");

            yield return new ExtentMenuButtonDefinition(
                "Change Form Definition",
                x =>
                {
                    var form = NavigatorForDialogs.Locate(
                        itemExplorerControl.NavigationHost,
                        WorkspaceNames.NameManagement,
                        WorkspaceNames.UriUserViewExtent) as IElement;

                    if (form == null)
                    {
                        itemExplorerControl.ClearOverridingForm();
                    }
                    else
                    {
                        itemExplorerControl.SetOverridingForm(form);
                    }
                },
                "",
                NavigationCategories.Form + ".Definition");

            yield return new ExtentMenuButtonDefinition(
                "Reset Form Definition",
                x => itemExplorerControl.ClearOverridingForm(),
                string.Empty,
                NavigationCategories.Form + ".Definition");

            yield return new ExtentMenuButtonDefinition(
                "Set as default for extenttype",
                x =>
                {
                    if (itemExplorerControl.OverridingViewDefinition?.Element == null)
                    {
                        MessageBox.Show(
                            "The used form is automatically selected. This automatic selection cannot be put as a standard");
                        return;
                    }

                    if (itemExplorerControl.SelectedItem is IExtent selectedExtent)
                    {
                        var selectedExtentType = selectedExtent.GetExtentType();
                        if (string.IsNullOrEmpty(selectedExtentType))
                        {
                            MessageBox.Show("Given Extent does not contain an extent type, so rule cannot be created");
                            return;
                        }

                        if (MessageBox.Show(
                                $"The current view will be defined as the standard view for the extent type: {selectedExtentType}. \r\n\r\n Is this correct?",
                                "Confirmation",
                                MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }

                        var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
                        var userViewExtent = viewLogic.GetUserViewExtent();
                        var factory = new MofFactory(userViewExtent);
                        var formAndFields = viewLogic.GetFormAndFieldInstance();

                        viewLogic.RemoveViewAssociationForExtentType(selectedExtentType);

                        var viewAssociation = factory.create(formAndFields.__ViewAssociation);
                        viewAssociation.set(_FormAndFields._ViewAssociation.extentType, selectedExtentType);
                        viewAssociation.set(_FormAndFields._ViewAssociation.form, itemExplorerControl.EffectiveForm);
                        viewAssociation.set(_FormAndFields._ViewAssociation.viewType, ViewType.TreeItemExtent);
                        userViewExtent.elements().add(viewAssociation);

                        MessageBox.Show("View Association created");
                    }
                },
                string.Empty,
                NavigationCategories.Form + ".Definition"
            );

            yield return new ExtentMenuButtonDefinition(
                "Clear default association",
                x =>
                {
                    if (itemExplorerControl.SelectedItem is IExtent selectedExtent)
                    {
                        var selectedExtentType = selectedExtent.GetExtentType();
                        if (string.IsNullOrEmpty(selectedExtentType))
                        {
                            MessageBox.Show("Given Extent does not contain an extent type, so rule cannot be created");
                            return;
                        }

                        var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();

                        if (viewLogic.RemoveViewAssociationForExtentType(selectedExtentType))
                        {
                            MessageBox.Show("View Association deleted");
                        }
                        else
                        {
                            MessageBox.Show("No default view association was found.");
                        }
                    }
                },
                string.Empty,
                NavigationCategories.Form + ".Definition");

            yield return new ExtentMenuButtonDefinition(
                "Autogenerate form",
                x => { itemExplorerControl.ForceAutoGenerationOfForm(); },
                string.Empty,
                NavigationCategories.Form + ".Definition");

            // Inject the buttons to create a new class or a new property (should be done per default, but at the moment per plugin)
            var extent = itemExplorerControl.RootItem.GetExtentOf();
            var extentType = extent?.GetExtentType();
            if (extentType == ViewLogic.ViewExtentType)
            {
                yield return new ItemMenuButtonDefinition(
                    "Create Extent Form by Classifier",
                    (x) => AskUserAndCreateFormInstance(itemExplorerControl, CreateFormByClassifierType.ExtentForm),
                    null,
                    "Form Manager.Create");

                yield return new ItemMenuButtonDefinition(
                    "Create Detail Form by Classifier",
                    (x) => AskUserAndCreateFormInstance(itemExplorerControl, CreateFormByClassifierType.DetailForm),
                    null,
                    "Form Manager.Create");
            }
        }

        enum CreateFormByClassifierType
        {
            DetailForm,
            ExtentForm
        }

        /// <summary>
        /// Creates a form by using the classifier
        /// </summary>
        /// <param name="itemExplorerControl">Navigational element to create the windows</param>
        /// <param name="type">Type of the form to be created</param>
        private static void AskUserAndCreateFormInstance(ItemExplorerControl itemExplorerControl, CreateFormByClassifierType type)
        {
            if (NavigatorForDialogs.Locate(
                itemExplorerControl.NavigationHost,
                WorkspaceNames.NameTypes,
                WorkspaceNames.UriUserTypesExtent) is IElement locatedItem)
            {
                var formCreator = GiveMe.Scope.Resolve<FormCreator>();
                var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
                var userViewExtent = viewLogic.GetUserViewExtent();

                IElement createdForm;
                switch (type)
                {
                    case CreateFormByClassifierType.DetailForm:
                        createdForm = formCreator.CreateDetailFormByMetaClass(locatedItem);
                        break;
                    case CreateFormByClassifierType.ExtentForm:
                        createdForm = formCreator.CreateExtentFormByMetaClass(locatedItem);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                userViewExtent.elements().add(createdForm);

                NavigatorForItems.NavigateToElementDetailView(
                    itemExplorerControl.NavigationHost,
                    createdForm);
            }
        }

        /// <summary>
        /// Gets the view extenstions for the detail window
        /// </summary>
        /// <param name="viewExtensionTargetInformation">The information about guests and hosts</param>
        /// <param name="detailWindow"></param>
        /// <returns></returns>
        private static IEnumerable<ViewExtension> GetForDetailWindow(
            ViewExtensionTargetInformation viewExtensionTargetInformation,
            DetailFormWindow detailWindow)
        {
            yield return new ApplicationMenuButtonDefinition(
                "Change Form Definition",
                ChangeFormDefinition,
                null,
                NavigationCategories.Form);

            yield return new ApplicationMenuButtonDefinition(
                "Reset Form Definition",
                ClearOverridingForm,
                null,
                NavigationCategories.Form);

            yield return new ApplicationMenuButtonDefinition(
                "Set as default for metaclass",
                SetAsDefaultForMetaclass,
                string.Empty,
                NavigationCategories.Form
            );

            yield return new ApplicationMenuButtonDefinition(
                "Clear default association",
                ClearAssociation,
                null,
                NavigationCategories.Form);
            
            void ChangeFormDefinition()
            {
                var form = NavigatorForDialogs.Locate(
                    detailWindow,
                    WorkspaceNames.NameManagement,
                    WorkspaceNames.UriUserViewExtent) as IElement;

                if (form == null)
                {
                    detailWindow.ClearOverridingForm();
                }
                else
                {
                    detailWindow.SetOverridingForm(form);
                }
            }
            
            void ClearOverridingForm()
            {
                detailWindow.ClearOverridingForm();
            }

            void ClearAssociation()
            {
                var detailElement = detailWindow.DetailElement as IElement;
                var metaClass = detailElement?.metaclass;
                if (metaClass == null)
                {
                    MessageBox.Show(
                        "The current detail element does not contain a metaclass.\r\nThis means that the association cannot be removed.");
                    return;
                }

                var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
                if (viewLogic.RemoveViewAssociationForDetailMetaClass(metaClass))
                {
                    MessageBox.Show("View Association deleted");
                }
                else
                {
                    MessageBox.Show("No default view association was found.");
                }
            }

            void SetAsDefaultForMetaclass()
            {
                if (detailWindow.OverridingViewDefinition?.Element == null)
                {
                    MessageBox.Show(
                        "The used form is automatically selected. This automatic selection cannot be put as a standard");
                    return;
                }


                var detailElement = detailWindow.DetailElement as IElement;
                var metaClass = detailElement?.metaclass;

                if (metaClass == null)
                {
                    MessageBox.Show(
                        "The current detail element does not contain a metaclass.\r\nThis means that the association cannot be removed.");
                    return;
                }

                if (MessageBox.Show(
                        $"The current view will be defined as the standard view for the metaclass: {NamedElementMethods.GetFullName(metaClass)}. \r\n\r\n Is this correct?",
                        "Confirmation",
                        MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }

                var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
                var userViewExtent = viewLogic.GetUserViewExtent();
                var factory = new MofFactory(userViewExtent);
                var formAndFields = viewLogic.GetFormAndFieldInstance();

                viewLogic.RemoveViewAssociationForDetailMetaClass(metaClass);

                var viewAssociation = factory.create(formAndFields.__ViewAssociation);
                viewAssociation.set(_FormAndFields._ViewAssociation.metaClass, metaClass);
                viewAssociation.set(_FormAndFields._ViewAssociation.form, detailWindow.OverridingViewDefinition.Element);
                viewAssociation.set(_FormAndFields._ViewAssociation.viewType, ViewType.Detail);
                userViewExtent.elements().add(viewAssociation);

                MessageBox.Show("View Association created");
            }
        }

        /// <summary>
        /// Gets the navigation for the application window
        /// </summary>
        /// <param name="viewExtensionTargetInformation"></param>
        /// <returns></returns>
        private static ViewExtension GetForApplicationWindow(ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            var result = new ApplicationMenuButtonDefinition(
                "Goto User Views",
                () => NavigatorForItems.NavigateToItemsInExtent(
                    viewExtensionTargetInformation.NavigationHost,
                    WorkspaceNames.NameManagement,
                    WorkspaceNames.UriUserViewExtent),
                string.Empty,
                NavigationCategories.DatenMeisterNavigation);

            return result;
        }
    }
}