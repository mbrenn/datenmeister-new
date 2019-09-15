using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Lists
{
    public class WorkspaceList : ItemExplorerControl
    {
        public WorkspaceList()
        {
            Loaded += WorkspaceList_Loaded;
        }

        private void WorkspaceList_Loaded(object sender, RoutedEventArgs e)
        {
            _extent = ManagementProviderHelper.GetExtentsForWorkspaces(GiveMe.Scope);
            SetItems(_extent.elements());

            var eventManager = GiveMe.Scope.Resolve<ChangeEventManager>();
            EventHandle = eventManager.RegisterFor(_extent, (x,y) =>
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
                view = ListRequests.RequestFormForExtents(_extent, workspaceId, NavigationHost);
            }
            else
            {
                view = ListRequests.RequestFormForWorkspaces(_extent, NavigationHost);
            }

            PrepareNavigation(view);

            // Sets the workspaces
            EvaluateForm(SelectedItems, view);
        }

        /// <summary>
        /// Prepares the navigation 
        /// </summary>
        /// <param name="viewDefinition">Definition of the view</param>
        private void PrepareNavigation(ViewDefinition viewDefinition)
        {
            void NewWorkspace()
            {
                _ = NavigatorForWorkspaces.CreateNewWorkspace(NavigationHost);
            }

            viewDefinition.ViewExtensions.Add(
                new ApplicationMenuButtonDefinition(
                    "Add Workspace",
                    NewWorkspace,
                    "workspaces-new",
                    NavigationCategories.DatenMeister + "." + "Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new ApplicationMenuButtonDefinition(
                    "Open Workspace-Folder",
                    () => NavigatorForWorkspaces.OpenFolder(NavigationHost),
                    null,
                    NavigationCategories.DatenMeister + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new ApplicationMenuButtonDefinition(
                    "Reset DatenMeister",
                    () => NavigatorForWorkspaces.ResetDatenMeister(NavigationHost),
                    null,
                    NavigationCategories.DatenMeister + ".Workspaces"));

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