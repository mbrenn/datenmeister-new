using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class WorkspaceList : ItemExplorerControl, INavigationGuest
    {
        public WorkspaceList()
        {
            Loaded += WorkspaceList_Loaded;
        }

        private void WorkspaceList_Loaded(object sender, RoutedEventArgs e)
        {
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(App.Scope);
            SetItems(workspaceExtent.elements());

            var eventManager = App.Scope.Resolve<ChangeEventManager>();
            EventHandle = eventManager.RegisterFor(workspaceExtent, (x,y) =>
            {
                Tabs.FirstOrDefault()?.Control.UpdateContent();
            });
        }

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        protected override void OnRecreateViews()
        {
            ViewDefinition view;
            
            var selectedItemMetaClass = (SelectedPackage as IElement)?.getMetaClass();
            if (selectedItemMetaClass != null
                && NamedElementMethods.GetFullName(selectedItemMetaClass)?.Contains("Workspace") == true)
            {
                var workspaceId = SelectedPackage.get("id")?.ToString();
                view = ListRequests.RequestFormForExtents(this, workspaceId);
            }
            else
            {
                view = ListRequests.RequestFormForWorkspaces(NavigationHost);
            }

            PrepareNavigation(view);

            // Sets the workspaces
            AddTab(
                SelectedItems,
                view);
        }

        /// <summary>
        /// Prepares the navigation 
        /// </summary>
        /// <param name="viewDefinition">Definition of the view</param>
        private void PrepareNavigation(ViewDefinition viewDefinition)
        {
            void NewWorkspace()
            {
                NavigatorForWorkspaces.CreateNewWorkspace(NavigationHost);
            }

            void JumpToTypeManager()
            {
                NavigatorForItems.NavigateToItemsInExtent(
                    NavigationHost,
                    WorkspaceNames.NameTypes,
                    WorkspaceNames.UriUserTypesExtent);
            }

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Add Workspace",
                    NewWorkspace,
                    "workspaces-new",
                    NavigationCategories.File + "." + "Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Type Manager",
                    JumpToTypeManager,
                    string.Empty,
                    NavigationCategories.Type + "." + "Manager"
                ));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Open Workspace-Folder",
                    () => NavigatorForWorkspaces.OpenFolder(NavigationHost),
                    null,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Reset DatenMeister",
                    () => NavigatorForWorkspaces.ResetDatenMeister(NavigationHost),
                    null,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new InfoLineDefinition(() => new TextBlock
                {
                    Inlines =
                    {
                        new Bold {Inlines = {new Run("All Workspaces")}}
                    }
                }));
        }

        public override void OnMouseDoubleClick(IObject element)
        {
            var workspaceId = element.get("id").ToString();
            NavigatorForExtents.NavigateToExtentList(NavigationHost, workspaceId);
        }
    }
}