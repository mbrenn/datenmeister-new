using System.Collections.Generic;
using DatenMeister.Core.Plugins;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ImportExtentManager
{
    public class ImportExtentManagerPlugin : IDatenMeisterPlugin
    {
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new ImportExtentViewExtensions());
        }
    }

    public class ImportExtentViewExtensions : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            if (viewExtensionTargetInformation.NavigationGuest is ItemsInExtentList)
            {
                // Adds the import elements
                yield return new RibbonButtonDefinition(
                        "Import existing Extent",
                        null,
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
        }
    }
}