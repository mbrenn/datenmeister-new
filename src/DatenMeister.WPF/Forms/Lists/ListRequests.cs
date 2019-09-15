using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Specific;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Forms.Lists
{
    public static class ListRequests
    {
        /// <summary>
        /// Defines the logger for the class
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(ListRequests));

        /// <summary>
        /// Requests the form for the workspace
        /// </summary>
        /// <returns>Requested form</returns>
        internal static ViewDefinition RequestFormForWorkspaces(IExtent extent, INavigationHost navigationHost)
        {
            // Finds the view
            var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
            var formElement = viewLogic.GetInternalViewExtent().element($"#{ManagementViewDefinitions.IdWorkspaceListView}");
            /*var formElement = NamedElementMethods.GetByFullName(
                viewLogic.GetInternalViewExtent(),
                ManagementViewDefinitions.PathWorkspaceListView);*/

            if (formElement == null)
            {
                // The form was not found, so the form is created automatically
                // Creates the form out of the properties of the workspace
                var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Get<_ManagementProvider>();
                formElement = viewLogic.GetExtentFormForSubforms(
                    viewLogic.GetListFormForExtent(extent, formAndFields.__Workspace,
                        ViewDefinitionMode.Default));
            }

            var viewDefinition = new ViewDefinition("Workspaces", formElement)
            {
                TabViewExtensionsFunction = form =>
                {
                    var result = new List<ViewExtension>
                    {
                        new RowItemButtonDefinition(
                            "Show Extents",
                            ShowExtents,
                            ItemListViewControl.ButtonPosition.Before),
                        new RowItemButtonDefinition(
                            "Delete Workspace",
                            DeleteWorkspace)
                    };

                    return result;
                }
            };

            viewDefinition.ViewExtensions.Add(
                new TreeViewItemCommandDefinition(
                    "New Workspace",
                    (x) => { _ = NavigatorForWorkspaces.CreateNewWorkspace(navigationHost); }));

            return viewDefinition;

            void ShowExtents(INavigationGuest navigationGuest, IObject workspace)
            {
                var workspaceId = workspace.get("id")?.ToString();
                if (workspaceId == null)
                {
                    return;
                }

                NavigatorForExtents.NavigateToExtentList(navigationGuest.NavigationHost, workspaceId);
            }

            void DeleteWorkspace(INavigationGuest navigationGuest, IObject workspace)
            {
                if (MessageBox.Show(
                        "Are you sure to delete the workspace? All included extents will also be deleted.", "Confirmation",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var workspaceId = workspace.get("id").ToString();

                    var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
                    workspaceLogic.RemoveWorkspace(workspaceId);
                }
            }
        }

        /// <summary>
        /// Requests the form for extent elements
        /// </summary>
        /// <param name="extent">Extent being used</param>
        /// <param name="workspaceId">The Id of the workspace</param>
        /// <param name="navigationHost">Defines the navigation host being used for the window</param>
        /// <returns>The created form</returns>
        internal static ViewDefinition RequestFormForExtents(IExtent extent, string workspaceId, INavigationHost navigationHost)
        {
            var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
            var viewExtent = viewLogic.GetInternalViewExtent();
            var result =
                NamedElementMethods.GetByFullName(
                    viewExtent,
                    ManagementViewDefinitions.PathExtentListView);

            if (result == null)
            {
                // result = viewLogic.GetExtentForm(control.Items, ViewDefinitionMode.Default);
                var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Get<_ManagementProvider>();
                result = viewLogic.GetExtentFormForSubforms(
                    viewLogic.GetListFormForExtent(extent, formAndFields.__Extent,
                        ViewDefinitionMode.Default));
            }

            var viewDefinition = new ViewDefinition("Extents", result)
            {
                TabViewExtensionsFunction = (form) => new List<ViewExtension>
                {
                    new RowItemButtonDefinition("Show Items", ShowItems, ItemListViewControl.ButtonPosition.Before),
                    new RowItemButtonDefinition("Delete", DeleteExtent)
                }
            };

            viewDefinition.ViewExtensions.Add(
                new TreeViewItemCommandDefinition(
                    "New Extent",
                    (x) => { LoadExtent(); }));

            viewDefinition.ViewExtensions.Add(
                new ApplicationMenuButtonDefinition(
                    "New Xmi Extent",
                    NewXmiExtent,
                    null,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new ApplicationMenuButtonDefinition(
                    "Zip-Code Example",
                    AddZipCodeExample,
                    null,
                    NavigationCategories.File + ".Workspaces"));

            viewDefinition.ViewExtensions.Add(
                new ApplicationMenuButtonDefinition(
                    "Import Excel",
                    ImportFromExcel,
                    Icons.ImportExcel,
                    NavigationCategories.File + ".Import"));

            viewDefinition.ViewExtensions.Add(
                new ApplicationMenuButtonDefinition(
                    "Import from XMI",
                    ImportFromXmi,
                    Icons.ImportExcel,
                    NavigationCategories.File + ".Import"));

            viewDefinition.ViewExtensions.Add(
                new ApplicationMenuButtonDefinition(
                    "Load Extent",
                    LoadExtent,
                    Icons.ImportExcel,
                    NavigationCategories.File + ".Extent"));

            viewDefinition.ViewExtensions.Add(
                new InfoLineDefinition(() =>
                    new TextBlock
                    {
                        Inlines =
                        {
                            new Bold {Inlines = {new Run("Workspace: ")}},
                            new Run(workspaceId)
                            {
                                ContextMenu = ItemListViewControl.GetCopyToClipboardContextMenu(workspaceId)
                            }
                        }
                    }));

            void DeleteExtent(INavigationGuest guest, IObject element)
            {
                if (MessageBox.Show(
                        "Are you sure, you would like to delete the extent?", 
                        "Delete Extent",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
                    var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();

                    var extentToBeDeleted =
                        workspaceLogic.FindExtent(workspaceId, DotNetHelper.AsString(element.get("uri")));
                    extentManager.DeleteExtent(extentToBeDeleted);
                }
            }

            async void ImportFromExcel()
            {
                await NavigatorForExcelHandling.ImportFromExcel(navigationHost, workspaceId);
            }

            void NewXmiExtent()
            {
                _ = NavigatorForItems.NavigateToNewXmiExtentDetailView(navigationHost, workspaceId);
            }

            void AddZipCodeExample()
            {
                var zipCodeExampleManager = GiveMe.Scope.Resolve<ZipCodeExampleManager>();
                zipCodeExampleManager.AddZipCodeExample(workspaceId);
            }

            void ImportFromXmi()
            {
                var dlg = new ImportExtentDlg
                {
                    Owner = navigationHost.GetWindow(),
                    Workspace = workspaceId
                };

                dlg.Closed += (x, y) =>
                {
                    if (dlg.ImportCommand != null)
                    {
                        var extentImport = GiveMe.Scope.Resolve<ExtentImport>();
                        extentImport.ImportExtent(dlg.ImportCommand);
                    }
                };

                dlg.Show();
            }

            return viewDefinition;
            
            void ShowItems(INavigationGuest navigationGuest, IObject extentElement)
            {
                var listViewControl = (ItemListViewControl) navigationGuest;

                var uri = extentElement.get("uri").ToString();

                NavigatorForItems.NavigateToItemsInExtent(
                    listViewControl.NavigationHost,
                    workspaceId,
                    uri);
            }

            async void LoadExtent()
            {
                var extentLoaderConfig = await QueryExtentConfigurationByUserAsync(navigationHost);
                if (extentLoaderConfig != null)
                {
                    var extentManager = GiveMe.Scope.Resolve<IExtentManager>();

                    try
                    {
                        var loadedExtent = extentManager.LoadExtent(extentLoaderConfig, ExtentCreationFlags.LoadOrCreate);
                        Logger.Info($"User created extent via general dialog: {loadedExtent.contextURI()}");
                    }
                    catch (Exception exc)
                    {
                        Logger.Warn($"User failed to create extent via general dialog: {exc.Message}");
                        MessageBox.Show(exc.Message);
                    }

                }
            }
        }

        public static async Task<ExtentLoaderConfig> QueryExtentConfigurationByUserAsync(INavigationHost navigationHost)
        {
            // Let user select the type of the extent
            var dlg = new LocateItemDialog
            {
                ShowWorkspaceSelection = false,
                ShowExtentSelection = false,
                MessageText = "Select type of extent",
                Title = "Select type of extent"
            };

            var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
            var extent = workspaceLogic.FindExtent(WorkspaceNames.NameTypes, WorkspaceNames.UriInternalTypesExtent);

            var packageMethods = GiveMe.Scope.Resolve<PackageMethods>();
            var package =
                packageMethods.GetPackagedObjects(extent.elements(), ExtentManager.PackagePathTypesExtentLoaderConfig);
            dlg.SetAsRoot(package);

            // User has selected the type 
            if (dlg.ShowDialog() != true) return null;
            if (!(dlg.SelectedElement is IElement selectedExtentType)) return null;

            // Create the item
            var factory = new MofFactory(extent);
            var createdElement = factory.create(selectedExtentType);

            // Let user fill out the configuration of the extent
            var detailControl = await NavigatorForItems.NavigateToElementDetailView(navigationHost, createdElement);

            if (detailControl.Result == NavigationResult.Saved)
            {
                // Convert back to instance
                var extentLoaderConfig =
                    DotNetConverter.ConvertToDotNetObject(createdElement) as ExtentLoaderConfig;
                return extentLoaderConfig;
            }

            return null;
        }
    }
}