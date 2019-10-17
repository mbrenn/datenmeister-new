using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Modules.ViewFinder.Helper;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
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
    public class ViewManagerViewExtension : IViewExtensionFactory
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
            var itemExplorerControl = navigationGuest as ItemExplorerControl;
            var detailFormControl = viewExtensionTargetInformation.NavigationGuest as DetailFormControl;

            if (viewExtensionTargetInformation.NavigationHost is IApplicationWindow)
            {
                yield return GetForApplicationWindow(viewExtensionTargetInformation);
            }

            if (itemExplorerControl != null || detailFormControl != null)
            {
                if (detailFormControl != null)
                {
                    foreach (var viewExtension in
                        GetForDetailWindow(viewExtensionTargetInformation, itemExplorerControl, detailFormControl))
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
        }

        private static IEnumerable<ViewExtension> GetForItemExplorerControl(
            ItemExplorerControl itemExplorerControl)
        {
            var showFormDefinition = new ExtentMenuButtonDefinition(
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

            yield return showFormDefinition;

            var copyFormDefinition = new ExtentMenuButtonDefinition(
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

            yield return copyFormDefinition;

            var changeFormDefinition = new ExtentMenuButtonDefinition(
                "Change Form Definition",
                x =>
                {
                    var form = NavigatorForDialogs.Locate(
                        itemExplorerControl.NavigationHost,
                        WorkspaceNames.NameManagement,
                        WorkspaceNames.UriUserViewExtent);
                    
                    itemExplorerControl.SetOverridingForm(form);
                },
                "",
                NavigationCategories.Form + ".Definition");

            yield return changeFormDefinition;

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

        private static IEnumerable<ViewExtension> GetForDetailWindow(ViewExtensionTargetInformation viewExtensionTargetInformation,
            ItemExplorerControl itemExplorerControl, DetailFormControl detailFormControl)
        {
            var openView = new ExtentMenuButtonDefinition(
                "Change Form",
                async x =>
                {
                    var action = await Navigator.CreateDetailWindow(
                        viewExtensionTargetInformation.NavigationHost,
                        new NavigateToItemConfig
                        {
                            DetailElement = InMemoryObject.CreateEmpty(),
                            FormDefinition = GiveMe.Scope.WorkspaceLogic.GetInternalViewsExtent()
                                .element("#ViewManagerFindView")
                        });

                    if (action.Result == NavigationResult.Saved && action.DetailElement is IElement asElement)
                    {
                        var formDefinition = asElement.getOrDefault<IElement>("form");

                        itemExplorerControl?.AddTab(
                            itemExplorerControl.RootItem,
                            formDefinition,
                            null);

                        detailFormControl.SetForm(formDefinition);
                    }
                },
                "",
                NavigationCategories.Form + ".Definition");

            yield return openView;
        }

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