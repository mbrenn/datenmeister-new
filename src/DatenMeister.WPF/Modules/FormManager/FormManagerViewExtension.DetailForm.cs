#nullable enable

using System;
using System.Collections.Generic;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
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
            var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Require<_FormAndFields>()
                                ?? throw new InvalidOperationException("No _FormAndFields in Type Workspace");
            
            // Checks for specific elements
            var detailAsElement = detailWindow.DetailElement as IElement;
            var metaClassOfDetailElement = detailAsElement?.getMetaClass();
            
            yield return new ApplicationMenuButtonDefinition(
                "Change Form Definition",
                ChangeFormDefinition,
                null,
                NavigationCategories.Form + ".Current");

            yield return new ApplicationMenuButtonDefinition(
                "Reset Form Definition",
                ClearOverridingForm,
                null,
                NavigationCategories.Form +".Current");

            yield return new ApplicationMenuButtonDefinition(
                "Set as default for Metaclass",
                SetAsDefaultForMetaclass,
                string.Empty,
                NavigationCategories.Form+".Current"
            );

            yield return new ApplicationMenuButtonDefinition(
                "Clear default Association",
                ClearAssociation,
                null,
                NavigationCategories.Form+".Current");
            

            yield return new ApplicationMenuButtonDefinition(
                "Autogenerate form",
                detailWindow.ForceAutoGenerationOfForm,
                string.Empty,
                NavigationCategories.Form + ".Current");


            // The currently selected element is a form
            if (ClassifierMethods.IsSpecializedClassifierOf(metaClassOfDetailElement, formAndFields.__Form)
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

            void ChangeFormDefinition()
            {
                if (!(NavigatorForDialogs.Locate(
                    detailWindow,
                    WorkspaceNames.NameManagement,
                    WorkspaceNames.UriUserFormExtent) is IElement form))
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

                var formAssociation = factory.create(formAndFields.__FormAssociation);
                formAssociation.set(_FormAndFields._FormAssociation.metaClass, metaClass);
                formAssociation.set(_FormAndFields._FormAssociation.form, detailWindow.OverridingFormDefinition.Element);
                formAssociation.set(_FormAndFields._FormAssociation.formType, FormType.Detail);
                userViewExtent.elements().add(formAssociation);

                MessageBox.Show("View Association created");
            }

            async void CreateFieldByProperty()
            {
                var navigationHost = viewExtensionInfo.NavigationHost
                                     ?? throw new InvalidOperationException("navigationHost == null");
                
                if (await NavigatorForDialogs.Locate(
                    navigationHost,
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

            async void CreateAsDefaultType()
            {
                var navigationHost = viewExtensionInfo.NavigationHost
                                     ?? throw new InvalidOperationException("navigationHost == null");
                var defaultTypeClassifierHints = new DefaultClassifierHints(GiveMe.Scope.WorkspaceLogic);
                var extent = (detailAsElement as IHasExtent)?.Extent;
                if (extent == null)
                    throw new InvalidOperationException("extent == null");

                if (detailAsElement == null)
                    throw new InvalidOperationException("DetailElement not IElement");

                var factory = new MofFactory(extent);
                var container = detailAsElement.container();
                var isPackage = container != null && defaultTypeClassifierHints.IsPackageLike(container);

                var formAssociation = factory.create(formAndFields.__FormAssociation);

                defaultTypeClassifierHints.AddToExtentOrElement(
                    isPackage && container != null ? container : (IObject) extent,
                    formAssociation);
                
                formAssociation.set(
                    _FormAndFields._FormAssociation.name, 
                    "Detail Association for " + NamedElementMethods.GetName(detailAsElement));

                var formMetaClass = detailAsElement.metaclass;
                var isDetailForm = ClassifierMethods.IsSpecializedClassifierOf(formMetaClass, formAndFields.__DetailForm);
                var isExtentForm = ClassifierMethods.IsSpecializedClassifierOf(formMetaClass, formAndFields.__ExtentForm);
                var formType = 
                    isExtentForm ? FormType.TreeItemDetail :
                    isDetailForm ? FormType.Detail :
                    FormType.TreeItemExtent;

                formAssociation.set(_FormAndFields._FormAssociation.formType, formType);
                formAssociation.set(_FormAndFields._FormAssociation.form, detailAsElement);

                await NavigatorForItems.NavigateToElementDetailView(navigationHost, formAssociation);
            }
        }
    }
}