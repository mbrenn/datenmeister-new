using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Forms.Detail;
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
        }

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        protected override void OnRecreateViews()
        {
            ViewDefinition view;
            
            var selectedItemMetaClass = (SelectedPackage as IElement)?.getMetaClass();
            Action<ItemExplorerTab> afterAction;
            if (selectedItemMetaClass != null
                && NamedElementMethods.GetFullName(selectedItemMetaClass)?.Contains("Workspace") == true)
            {
                view = ListRequests.RequestFormForExtents();
                afterAction = x => ListRequests.AddButtonsForExtents(x.Control, SelectedPackage.get("id")?.ToString());
            }
            else
            {
                view = ListRequests.RequestFormForWorkspaces();
                afterAction = x => ListRequests.AddButtonsForWorkspaces(x.Control);
            }

            PrepareNavigation(view);

            // Sets the workspaces
            var element = AddTab(
                SelectedItems,
                view);
            afterAction(element);
        }

        /// <summary>
        /// Prepares the navigation 
        /// </summary>
        /// <param name="viewDefinition">Definition of the view</param>
        private void PrepareNavigation(ViewDefinition viewDefinition)
        {
            base.PrepareNavigation();
            void NewWorkspace()
            {
                var events = NavigationHost.NavigateTo(() =>
                    {
                        var dlg = new NewWorkspaceControl();
                        dlg.SetContent();
                        return dlg;
                    },
                    NavigationMode.Detail);

                events.Closed += (x, y) => RecreateViews();
            }

            void JumpToTypeManager()
            {
                NavigatorForItems.NavigateToItemsInExtent(
                    NavigationHost,
                    WorkspaceNames.NameTypes,
                    WorkspaceNames.UriUserTypesExtent);
            }

            viewDefinition.ExtendedProperties.Add(
                new RibbonButtonDefinition(
                    "Add Workspace",
                    NewWorkspace,
                    "workspaces-new",
                    NavigationCategories.File + "." + "Workspaces"));

            viewDefinition.ExtendedProperties.Add(
                new RibbonButtonDefinition(
                    "Type Manager",
                    JumpToTypeManager,
                    string.Empty,
                    NavigationCategories.Type + "." + "Manager"
                ));

            viewDefinition.ExtendedProperties.Add(
                new RibbonButtonDefinition(
                    "Open Workspace-Folder",
                    () => NavigatorForWorkspaces.OpenFolder(NavigationHost),
                    null,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ExtendedProperties.Add(
                new RibbonButtonDefinition(
                    "Reset DatenMeister",
                    () => NavigatorForWorkspaces.ResetDatenMeister(NavigationHost),
                    null,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ExtendedProperties.Add(
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