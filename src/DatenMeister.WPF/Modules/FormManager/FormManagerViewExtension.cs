#nullable enable

using System;
using System.Collections.Generic;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
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
            var navigationHost = itemExplorerControl.NavigationHost ??
                                 throw new InvalidOperationException("navigationHost == null");
            
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

                _ = NavigatorForItems.NavigateToElementDetailView(
                    navigationHost,
                    createdForm);
            }
        }

        /// <summary>
        /// Gets the navigation for the application window
        /// </summary>
        /// <param name="viewExtensionTargetInformation"></param>
        /// <returns></returns>
        private static ViewExtension GetForApplicationWindow(
            ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            var navigationHost = viewExtensionTargetInformation.NavigationHost ??
                                 throw new InvalidOperationException("navigationHost == null");
            
            var result = new ApplicationMenuButtonDefinition(
                "Goto User Forms", async () => await NavigatorForItems.NavigateToItemsInExtent(
                    navigationHost,
                    WorkspaceNames.NameManagement,
                    WorkspaceNames.UriUserFormExtent),
                string.Empty,
                NavigationCategories.DatenMeisterNavigation);

            return result;
        }
    }
}