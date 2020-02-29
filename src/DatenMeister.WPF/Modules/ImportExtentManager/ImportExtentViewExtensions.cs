using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ImportExtentManager
{
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
                var navigationHost = viewExtensionInfo.NavigationHost
                    ?? throw new InvalidOperationException("navigationHost == null");
                
                var controlNavigation = await NavigatorForItems.NavigateToElementDetailViewAsync(
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
                    if (metaClass?.@equals(_ManagementProvider.TheOne.__Extent) != true)
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

                    var workspaceName = workspace.getOrDefault<string>(_ManagementProvider._Workspace.id);
                    var uri = selectedExtent.getOrDefault<string>(_ManagementProvider._Extent.uri);

                    // Gets the extent from which the data shall be imported
                    var sourceExtent = GiveMe.Scope.WorkspaceLogic.FindExtent(workspaceName, uri);

                    var itemCountBefore = sourceExtent.elements().Count();
                    var elements = (itemsInExtentList.RootItem as IExtent)?.elements()
                                   ?? throw new InvalidOperationException("elements == null");
                    _plugin.PerformImport(sourceExtent, elements);
                    var itemCountAfter = sourceExtent.elements().Count();

                    MessageBox.Show($"Import has been performed. {itemCountAfter - itemCountBefore} root elements have been added.");
                }
            }

            async void ImportNewExtent(IExtent extent)
            {
                var navigationHost = viewExtensionInfo.NavigationHost
                                     ?? throw new InvalidOperationException("navigationHost == null");
                var result = await WorkspaceExtentFormGenerator.QueryExtentConfigurationByUserAsync(
                    navigationHost);
                if (result != null)
                {
                    // Now, we got the item extent...
                    var extentManager = GiveMe.Scope.Resolve<IExtentManager>();
                    var loadedExtent = extentManager.LoadExtentWithoutAdding(result);
                    if (loadedExtent != null)
                    {
                        var itemCountBefore = loadedExtent.elements().Count();
                        var elements = (itemsInExtentList.RootItem as IExtent)?.elements()
                                       ?? throw new InvalidOperationException("elements == null");
                        _plugin.PerformImport(loadedExtent, elements);
                        var itemCountAfter = loadedExtent.elements().Count();

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
    }
}