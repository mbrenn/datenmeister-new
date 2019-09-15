using System.Collections.Generic;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Modules.ViewManager
{
    /// <summary>
    /// Contains the factory for the view extensions
    /// </summary>
    public class ViewManagerViewExtensionFactory : IViewExtensionFactory
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
            var detailFormControl = viewExtensionTargetInformation.NavigationHost as DetailFormWindow;

            if (viewExtensionTargetInformation.NavigationHost != null)
            {
                var result = new ApplicationMenuButtonDefinition(
                    "View User Views",
                    () => Navigation.NavigatorForItems.NavigateToItemsInExtent(
                        viewExtensionTargetInformation.NavigationHost,
                        WorkspaceNames.NameManagement,
                        WorkspaceNames.UriUserViewExtent),
                    string.Empty,
                    "File.Navigation");

                yield return result;
            }

            if (itemExplorerControl != null || detailFormControl != null)
            {
                var openView = new ApplicationMenuButtonDefinition(
                    "Change Form",
                    async () =>
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
                                itemExplorerControl.Items,
                                formDefinition,
                                null);

                            detailFormControl?.SetForm(formDefinition);
                        }
                    },
                    "",
                    NavigationCategories.Views + ".Forms");

                yield return openView;

                if (itemExplorerControl != null)
                {
                    var showFormDefinition = new ApplicationMenuButtonDefinition(
                        "Show Form Definition",
                        () =>
                        {
                            var dlg = new ItemXmlViewWindow
                            {
                                /*SupportWriting = true,*/
                                Owner = Window.GetWindow(itemExplorerControl.NavigationHost.GetWindow())
                            };
                            dlg.SupportWriting = false;

                            dlg.UpdateContent(itemExplorerControl.EffectiveForm);

                            dlg.ShowDialog();

                        },
                        "",
                        NavigationCategories.Views + ".Forms");

                    yield return showFormDefinition;

                    var copyFormDefinition = new ApplicationMenuButtonDefinition(
                        "Save Form Definition",
                        () =>
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
                        NavigationCategories.Views + ".Forms");

                    yield return copyFormDefinition;
                }
            }
        }
    }
}