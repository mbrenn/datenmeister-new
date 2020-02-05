#nullable enable

using System.Collections.Generic;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
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
                    
                    var viewLogic = GiveMe.Scope.Resolve<FormLogic>();
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
                var extentType = extent?.GetExtentType();
                if (extentType == FormLogic.FormExtentType)
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
                }
            }
        }
    }
}