using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.DotNet;
using DatenMeister.Core.Runtime;
using DatenMeister.Provider.ExtentManagement;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.DefaultTypes;

public class DefaultTypesExtension : IViewExtensionFactory
{
    private readonly DefaultClassifierHints _defaultClassifierHints;

    public DefaultTypesExtension(DefaultClassifierHints hints)
    {
        _defaultClassifierHints = hints;
    }

    public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionInfo viewExtensionInfo)
    {
        var btn = GetNewPackageButton(viewExtensionInfo);
        if (btn != null)
        {
            yield return btn;
        }
    }

    /// <summary>
    /// Creates a button allowing to create a new package
    /// </summary>
    /// <param name="viewExtensionInfo"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private ItemButtonDefinition? GetNewPackageButton(
        ViewExtensionInfo viewExtensionInfo)
    {
        // Only, if the navigation guest is the item explorer view
        var itemExplorerView = viewExtensionInfo.GetItemExplorerControl();

        // Check, if we are in an item explorer control
        if (itemExplorerView == null)
            return null;

        // Only if the root element is an extent, otherwise it is a special thing which we
        // don't want to handle
        if (!(itemExplorerView.RootItem is MofExtent extent))
            return null;

        // DotNetProvider and ExtentOfWorkspaces are also special providers
        if (extent.Provider is DotNetProvider || extent.Provider is ExtentOfWorkspaceProvider)
            return null;

        // Check, if the selected element is a package or an extent
        // which allows 
        if (itemExplorerView.SelectedItem is IElement selectedElement
            && !DefaultClassifierHints.IsPackageLike(selectedElement))
            return null;

        return
            new ItemButtonDefinition(
                "New Package",
                clickedItem =>
                {
                    if (clickedItem == null) throw new InvalidOperationException("ClickedItem == null");
                    if (!(clickedItem is IHasExtent asExtent))
                        throw new InvalidOperationException("Not of type asExtent");

                    // Create new package
                    var factory = new MofFactory(extent);
                    var type = _defaultClassifierHints.GetDefaultPackageClassifier(asExtent);
                    var package = factory.create(type);
                    package.set(_UML._CommonStructure._NamedElement.name, "Unnamed");

                    DefaultClassifierHints.AddToExtentOrElement(
                        clickedItem,
                        package);

                    _ = NavigatorForItems.NavigateToElementDetailView(
                        viewExtensionInfo.NavigationHost,
                        package);
                });
    }
}