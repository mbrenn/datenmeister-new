#nullable enable

using System;
using System.Collections.Generic;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormCreator;
using DatenMeister.Integration.DotNet;
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
                "Open Form as Xmi",
                x =>
                {
                    var effectiveForm = itemExplorerControl.EffectiveForm;
                    if (effectiveForm == null)
                    {
                        MessageBox.Show("No effective form is set");
                        return;
                    }

                    var url = effectiveForm.getOrDefault<string>(_DatenMeister._Forms._Form.originalUri);
                    var originalForm = string.IsNullOrEmpty(url)
                        ? null
                        : GiveMe.Scope.WorkspaceLogic.FindItem(url) as IObject;

                    var window = itemExplorerControl.NavigationHost?.GetWindow();
                    var dlg = new ItemXmlViewWindow
                    {
                        /*SupportWriting = true,*/
                        Owner = window == null ? null : Window.GetWindow(window),
                        SupportWriting = false
                    };
                    
                    if (originalForm == null)
                    {
                        MessageBox.Show(
                            "The form is dynamically created. Modifications will not be stored permanently.");
                        
                        dlg.UpdateContent(effectiveForm);
                    }
                    else
                    {
                        dlg.UpdateContent(originalForm);
                    }
                    
                    dlg.ShowDialog();
                },
                "",
                NavigationCategories.Form + ".Current");
            
            yield return new ExtentMenuButtonDefinition(
                "Open Form",
                async x =>
                {
                    var effectiveForm = itemExplorerControl.EffectiveForm;
                    if (effectiveForm == null)
                    {
                        MessageBox.Show("No effective form is set");
                        return;
                    }

                    var url = effectiveForm.getOrDefault<string>(_DatenMeister._Forms._Form.originalUri);
                    var originalForm = string.IsNullOrEmpty(url)
                        ? null
                        : GiveMe.Scope.WorkspaceLogic.FindItem(url) as IObject;
                    
                    if (originalForm == null)
                    {
                        MessageBox.Show(
                            "The form is dynamically created. Modifications will not be stored permanently.");

                        await NavigatorForItems.NavigateToElementDetailView(
                            itemExplorerControl.NavigationHost ?? throw new InvalidOperationException("NavigationHost is not set"),
                            effectiveForm, 
                            title: "Dynamic Form");
                    }
                    else
                    {
                        await NavigatorForItems.NavigateToElementDetailView(
                            itemExplorerControl.NavigationHost ?? throw new InvalidOperationException("NavigationHost is not set"),
                            originalForm, 
                            title: "Form");
                    }
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
                NavigationCategories.Form + ".Change");

            yield return new ExtentMenuButtonDefinition(
                "Change Form Definition",
                async x =>
                {
                    if (!(await NavigatorForDialogs.Locate(
                        itemExplorerControl.NavigationHost,
                        WorkspaceNames.WorkspaceManagement,
                        WorkspaceNames.UriExtentUserForm) is IElement form))
                    {
                        itemExplorerControl.ClearOverridingForm();
                    }
                    else
                    {
                        itemExplorerControl.SetOverridingForm(form);
                    }
                },
                "",
                NavigationCategories.Form + ".Change");

            yield return new ExtentMenuButtonDefinition(
                "Reset Form Definition",
                x => itemExplorerControl.ClearOverridingForm(),
                string.Empty,
                NavigationCategories.Form + ".Change");

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

                        viewLogic.RemoveFormAssociationForExtentType(selectedExtentType);

                        var formAssociation = factory.create(_DatenMeister.TheOne.Forms.__FormAssociation);
                        formAssociation.set(_DatenMeister._Forms._FormAssociation.extentType, selectedExtentType);
                        formAssociation.set(_DatenMeister._Forms._FormAssociation.form, itemExplorerControl.EffectiveForm);
                        formAssociation.set(_DatenMeister._Forms._FormAssociation.formType, _DatenMeister._Forms.___FormType.TreeItemExtent);
                        userViewExtent.elements().add(formAssociation);

                        MessageBox.Show("View Association created");
                    }
                },
                string.Empty,
                NavigationCategories.Form + ".Create"
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
                NavigationCategories.Form + ".Create")
            {
                IsEnabled = itemExplorerControl.OverridingViewDefinition?.Element != null
            };

            yield return new ExtentMenuButtonDefinition(
                "Autogenerate form",
                x => { itemExplorerControl.ForceAutoGenerationOfForm(); },
                string.Empty,
                NavigationCategories.Form + ".Change");

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
                        "Create List Form by Classifier",
                        (x) => AskUserAndCreateFormInstance(itemExplorerControl, CreateFormByClassifierType.ListForm),
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
            ExtentForm,
            
            /// <summary>
            /// To be chosen when the form shall be created for a list form
            /// </summary>
            ListForm
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
                WorkspaceNames.WorkspaceTypes,
                WorkspaceNames.UriExtentUserTypes) is IElement locatedItem)
            {
                var formCreator = GiveMe.Scope.Resolve<FormCreator>();

                IElement createdForm = type switch
                {
                    CreateFormByClassifierType.DetailForm => formCreator.CreateDetailFormByMetaClass(locatedItem),
                    CreateFormByClassifierType.ExtentForm => formCreator.CreateExtentFormByMetaClass(locatedItem),
                    CreateFormByClassifierType.ListForm => formCreator.CreateListFormForMetaClass(
                        locatedItem,
                        CreationMode.ForListForms | CreationMode.ByMetaClass),
                    _ => throw new InvalidOperationException()
                };

                DefaultClassifierHints.AddToExtentOrElement(
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
                WorkspaceNames.WorkspaceTypes,
                WorkspaceNames.UriExtentUserTypes) is IElement locatedItem)
            {
                // Prepare the variables and fullname
                var containerExtent = selectedItem.GetUriExtentOf() ??
                                      throw new InvalidOperationException("Extent is not found");
                var factory = new MofFactory(containerExtent);
                var fullName = NamedElementMethods.GetFullName(locatedItem, ".");
                var className = NamedElementMethods.GetName(locatedItem);
                
                var formCreator = GiveMe.Scope.Resolve<FormCreator>();
                
                ////////////////////////////////////
                // Creates the package
                var packageClassifier = DefaultClassifierHints.GetDefaultPackageClassifier(containerExtent);
                var package = factory.create(packageClassifier);
                package.set(_UML._CommonStructure._NamedElement.name, className);
                DefaultClassifierHints.AddToExtentOrElement(selectedItem, package);
                var packageName = "Package-" + fullName;
                ExtentHelper.SetAvailableId(containerExtent, package, packageName);

                // Creates the detail form
                var detailForm = formCreator.CreateDetailFormByMetaClass(locatedItem);
                DefaultClassifierHints.AddToExtentOrElement(
                    package, 
                    detailForm);
                var name = fullName + "FormDetail";
                ExtentHelper.SetAvailableId(containerExtent, detailForm, name);
                
                // Creates the extent form
                var extentForm = formCreator.CreateExtentFormByMetaClass(locatedItem);
                DefaultClassifierHints.AddToExtentOrElement(
                    package, 
                    extentForm);
                name = fullName + "FormList";
                ExtentHelper.SetAvailableId(containerExtent, extentForm, name);

                // Creates association
                var formLogic = GiveMe.Scope.Resolve<FormsPlugin>();
                var association1 = 
                    formLogic.AddFormAssociationForMetaclass(detailForm, locatedItem, _DatenMeister._Forms.___FormType.Detail);
                var association2 = 
                    formLogic.AddFormAssociationForMetaclass(extentForm, locatedItem, _DatenMeister._Forms.___FormType.TreeItemDetail);
                
                DefaultClassifierHints.AddToExtentOrElement(package, association1);
                name = fullName + "AssociationDetail";
                ExtentHelper.SetAvailableId(containerExtent, association1, name);

                DefaultClassifierHints.AddToExtentOrElement(package, association2);
                name = fullName + "AssociationList";
                ExtentHelper.SetAvailableId(containerExtent, association2, name);
            }
        }
    }
}