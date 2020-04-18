#nullable enable

using System;
using System.Collections.Generic;
using System.Windows;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using MessageBox = System.Windows.MessageBox;

namespace DatenMeister.WPF.Modules.FormManager
{
    /// <summary>
    /// Contains the factory for the view extensions
    /// </summary>
    public partial class FormManagerViewExtension : IViewExtensionFactory
    {
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
                NavigationCategories.Form + ".Current");

            yield return new ExtentMenuButtonDefinition(
                "Save Form Definition",
                x =>
                {
                    var effectiveForm = itemExplorerControl.EffectiveForm;
                    if (effectiveForm == null)
                    {
                        MessageBox.Show("No effective form is set");
                        return;
                    }
                    
                    var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
                    var target = viewLogic.GetUserFormExtent();
                    var copier = new ObjectCopier(new MofFactory(target));

                    var copiedForm = copier.Copy(effectiveForm);
                    target.elements().add(copiedForm);

                    _ = NavigatorForItems.NavigateToElementDetailView(itemExplorerControl.NavigationHost,
                        copiedForm);
                },
                "",
                NavigationCategories.Form + ".Current");

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
                NavigationCategories.Form + ".Current");

            yield return new ExtentMenuButtonDefinition(
                "Reset Form Definition",
                x => itemExplorerControl.ClearOverridingForm(),
                string.Empty,
                NavigationCategories.Form + ".Current");

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
                        var selectedExtentType = selectedExtent.GetConfiguration().ExtentType;
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

                        var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
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
                NavigationCategories.Form + ".Current"
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
                        var selectedExtentType = selectedExtent.GetConfiguration().ExtentType;
                        if (string.IsNullOrEmpty(selectedExtentType))
                        {
                            MessageBox.Show("Given Extent does not contain an extent type, so rule cannot be created");
                            return;
                        }

                        var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();

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
                NavigationCategories.Form + ".Current")
            {
                IsEnabled = itemExplorerControl.OverridingViewDefinition?.Element != null
            };

            yield return new ExtentMenuButtonDefinition(
                "Autogenerate form",
                x => { itemExplorerControl.ForceAutoGenerationOfForm(); },
                string.Empty,
                NavigationCategories.Form + ".Current");

            // Inject the buttons to create a new class or a new property (should be done per default, but at the moment per plugin)
            var rootItem = itemExplorerControl.RootItem;

            if (rootItem != null)
            {
                var extent = rootItem.GetExtentOf();
                var extentType = extent?.GetConfiguration().ExtentType;
                if (extentType == FormsPlugin.FormExtentType)
                {
                    yield return new ItemMenuButtonDefinition(
                        "Create Extent Form by Classifier",
                        (x) => AskUserAndCreateFormInstance(itemExplorerControl, CreateFormByClassifierType.ExtentForm),
                        null,
                        "Form.Form Manager");

                    yield return new ItemMenuButtonDefinition(
                        "Create Detail Form by Classifier",
                        (x) => AskUserAndCreateFormInstance(itemExplorerControl, CreateFormByClassifierType.DetailForm),
                        null,
                        "Form.Form Manager");

                    yield return new ItemMenuButtonDefinition(
                        "Create Forms and Association",
                        (x) => AskUserForFormsAndAssociation(itemExplorerControl),
                        null,
                        "Form.Form Manager");
                }
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
        private static async void AskUserAndCreateFormInstance(
            ItemExplorerControl itemExplorerControl, 
            CreateFormByClassifierType type)
        {
            var navigationHost = itemExplorerControl.NavigationHost ??
                                 throw new InvalidOperationException("navigationHost == null");
            if (!(itemExplorerControl.SelectedItem is { } selectedItem))
            {
                MessageBox.Show("No item is selected.");
                return;
            }
            
            if (await NavigatorForDialogs.Locate(
                navigationHost,
                WorkspaceNames.NameTypes,
                WorkspaceNames.UriUserTypesExtent) is IElement locatedItem)
            {
                var formCreator = GiveMe.Scope.Resolve<FormCreator>();

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
                
                GiveMe.Scope.Resolve<DefaultClassifierHints>().AddToExtentOrElement(
                    selectedItem, 
                    createdForm);

                _ = NavigatorForItems.NavigateToElementDetailView(
                    navigationHost,
                    createdForm);
            }
        }

        /// <summary>
        /// Creates a form by using the classifier
        /// </summary>
        /// <param name="itemExplorerControl">Navigational element to create the windows</param>
        /// <param name="type">Type of the form to be created</param>
        private static async void AskUserForFormsAndAssociation(
            ItemExplorerControl itemExplorerControl)
        {   
            var navigationHost = itemExplorerControl.NavigationHost ??
                                 throw new InvalidOperationException("navigationHost == null");
            if (!(itemExplorerControl.SelectedItem is { } selectedItem))
            {
                MessageBox.Show("No item is selected.");
                return;
            }

            if (await NavigatorForDialogs.Locate(
                navigationHost,
                WorkspaceNames.NameTypes,
                WorkspaceNames.UriUserTypesExtent) is IElement locatedItem)
            {
                // Prepare the variables and fullname
                var containerExtent = selectedItem.GetUriExtentOf() ??
                                      throw new InvalidOperationException("Extent is not found");
                var factory = new MofFactory(containerExtent);
                var fullName = NamedElementMethods.GetFullName(locatedItem, ".");
                var className = NamedElementMethods.GetName(locatedItem);
                
                var formCreator = GiveMe.Scope.Resolve<FormCreator>();
                var defaultClassifierHints = GiveMe.Scope.Resolve<DefaultClassifierHints>();
                
                ////////////////////////////////////
                // Creates the package
                var packageClassifier = defaultClassifierHints.GetDefaultPackageClassifier(containerExtent);
                var package = factory.create(packageClassifier);
                package.set(_UML._CommonStructure._NamedElement.name, className);
                defaultClassifierHints.AddToExtentOrElement(selectedItem, package);
                var packageName = "Package-" + fullName;
                ExtentHelper.SetAvailableId(containerExtent, package, packageName);

                // Creates the detail form
                var detailForm = formCreator.CreateDetailFormByMetaClass(locatedItem);
                defaultClassifierHints.AddToExtentOrElement(
                    package, 
                    detailForm);
                var name = fullName + "FormDetail";
                ExtentHelper.SetAvailableId(containerExtent, detailForm, name);
                
                // Creates the extent form
                var extentForm = formCreator.CreateExtentFormByMetaClass(locatedItem);
                defaultClassifierHints.AddToExtentOrElement(
                    package, 
                    extentForm);
                name = fullName + "FormList";
                ExtentHelper.SetAvailableId(containerExtent, extentForm, name);

                // Creates association
                var formLogic = GiveMe.Scope.Resolve<FormsPlugin>();
                var association1 = 
                    formLogic.AddFormAssociationForMetaclass(detailForm, locatedItem, FormType.Detail);
                var association2 = 
                    formLogic.AddFormAssociationForMetaclass(extentForm, locatedItem, FormType.TreeItemDetail);
                
                defaultClassifierHints.AddToExtentOrElement(package, association1);
                name = fullName + "AssociationDetail";
                ExtentHelper.SetAvailableId(containerExtent, association1, name);

                defaultClassifierHints.AddToExtentOrElement(package, association2);
                name = fullName + "AssociationList";
                ExtentHelper.SetAvailableId(containerExtent, association2, name);
            }
        }
    }
}