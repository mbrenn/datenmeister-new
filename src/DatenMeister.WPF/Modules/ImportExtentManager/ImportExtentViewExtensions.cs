using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
using DatenMeister.WPF.Forms.Lists;
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

        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            if (viewExtensionTargetInformation.NavigationGuest is ItemsInExtentList itemInExtentList)
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
                var controlNavigation = await NavigatorForItems.NavigateToElementDetailViewAsync(
                    viewExtensionTargetInformation.NavigationHost,
                    new NavigateToItemConfig
                    {
                        DetailElement = InMemoryObject.CreateEmpty(),
                        Form = new FormDefinition(
                            GiveMe.Scope.WorkspaceLogic.GetInternalViewsExtent()
                                .element("#ImportManagerFindExtent"))
                    });

                if (controlNavigation.Result == NavigationResult.Saved)
                {
                    var selectedExtent = controlNavigation.DetailElement.getOrDefault<IElement>("selectedExtent");
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
                    }

                    var workspaceName = workspace.getOrDefault<string>(_ManagementProvider._Workspace.id);
                    var uri = selectedExtent.getOrDefault<string>(_ManagementProvider._Extent.uri);

                    // Gets the extent from which the data shall be imported
                    var sourceExtent = GiveMe.Scope.WorkspaceLogic.FindExtent(workspaceName, uri);

                    var itemCountBefore = sourceExtent.elements().Count();
                    _plugin.PerformImport(sourceExtent, (itemInExtentList.RootItem as IExtent)?.elements());
                    var itemCountAfter = sourceExtent.elements().Count();

                    MessageBox.Show($"Import has been performed. {itemCountAfter - itemCountBefore} root elements have been added.");
                }
            }

            async void ImportNewExtent(IExtent extent)
            {
                var result = await WorkspaceExtentFormGenerator.QueryExtentConfigurationByUserAsync(viewExtensionTargetInformation.NavigationHost);
                if (result != null)
                {
                    // Now, we got the item extent...
                    var extentManager = GiveMe.Scope.Resolve<IExtentManager>();
                    var loadedExtent = extentManager.LoadExtentWithoutAdding(result);
                    if (loadedExtent != null)
                    {
                        var itemCountBefore = loadedExtent.elements().Count();
                        _plugin.PerformImport(loadedExtent, (itemInExtentList.RootItem as IExtent)?.elements());
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