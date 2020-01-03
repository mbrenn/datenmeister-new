#nullable  enable

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using MessageBox = System.Windows.MessageBox;

namespace DatenMeister.WPF.Modules.FormManager
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
                    var effectiveForm = itemExplorerControl.EffectiveForm;
                    if (effectiveForm == null)
                    {
                        MessageBox.Show("No effective form is set");
                        return;
                    }
                    
                    var window = itemExplorerControl.NavigationHost?.GetWindow();
                    var dlg = new ItemXmlViewWindow
                    {
                        /*SupportWriting = true,*/
                        Owner = window == null ? null : Window.GetWindow(window),
                        SupportWriting = false
                    };

                    dlg.UpdateContent(effectiveForm);
                    dlg.ShowDialog();
                },
                "",
                NavigationCategories.Form + ".Definition");

            yield return new ExtentMenuButtonDefinition(
                "Save Form Definition",
                x =>
                {
                    var viewLogic = GiveMe.Scope.Resolve<FormLogic>();
                    var target = viewLogic.GetUserFormExtent();
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
                        WorkspaceNames.UriUserFormExtent) as IElement;

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

                        var viewLogic = GiveMe.Scope.Resolve<FormLogic>();
                        var userViewExtent = viewLogic.GetUserFormExtent();
                        var factory = new MofFactory(userViewExtent);
                        var formAndFields = viewLogic.GetFormAndFieldInstance();

                        viewLogic.RemoveFormAssociationForExtentType(selectedExtentType);

                        var formAssociation = factory.create(formAndFields.__FormAssociation);
                        formAssociation.set(_FormAndFields._FormAssociation.extentType, selectedExtentType);
                        formAssociation.set(_FormAndFields._FormAssociation.form, itemExplorerControl.EffectiveForm);
                        formAssociation.set(_FormAndFields._FormAssociation.formType, FormType.TreeItemExtent);
                        userViewExtent.elements().add(formAssociation);

                        MessageBox.Show("View Association created");
                    }
                },
                string.Empty,
                NavigationCategories.Form + ".Definition"
            )
            {
                IsEnabled = itemExplorerControl.OverridingViewDefinition?.Element != null
            };

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

                        var viewLogic = GiveMe.Scope.Resolve<FormLogic>();

                        if (viewLogic.RemoveFormAssociationForExtentType(selectedExtentType))
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
                NavigationCategories.Form + ".Definition")
            {
                IsEnabled = itemExplorerControl.OverridingViewDefinition?.Element != null
            };

            yield return new ExtentMenuButtonDefinition(
                "Autogenerate form",
                x => { itemExplorerControl.ForceAutoGenerationOfForm(); },
                string.Empty,
                NavigationCategories.Form + ".Definition");

            // Inject the buttons to create a new class or a new property (should be done per default, but at the moment per plugin)
            var extent = itemExplorerControl.RootItem.GetExtentOf();
            var extentType = extent?.GetExtentType();
            if (extentType == FormLogic.ViewExtentType)
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

        /// <summary>
        /// Defines whether the form shall be created for an extent form or for the detail form
        /// </summary>
        enum CreateFormByClassifierType
        {
            /// <summary>
            /// To be chosen, when the form shall be created for a detail form
            /// </summary>
            DetailForm,
            
            /// <summary>
            /// To be chosen, when the form shall be created for an extent form
            /// </summary>
            ExtentForm
        }

        /// <summary>
        /// Creates a form by using the classifier
        /// </summary>
        /// <param name="itemExplorerControl">Navigational element to create the windows</param>
        /// <param name="type">Type of the form to be created</param>
        private static void AskUserAndCreateFormInstance(
            ItemExplorerControl itemExplorerControl, 
            CreateFormByClassifierType type)
        {
            if (NavigatorForDialogs.Locate(
                itemExplorerControl.NavigationHost,
                WorkspaceNames.NameTypes,
                WorkspaceNames.UriUserTypesExtent) is IElement locatedItem)
            {
                var formCreator = GiveMe.Scope.Resolve<FormCreator>();
                var viewLogic = GiveMe.Scope.Resolve<FormLogic>();
                var userViewExtent = viewLogic.GetUserFormExtent();

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
            var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Get<_FormAndFields>();
            
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
                "Set as default for Metaclass",
                SetAsDefaultForMetaclass,
                string.Empty,
                NavigationCategories.Form
            );

            yield return new ApplicationMenuButtonDefinition(
                "Clear default Association",
                ClearAssociation,
                null,
                NavigationCategories.Form);

            // Checks for specific elements
            var detailAsElement = detailWindow.DetailElement as IElement;
            var metaClassOfDetailElement = detailAsElement?.getMetaClass();
            if (formAndFields == null) throw new InvalidOperationException("No _FormAndFields in Type Workspace");

            if (ClassifierMethods.IsSpecializedClassifierOf(metaClassOfDetailElement, formAndFields.__Form)
                && detailAsElement != null)
            {
                yield return new ApplicationMenuButtonDefinition(
                    "Create Field by Property...",
                    CreateFieldByProperty,
                    null,
                    NavigationCategories.Form);
            }

            void ChangeFormDefinition()
            {
                var form = NavigatorForDialogs.Locate(
                    detailWindow,
                    WorkspaceNames.NameManagement,
                    WorkspaceNames.UriUserFormExtent) as IElement;

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

                var viewLogic = GiveMe.Scope.Resolve<FormLogic>();
                if (viewLogic.RemoveFormAssociationForDetailMetaClass(metaClass))
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

                var viewLogic = GiveMe.Scope.Resolve<FormLogic>();
                var userViewExtent = viewLogic.GetUserFormExtent();
                var factory = new MofFactory(userViewExtent);

                viewLogic.RemoveFormAssociationForDetailMetaClass(metaClass);

                var formAssociation = factory.create(formAndFields.__FormAssociation);
                formAssociation.set(_FormAndFields._FormAssociation.metaClass, metaClass);
                formAssociation.set(_FormAndFields._FormAssociation.form, detailWindow.OverridingViewDefinition.Element);
                formAssociation.set(_FormAndFields._FormAssociation.formType, FormType.Detail);
                userViewExtent.elements().add(formAssociation);

                MessageBox.Show("View Association created");
            }

            void CreateFieldByProperty()
            {
                if (NavigatorForDialogs.Locate(
                    viewExtensionTargetInformation.NavigationHost,
                    WorkspaceNames.NameTypes,
                    WorkspaceNames.UriUserTypesExtent) is IElement locatedItem)
                {
                    var formCreator = GiveMe.Scope.Resolve<FormCreator>();
                    formCreator.AddToFormByUmlElement(
                        detailAsElement!, // !Ok, since this method will only be called when detailAsElement is set 
                        locatedItem,
                        CreationMode.All);
                }
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
                "Goto User Forms",
                () => NavigatorForItems.NavigateToItemsInExtent(
                    viewExtensionTargetInformation.NavigationHost,
                    WorkspaceNames.NameManagement,
                    WorkspaceNames.UriUserFormExtent),
                string.Empty,
                NavigationCategories.DatenMeisterNavigation);

            return result;
        }
    }
}