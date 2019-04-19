using System.Collections.Generic;
using DatenMeister.Integration;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ImportExtentManager
{
    /// <summary>
    /// Defines the view extension factory for the Import Extent function
    /// </summary>
    public class ImportExtentViewExtensions : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            if (viewExtensionTargetInformation.NavigationGuest is ItemsInExtentList)
            {
                // Adds the import elements
                yield return new RibbonButtonDefinition(
                    "Import existing Extent",
                    ImportExistingExtent,
                    null,
                    NavigationCategories.File + ".Import");

                yield return new RibbonButtonDefinition(
                    "Import new Extent",
                    null,
                    null,
                    NavigationCategories.File + ".Import");

                yield return new RibbonButtonDefinition(
                    "Import from Clipboard",
                    null,
                    null,
                    NavigationCategories.File + ".Import");
            }

            // Imports the existing extent
            void ImportExistingExtent()
            {
                NavigatorForItems.NavigateToElementDetailView(
                    viewExtensionTargetInformation.NavigationHost,
                    new NavigateToItemConfig
                    {
                        DetailElement = InMemoryObject.CreateEmpty(),
                        FormDefinition = GiveMe.Scope.WorkspaceLogic.GetInternalViewsExtent()
                            .element("#ImportManagerFindExtent")
                    });
            }
        }
    }
}