using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.Extents;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.ZipCodeExample;
using DatenMeister.Provider.ExtentManagement;
using DatenMeister.Types;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Helper;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.GuiElements;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Forms.Lists
{
    public static class WorkspaceExtentFormGenerator
    {
        /// <summary>
        /// Defines the logger for the class
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(WorkspaceExtentFormGenerator));

        /// <summary>
        /// Requests the form for the workspace
        /// </summary>
        /// <returns>Requested form</returns>
        internal static FormDefinition RequestFormForWorkspaces(IExtent extent, INavigationHost navigationHost)
        {
            // Finds the view
            var formsPlugin = GiveMe.Scope.Resolve<FormMethods>();
            var formFactory = GiveMe.Scope.Resolve<FormFactory>();

            var formElement = formsPlugin.GetInternalFormExtent()
                .element($"#{ManagementViewDefinitions.IdWorkspaceListView}");

            if (formElement == null)
            {
                // The form was not found, so the form is created automatically
                // Creates the form out of the properties of the workspace
                formElement = formFactory.CreateListFormForCollection(
                                  extent.elements(),
                                  new FormFactoryConfiguration())
                              ?? throw new InvalidOperationException("List form could not be created");
                formElement.set(_DatenMeister._Forms._TableForm.inhibitNewItems, true);
            }

            var formDefinition = new FormDefinition("Workspaces", formElement)
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

            formDefinition.ViewExtensions.Add(
                new ItemMenuButtonDefinition(
                    "Add Workspace",
                    NewWorkspace,
                    "workspaces-new",
                    NavigationCategories.DatenMeister + "." + "Workspaces"));

            formDefinition.ViewExtensions.Add(
                new ItemMenuButtonDefinition(
                    "Open Workspace-Folder",
                    x => NavigatorForWorkspaces.OpenFolder(navigationHost),
                    null,
                    NavigationCategories.DatenMeister + ".Workspaces"));

            formDefinition.ViewExtensions.Add(
                new ItemMenuButtonDefinition(
                    "Reset DatenMeister",
                    x => NavigatorForWorkspaces.ResetDatenMeister(navigationHost),
                    null,
                    NavigationCategories.DatenMeister + ".Workspaces"));

            return formDefinition;

            void NewWorkspace(IObject workspaceObject)
            {
                _ = NavigatorForWorkspaces.CreateNewWorkspace(navigationHost);
            }

            void ShowExtents(INavigationGuest navigationGuest, IObject workspace)
            {
                var workspaceId = workspace.getOrDefault<string>("id");
                if (workspaceId == null)
                {
                    return;
                }

                NavigatorForExtents.NavigateToExtentList(navigationGuest.NavigationHost, workspaceId);
            }

            void DeleteWorkspace(INavigationGuest navigationGuest, IObject workspace)
            {
                var workspaceId = workspace.getOrDefault<string>("id");

                if (MessageBox.Show(
                        $"Are you sure to delete the workspace '{workspaceId}'? " +
                        "All included extents will also be deleted.",
                        "Confirmation",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
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
        internal static FormDefinition RequestFormForExtents(
            IExtent extent,
            string workspaceId,
            INavigationHost navigationHost)
        {
            var viewLogic = GiveMe.Scope.Resolve<FormMethods>();
            var formFactory = GiveMe.Scope.Resolve<FormFactory>();
            var viewExtent = viewLogic.GetInternalFormExtent();
            var result =
                viewExtent.GetUriResolver().ResolveById("Workspace.ExtentFormForExtents");

            if (result == null)
            {
                result =
                    formFactory.CreateListFormForCollection(
                        extent.elements(),
                        new FormFactoryConfiguration()) ??
                    throw new InvalidOperationException("listForm == null");
                result.set(_DatenMeister._Forms._TableForm.inhibitDeleteItems, true);
                result.set(_DatenMeister._Forms._TableForm.inhibitNewItems, true);
                result.set(_DatenMeister._Forms._TableForm.property,
                    nameof(_DatenMeister._Management._Workspace.extents));
            }

            var viewDefinition = new FormDefinition("Extents", result)
            {
                TabViewExtensionsFunction = form => new List<ViewExtension>
                {
                    new RowItemButtonDefinition("Show Items", ShowItems, ItemListViewControl.ButtonPosition.Before),
                    new RowItemButtonDefinition("Save", SaveExtent),
                    new RowItemButtonDefinition("Delete", DeleteExtent),
                    new RowItemButtonDefinition(
                        "Edit Properties",
                        async (navigationGuest, element) =>
                        {
                            var extentUrl = element.getOrDefault<string>(_DatenMeister._Management._Extent.uri);
                            var foundExtent = GiveMe.Scope.WorkspaceLogic.FindExtent(workspaceId, extentUrl);
                            if (foundExtent == null)
                            {
                                MessageBox.Show("No extent is found");
                            }
                            else
                            {
                                await NavigatorForExtents.OpenPropertiesOfExtent(
                                    navigationGuest.NavigationHost,
                                    foundExtent);
                            }
                        })
                }
            };

            viewDefinition.ViewExtensions.Add(
                new ItemMenuButtonDefinition(
                    "Create Xmi Extent",
                    NewXmiExtent,
                    null,
                    NavigationCategories.DatenMeister + ".Extent"));

            viewDefinition.ViewExtensions.Add(
                new ItemMenuButtonDefinition(
                    "Zip-Code Example",
                    AddZipCodeExample,
                    null,
                    NavigationCategories.DatenMeister + ".Extent"));

            viewDefinition.ViewExtensions.Add(
                new ItemMenuButtonDefinition(
                    "Import Excel",
                    ImportFromExcel,
                    Icons.ImportExcel,
                    NavigationCategories.DatenMeister + ".Import"));

            viewDefinition.ViewExtensions.Add(
                new ItemMenuButtonDefinition(
                    "Load Extent",
                    async x => await LoadExtentFromXmi(workspaceId, navigationHost, viewExtent),
                    Icons.ImportExcel,
                    NavigationCategories.DatenMeister + ".Extent"));

            viewDefinition.ViewExtensions.Add(
                new ItemMenuButtonDefinition(
                    "Create Extent",
                    LoadExtent,
                    Icons.ImportExcel,
                    NavigationCategories.DatenMeister + ".Extent"));

            viewDefinition.ViewExtensions.Add(
                new InfoLineDefinition(() =>
                    new TextBlock
                    {
                        Inlines =
                        {
                            new Bold { Inlines = { new Run("Workspace: ") } },
                            new Run(workspaceId)
                            {
                                ContextMenu = ItemListViewControl.GetCopyToClipboardContextMenu(workspaceId)
                            }
                        }
                    }));

            void DeleteExtent(INavigationGuest guest, IObject element)
            {
                var extentObject = (element as MofElement)?.ProviderObject as ExtentObject;
                var uri = element.getOrDefault<string>("uri");
                if (MessageBox.Show(
                        $"Are you sure, you would like to delete the extent '{uri}'?",
                        "Delete Extent",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
                    var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();

                    var extentToBeDeleted =
                        workspaceLogic.FindExtent(workspaceId, uri);
                    if (extentToBeDeleted != null)
                    {
                        extentManager.RemoveExtent(extentToBeDeleted);
                    }

                    if (extentObject?.LoadedExtentInformation != null)
                    {
                        extentManager.RemoveExtent(extentObject.LoadedExtentInformation);
                    }

                    guest.UpdateForm();
                }
            }

            async void ImportFromExcel(IObject collection)
            {
                await NavigatorForExcelHandling.ImportFromExcel(navigationHost, workspaceId);
            }

            void NewXmiExtent(IObject item)
            {
                _ = NavigatorForExtents.NavigateToNewXmiExtentDetailView(navigationHost, workspaceId);
            }

            void AddZipCodeExample(IObject item)
            {
                var zipCodeExampleManager = GiveMe.Scope.Resolve<ZipCodeExampleManager>();
                zipCodeExampleManager.AddZipCodeExample(workspaceId);
            }

            return viewDefinition;

            void ShowItems(INavigationGuest navigationGuest, IObject extentElement)
            {
                var listViewControl = (ItemListViewControl)navigationGuest;

                var uri = extentElement.getOrDefault<string>("uri");

                _ = NavigatorForItems.NavigateToItemsInExtent(
                    listViewControl.NavigationHost,
                    workspaceId,
                    uri);
            }

            async void LoadExtent(IObject? item)
            {
                var extentLoaderConfig =
                    await QueryExtentConfigurationByUserAsync(navigationHost);
                if (extentLoaderConfig != null)
                {
                    var extentManager = GiveMe.Scope.Resolve<ExtentManager>();

                    try
                    {
                        var loadedExtent = extentManager.LoadExtent(
                            extentLoaderConfig,
                            ExtentCreationFlags.LoadOrCreate);
                        if (loadedExtent.LoadingState == ExtentLoadingState.Failed)
                        {
                            Logger.Info($"Extent could not be created: {loadedExtent.FailLoadingMessage}");
                            MessageBox.Show($"Extent could not be created: {loadedExtent.FailLoadingMessage}");
                        }
                        else if (loadedExtent.LoadingState == ExtentLoadingState.Loaded
                                 && loadedExtent.Extent != null)
                        {
                            Logger.Info($"User created extent via general dialog: {loadedExtent.Extent.contextURI()}");
                        }
                    }
                    catch (Exception exc)
                    {
                        Logger.Warn($"User failed to create extent via general dialog: {exc.Message}");
                        MessageBox.Show(exc.Message);
                    }
                }
            }

            void SaveExtent(INavigationGuest navigationGuest, IObject item)
            {
                var uri = item.getOrDefault<string>(nameof(_DatenMeister._Management._Extent.uri));
                var storeExtent = GiveMe.Scope.WorkspaceLogic.FindExtent(workspaceId, uri);

                var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
                if (storeExtent != null)
                {
                    extentManager.StoreExtent(storeExtent);

                    MessageBox.Show("Extent saved");
                    navigationGuest.UpdateForm();
                }
            }
        }

        private static async Task LoadExtentFromXmi(string workspaceId, INavigationHost navigationHost,
            IUriExtent viewExtent)
        {
            try
            {
                var localTypeSupport = GiveMe.Scope.Resolve<LocalTypeSupport>();
                var foundType =
                    localTypeSupport.InternalTypes.element("#DatenMeister.Models.ExtentManager.ImportSettings")
                    ?? throw new InvalidOperationException(
                        "DatenMeister.Models.ExtentManager.ImportSettings is not found");

                var userResult = InMemoryObject.CreateEmpty(foundType);
                userResult.set("workspace", workspaceId);
                var foundForm = viewExtent.element("#OpenExtentAsFile")
                                ?? throw new InvalidOperationException("#OpenExtentAsFile not found");
                var navigationResult =
                    await Navigator.CreateDetailWindow(navigationHost,
                        new NavigateToItemConfig
                        {
                            DetailElement = userResult,
                            Form = new FormDefinition(foundForm),
                            PropertyValueChanged = OnPropertyValueChanged
                        });

                if (navigationResult?.Result == NavigationResult.Saved && navigationResult.DetailElement != null)
                {
                    // Load from extent import class
                    var extentImport = GiveMe.Scope.Resolve<ExtentImport>();
                    extentImport.ImportExtent(navigationResult.DetailElement);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }

            // Defines the method to be called when the property value of 
            // the item within the detail form has been changed
            static void OnPropertyValueChanged(PropertyValueChangedEventArgs prop)
            {
                if (prop.PropertyName != "filePath") return;

                var filePath = prop.NewValue?.ToString();
                try
                {
                    if (filePath == null || string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    {
                        return;
                    }

                    var metaNode = XmiProvider.GetMetaNodeFromFile(filePath);
                    var uri = metaNode.Attribute("__uri")?.Value;
                    if (uri != null && !string.IsNullOrEmpty(uri))
                    {
                        prop.FormControl.InjectPropertyValue("extentUri", uri);
                    }
                }
                catch (Exception e)
                {
                    Logger.Warn("Error after selecting an extent: " + e.Message);
                }
            }
        }

        public static async Task<IElement?> QueryExtentConfigurationByUserAsync(INavigationHost navigationHost)
        {
            // Let user select the type of the extent
            var dlg = new LocateItemDialog
            {
                ShowWorkspaceSelection = false,
                ShowExtentSelection = false,
                MessageText = "Select type of extent",
                Title = "Select type of extent",
                Owner = navigationHost.GetWindow()
            };

            var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
            var extent =
                workspaceLogic.FindExtent(WorkspaceNames.WorkspaceTypes, WorkspaceNames.UriExtentInternalTypes);
            if (extent == null)
            {
                Logger.Error("Extent for Types is not found");
                return null;
            }

            var packageMethods = GiveMe.Scope.Resolve<PackageMethods>();
            var package =
                packageMethods.GetPackageStructure(
                    extent.elements(),
                    ExtentManager.PackagePathTypesExtentLoaderConfig);
            if (package == null)
            {
                throw new InvalidOperationException(ExtentManager.PackagePathTypesExtentLoaderConfig + " not found");
            }

            dlg.SetAsRoot(extent /*package*/);

            // User has selected the type
            if (dlg.ShowDialog() != true) return null;
            if (!(dlg.SelectedElement is IElement selectedExtentType)) return null;

            // Create the item
            var factory = new MofFactory(extent);
            var createdElement = factory.create(selectedExtentType);

            // Let user fill out the configuration of the extent
            var detailControl = await NavigatorForItems.NavigateToElementDetailView(navigationHost, createdElement);

            if (detailControl != null && detailControl.Result == NavigationResult.Saved)
            {
                // Convert back to instance
                return createdElement;
            }

            return null;
        }
    }
}