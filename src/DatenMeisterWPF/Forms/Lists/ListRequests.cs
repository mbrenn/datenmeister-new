using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Forms.Specific;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ListRequests
    {
        /// <summary>
        /// Requests the form for the workspace
        /// </summary>
        /// <returns>Requested form</returns>
        internal static ViewDefinition RequestFormForWorkspaces()
        {
            // Finds the view
            var viewLogic = App.Scope.Resolve<ViewLogic>();
            var formElement = NamedElementMethods.GetByFullName(
                viewLogic.GetInternalViewExtent(),
                ManagementViewDefinitions.PathWorkspaceListView);
            var viewDefinition = new ViewDefinition("Workspaces", formElement);

            viewDefinition.ViewExtensions.Add(
                new RowItemButtonDefinition("Show Extents", ShowExtents));
            viewDefinition.ViewExtensions.Add(
                new RowItemButtonDefinition("Delete Workspace", DeleteWorkspace));

            return viewDefinition;

            void ShowExtents(INavigationGuest navigationHost, IObject workspace)
            {
                var workspaceId = workspace.get("id")?.ToString();
                if (workspaceId == null)
                {
                    return;
                }

                var events = NavigatorForExtents.NavigateToExtentList(navigationHost.NavigationHost, workspaceId);

                events.Closed += (x, y) => (navigationHost as ItemListViewControl)?.UpdateContent();
            }

            void DeleteWorkspace(INavigationGuest navigationHost, IObject workspace)
            {
                if (MessageBox.Show(
                        "Are you sure to delete the workspace? All included extents will also be deleted.", "Confirmation",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var workspaceId = workspace.get("id").ToString();

                    var workspaceLogic = App.Scope.Resolve<IWorkspaceLogic>();
                    workspaceLogic.RemoveWorkspace(workspaceId);

                    (navigationHost as ItemListViewControl)?.UpdateContent();
                }
            }
        }

        /// <summary>
        /// Requests the form for extent elements
        /// </summary>
        /// <param name="workspaceId">The Id of the workspace</param>
        /// <returns>The created form</returns>
        internal static ViewDefinition RequestFormForExtents(ItemExplorerControl control, string workspaceId)
        {
            var viewExtent = App.Scope.Resolve<ViewLogic>().GetInternalViewExtent();
            var result =
                NamedElementMethods.GetByFullName(
                    viewExtent,
                    ManagementViewDefinitions.PathExtentListView);

            var viewDefinition = new ViewDefinition("Extents", result);
            viewDefinition.ViewExtensions.Add(new RowItemButtonDefinition("Show Items", ShowItems));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "New Xmi Extent",
                    NewXmiExtent,
                    null,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Zip-Code Example",
                    AddZipCodeExample,
                    null,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Import Excel",
                    ImportFromExcel,
                    Icons.ImportExcel,
                    NavigationCategories.File + ".Import"));

            viewDefinition.ViewExtensions.Add(
                new RibbonButtonDefinition(
                    "Import from XMI",
                    ImportFromXmi,
                    Icons.ImportExcel,
                    NavigationCategories.File + ".Import"));

            viewDefinition.ViewExtensions.Add(
                new RowItemButtonDefinition("Delete", DeleteExtent));

            viewDefinition.ViewExtensions.Add(
                new InfoLineDefinition(() =>
                    new TextBlock
                    {
                        Inlines =
                        {
                            new Bold {Inlines = {new Run("Workspace: ")}},
                            new Run(workspaceId)
                        }
                    }));


            void DeleteExtent(INavigationGuest guest, IObject element)
            {
                if (MessageBox.Show("Are you sure, you would like to delete the extent?", "Delete Extent",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var extentManager = App.Scope.Resolve<ExtentManager>();
                    var workspaceLogic = App.Scope.Resolve<IWorkspaceLogic>();

                    var extentToBeDeleted =
                        workspaceLogic.FindExtent(workspaceId, DotNetHelper.AsString(element.get("uri")));
                    extentManager.DeleteExtent(extentToBeDeleted);
                    control.UpdateAllViews();
                }
            }

            void ImportFromExcel()
            {
                NavigatorForExcelHandling.ImportFromExcel(control.NavigationHost, workspaceId);
                control.UpdateAllViews();
            }

            void NewXmiExtent()
            {
                var events = NavigatorForItems.NavigateToNewXmiExtentDetailView(control.NavigationHost, workspaceId);
                events.Closed += (x, y) => control.UpdateAllViews();
            }

            void AddZipCodeExample()
            {
                var zipCodeExampleManager = App.Scope.Resolve<ZipCodeExampleManager>();
                zipCodeExampleManager.AddZipCodeExample(workspaceId);
                control.UpdateAllViews();
            }

            void ImportFromXmi()
            {
                var dlg = new ImportExtentDlg
                {
                    Owner = control.NavigationHost.GetWindow(),
                    Workspace = workspaceId
                };

                dlg.Closed += (x, y) =>
                {
                    if (dlg.ImportCommand != null)
                    {
                        var extentImport = App.Scope.Resolve<ExtentImport>();
                        extentImport.ImportExtent(dlg.ImportCommand);
                        control.UpdateAllViews();
                    }
                };

                dlg.Show();
            }

            return viewDefinition;
            
            void ShowItems(INavigationGuest navigationGuest, IObject extentElement)
            {
                var listViewControl = navigationGuest as ItemListViewControl;

                var uri = extentElement.get("uri").ToString();

                var events = NavigatorForItems.NavigateToItemsInExtent(
                    listViewControl.NavigationHost,
                    workspaceId,
                    uri);
                events.Closed += (x, y) => listViewControl.UpdateContent();
            }
        }
    }
}