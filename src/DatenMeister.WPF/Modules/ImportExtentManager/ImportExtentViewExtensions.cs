using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ImportExtentManager;

/// <summary>
/// Defines the view extension factory for the Import Extent function
/// </summary>
public class ImportExtentViewExtensions : IViewExtensionFactory
{
    private readonly ImportExtentManagerPlugin _plugin;

    public ImportExtentViewExtensions(ImportExtentManagerPlugin plugin)
    {
        _plugin = plugin;
    }

    public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionInfo viewExtensionInfo)
    {
        foreach (var viewExtension in OfferExtentLoadingForDetail(viewExtensionInfo)) 
            yield return viewExtension;

        foreach (var viewExtension in OfferForExtentOverview(viewExtensionInfo)) 
            yield return viewExtension;
    }

    private IEnumerable<ViewExtension> OfferForExtentOverview(ViewExtensionInfo viewExtensionInfo)
    {
        var itemsInExtentList = viewExtensionInfo.GetItemsInExtentExplorerControl();
        if (itemsInExtentList != null)
        {
            // Adds the import elements
            yield return new ExtentMenuButtonDefinition(
                "Import existing Extent",
                ImportExistingExtent,
                null,
                NavigationCategories.Extents + ".Import");

            yield return new ExtentMenuButtonDefinition(
                "Import by new Extent",
                ImportNewExtent,
                null,
                NavigationCategories.Extents + ".Import");

            yield return new ExtentMenuButtonDefinition(
                "Import from Clipboard",
                null!,
                null,
                NavigationCategories.Extents + ".Import");
        }

        // Imports the existing extent
        async void ImportExistingExtent(IExtent extent)
        {
            if (itemsInExtentList == null)
                return;

            var navigationHost = viewExtensionInfo.NavigationHost
                                 ?? throw new InvalidOperationException("navigationHost == null");

            var controlNavigation = await NavigatorForItems.NavigateToElementDetailView(
                navigationHost,
                new NavigateToItemConfig
                {
                    Form = new FormDefinition(
                        GiveMe.Scope.WorkspaceLogic.GetInternalFormsExtent()
                            .element("#ImportManagerFindExtent")
                        ?? throw new InvalidOperationException("#ImportManagerFindExtent not found"))
                });

            if (controlNavigation != null && controlNavigation.Result == NavigationResult.Saved)
            {
                var detailElement = controlNavigation.DetailElement;

                if (detailElement == null)
                    return;

                var selectedExtent = detailElement.getOrDefault<IElement>("selectedExtent");
                if (selectedExtent == null)
                {
                    MessageBox.Show("No extent selected");
                    return;
                }

                var metaClass = selectedExtent.getMetaClass();
                if (metaClass?.equals(_DatenMeister.TheOne.Management.__Extent) != true)
                {
                    MessageBox.Show("Selected element does not reference an extent");
                    return;
                }

                var workspace = selectedExtent.container();
                if (workspace == null)
                {
                    MessageBox.Show("The extent is not connected to the workspace.");
                    return;
                }

                var workspaceName = workspace.getOrDefault<string>(_DatenMeister._Management._Workspace.id);
                var uri = selectedExtent.getOrDefault<string>(_DatenMeister._Management._Extent.uri);

                // Gets the extent from which the data shall be imported
                var sourceExtent = GiveMe.Scope.WorkspaceLogic.FindExtent(workspaceName, uri);
                if (sourceExtent == null)
                {
                    MessageBox.Show($"Source extent with {uri} is not found. ");
                    return;
                }

                var itemCountBefore = sourceExtent.elements().Count();
                var elements = (itemsInExtentList.RootItem as IExtent)?.elements()
                               ?? throw new InvalidOperationException("elements == null");
                _plugin.PerformImport(sourceExtent, elements);
                var itemCountAfter = sourceExtent.elements().Count();

                MessageBox.Show(
                    $"Import has been performed. {itemCountAfter - itemCountBefore} root elements have been added.");
            }
        }

        async void ImportNewExtent(IExtent extent)
        {
            if (itemsInExtentList == null)
                return;

            var navigationHost = viewExtensionInfo.NavigationHost
                                 ?? throw new InvalidOperationException("navigationHost == null");
            var result = await WorkspaceExtentFormGenerator.QueryExtentConfigurationByUserAsync(
                navigationHost);
            if (result != null)
            {
                // Now, we got the item extent...
                var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
                var loadedExtent = new ExtentStorageData.LoadedExtentInformation(result);
                extentManager.LoadExtentWithoutAdding(result, ref loadedExtent);
                if (loadedExtent != null && loadedExtent.Extent != null &&
                    loadedExtent.LoadingState == ExtentLoadingState.Loaded)
                {
                    var itemCountBefore = loadedExtent.Extent.elements().Count();
                    var elements = (itemsInExtentList.RootItem as IExtent)?.elements()
                                   ?? throw new InvalidOperationException("elements == null");
                    _plugin.PerformImport(loadedExtent.Extent, elements);
                    var itemCountAfter = loadedExtent.Extent.elements().Count();

                    MessageBox.Show(
                        $"Import has been performed. {itemCountAfter - itemCountBefore} root elements have been added.");
                }
                else
                {
                    MessageBox.Show("Extent could not be loaded");
                }
            }
        }
    }

    private static IEnumerable<ViewExtension> OfferExtentLoadingForDetail(ViewExtensionInfo viewExtensionInfo)
    {
        // Creates a html report
        var reportInstance = 
            viewExtensionInfo.IsItemInDetailWindowOfType(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__ExtentLoaderConfig,
                true);
        if (reportInstance != null)
        {
            yield return
                new RowItemButtonDefinition(
                    "Load Extent",
                    async (x, y) =>
                    {
                        var asElement = y as IElement ?? throw new InvalidOperationException("Not an Element");
                        var workspaceLogic = GiveMe.Scope.WorkspaceLogic;
                        var scopeStorage = GiveMe.Scope.ScopeStorage;
                            
                        var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
                        var result = await extentManager.LoadExtent(asElement, ExtentCreationFlags.LoadOrCreate);

                        if (result.LoadingState == ExtentLoadingState.Loaded)
                        {
                            MessageBox.Show("The loader config has been executed");
                        }
                        else if (result.LoadingState == ExtentLoadingState.Failed)
                        {
                            MessageBox.Show("The loading has failed: " + result.FailLoadingMessage);
                        }
                    });
        }
    }
}