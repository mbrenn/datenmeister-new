#nullable enable

using System;
using System.Collections.Generic;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormCreator;
using DatenMeister.Integration.DotNet;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Modules.FormManager
{
    /// <summary>
    /// Contains the factory for the view extensions
    /// </summary>
    public partial class FormManagerViewExtension : IViewExtensionFactory
    {

        /// <summary>
        /// Gets the view extenstions for the detail window
        /// </summary>
        /// <param name="viewExtensionInfo">The information about guests and hosts</param>
        /// <param name="detailWindow"></param>
        /// <returns></returns>
        private static IEnumerable<ViewExtension> GetForDetailWindow(
            ViewExtensionInfo viewExtensionInfo,
            DetailFormWindow detailWindow)
        {
            if (!(detailWindow.MainControl is DetailFormControl detailFormControl))
            {
                yield break;
            }
            
            // Checks for specific elements
            var detailAsElement = detailWindow.DetailElement as IElement;
            var metaClassOfDetailElement = detailAsElement?.getMetaClass();
            
            yield return new ApplicationMenuButtonDefinition(
                "Open Form as Xmi",
                () =>
                {
                    var effectiveForm = detailFormControl.EffectiveForm;
                    if (effectiveForm == null)
                    {
                        MessageBox.Show("No effective form is set");
                        return;
                    }

                    var url = effectiveForm.getOrDefault<string>(_DatenMeister._Forms._Form.originalUri);
                    var originalForm = string.IsNullOrEmpty(url)
                        ? null
                        : GiveMe.Scope.WorkspaceLogic.FindItem(url) as IObject;

                    var window = detailWindow.GetWindow();
                    var dlg = new ItemXmlViewWindow
                    {
                        /*SupportWriting = true,*/
                        Owner = window,
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
            
            yield return new ApplicationMenuButtonDefinition(
                "Open Form",
                async () =>
                {
                    var effectiveForm = detailFormControl.EffectiveForm;
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
                            detailWindow ?? throw new InvalidOperationException("NavigationHost is not set"),
                            effectiveForm, 
                            title: "Dynamic Form");
                    }
                    else
                    {
                        await NavigatorForItems.NavigateToElementDetailView(
                            detailWindow ?? throw new InvalidOperationException("NavigationHost is not set"),
                            originalForm, 
                            title: "Form");
                    }
                },
                "",
                NavigationCategories.Form + ".Current");
            
            yield return new ApplicationMenuButtonDefinition(
                "Change Form Definition",
                ChangeFormDefinition,
                null,
                NavigationCategories.Form + ".Change");

            yield return new ApplicationMenuButtonDefinition(
                "Reset Form Definition",
                ClearOverridingForm,
                null,
                NavigationCategories.Form +".Change");

            yield return new ApplicationMenuButtonDefinition(
                "Set as default for Metaclass",
                SetAsDefaultForMetaclass,
                string.Empty,
                NavigationCategories.Form+".Create"
            );

            yield return new ApplicationMenuButtonDefinition(
                "Clear default Association",
                ClearAssociation,
                null,
                NavigationCategories.Form+".Create");
            

            yield return new ApplicationMenuButtonDefinition(
                "Autogenerate form",
                detailWindow.ForceAutoGenerationOfForm,
                string.Empty,
                NavigationCategories.Form + ".Change");


            // The currently selected element is a form
            if (ClassifierMethods.IsSpecializedClassifierOf(metaClassOfDetailElement, _DatenMeister.TheOne.Forms.__Form)
                && detailAsElement != null)
            {
                yield return new ApplicationMenuButtonDefinition(
                    "Create Field by Property...",
                    CreateFieldByProperty,
                    null,
                    NavigationCategories.Form + ".Form Manager");

                yield return new ApplicationMenuButtonDefinition(
                    "Set as default form for type...",
                    CreateAsDefaultType,
                    null,
                    NavigationCategories.Form + ".Form Manager");
            }

            async void ChangeFormDefinition()
            {
                if (!(await NavigatorForDialogs.Locate(
                    detailWindow,
                    WorkspaceNames.WorkspaceManagement,
                    WorkspaceNames.UriExtentUserForm) is IElement form))
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

                var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
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
                if (detailWindow.OverridingFormDefinition?.Element == null)
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

                var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
                var userViewExtent = viewLogic.GetUserFormExtent();
                var factory = new MofFactory(userViewExtent);

                viewLogic.RemoveFormAssociationForDetailMetaClass(metaClass);

                var formAssociation = factory.create(_DatenMeister.TheOne.Forms.__FormAssociation);
                formAssociation.set(_DatenMeister._Forms._FormAssociation.metaClass, metaClass);
                formAssociation.set(_DatenMeister._Forms._FormAssociation.form, detailWindow.OverridingFormDefinition.Element);
                formAssociation.set(_DatenMeister._Forms._FormAssociation.formType, _DatenMeister._Forms.___FormType.Detail);
                userViewExtent.elements().add(formAssociation);

                MessageBox.Show("View Association created");
            }

            async void CreateFieldByProperty()
            {
                var navigationHost = viewExtensionInfo.NavigationHost
                                     ?? throw new InvalidOperationException("navigationHost == null");
                
                if (await NavigatorForDialogs.Locate(
                    navigationHost,
                    WorkspaceNames.WorkspaceTypes,
                    WorkspaceNames.UriExtentUserTypes) is IElement locatedItem)
                {
                    var formCreator = GiveMe.Scope.Resolve<FormCreator>();
                    formCreator.AddToFormByUmlElement(
                        detailAsElement!, // !Ok, since this method will only be called when detailAsElement is set 
                        locatedItem,
                        CreationMode.All);
                }
            }

            async void CreateAsDefaultType()
            {
                var navigationHost = viewExtensionInfo.NavigationHost
                                     ?? throw new InvalidOperationException("navigationHost == null");
                var extent = (detailAsElement as IHasExtent)?.Extent;
                if (extent == null)
                    throw new InvalidOperationException("extent == null");

                if (detailAsElement == null)
                    throw new InvalidOperationException("DetailElement not IElement");

                var factory = new MofFactory(extent);
                var container = detailAsElement.container();
                var isPackage = container != null && DefaultClassifierHints.IsPackageLike(container);

                var formAssociation = factory.create(_DatenMeister.TheOne.Forms.__FormAssociation);

                DefaultClassifierHints.AddToExtentOrElement(
                    isPackage && container != null ? container : (IObject) extent,
                    formAssociation);
                
                formAssociation.set(
                    _DatenMeister._Forms._FormAssociation.name, 
                    "Detail Association for " + NamedElementMethods.GetName(detailAsElement));

                var formMetaClass = detailAsElement.metaclass;
                var isDetailForm = ClassifierMethods.IsSpecializedClassifierOf(formMetaClass, _DatenMeister.TheOne.Forms.__DetailForm);
                var isExtentForm = ClassifierMethods.IsSpecializedClassifierOf(formMetaClass, _DatenMeister.TheOne.Forms.__ExtentForm);
                var formType = 
                    isExtentForm ? _DatenMeister._Forms.___FormType.TreeItemDetail :
                    isDetailForm ? _DatenMeister._Forms.___FormType.Detail :
                    _DatenMeister._Forms.___FormType.TreeItemExtent;

                formAssociation.set(_DatenMeister._Forms._FormAssociation.formType, formType);
                formAssociation.set(_DatenMeister._Forms._FormAssociation.form, detailAsElement);

                await NavigatorForItems.NavigateToElementDetailView(navigationHost, formAssociation);
            }
        }
    }
}