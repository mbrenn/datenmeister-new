using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Detail;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class WorkspaceList : ListViewControl, INavigationGuest
    {
        public WorkspaceList()
        {
            Loaded += WorkspaceList_Loaded;
        }

        private void WorkspaceList_Loaded(object sender, RoutedEventArgs e)
        {
            SetContent();
        }

        protected override IElement RequestFormOverride(IElement selectedForm)
        {
            var selectedItemMetaClass = (DetailItem as IElement)?.getMetaClass();
            if (selectedItemMetaClass != null
                && NamedElementMethods.GetFullName(selectedItemMetaClass)?.Contains("Workspace") == true)
            {
                return ListRequests.RequestFormForExtents(this, DetailItem.get("id")?.ToString());
            }
            else
            {
                return ListRequests.RequestFormForWorkspaces(this);
            }
        }

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        public void SetContent()
        {
            // Sets the workspaces
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(App.Scope);
            SetContent(workspaceExtent.elements());
        }

        public new void PrepareNavigation()
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

                events.Closed += (x, y) => UpdateContent();
            }

            void JumpToTypeManager()
            {
                NavigatorForItems.NavigateToItemsInExtent(
                    NavigationHost,
                    WorkspaceNames.NameTypes,
                    WorkspaceNames.UriUserTypesExtent);
            }
            
            NavigationHost.AddNavigationButton(
                "Add Workspace",
                NewWorkspace,
                "workspaces-new",
                NavigationCategories.File + "." + "Workspaces");

            NavigationHost.AddNavigationButton(
                "Type Manager",
                JumpToTypeManager,
                string.Empty,
                NavigationCategories.Type + "." + "Manager"
            );

            NavigationHost.AddNavigationButton(
                "Open Workspace-Folder",
                () => NavigatorForWorkspaces.OpenFolder(NavigationHost),
                null,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Reset DatenMeister",
                () => NavigatorForWorkspaces.ResetDatenMeister(NavigationHost),
                null,
                NavigationCategories.File + ".Workspaces");

            AddInfoLine(
                new TextBlock
                {
                    Inlines =
                    {
                        new Bold {Inlines = {new Run("All Workspaces")}}
                    }
                });
        }

        public override void OnMouseDoubleClick(IObject element)
        {
            var workspaceId = element.get("id").ToString();
            NavigatorForExtents.NavigateToExtentList(NavigationHost, workspaceId);
        }
    }
}