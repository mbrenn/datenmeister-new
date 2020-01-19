#nullable enable
using System;
using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.DefaultTypes
{
    public class DefaultTypesExtension : IViewExtensionFactory
    {
        private readonly DefaultClassifierHints _defaultClassifierHints;

        public DefaultTypesExtension(DefaultClassifierHints hints)
        {
            _defaultClassifierHints = hints;
        }
        
        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            var btn = GetNewPackageButton(viewExtensionTargetInformation);
            if (btn != null)
            {
                yield return btn;
            }
        }

        private ItemButtonDefinition? GetNewPackageButton(
            ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            if (!(viewExtensionTargetInformation.NavigationGuest is ItemsTreeView treeView))
                return null;

            if (!(treeView.RootElement is MofExtent extent))
                return null;

            if (extent.Provider is DotNetProvider || extent.Provider is ExtentOfWorkspaces)
                return null;

            // Check, if the selected element is a package or an extent
            if (treeView.SelectedElement is IElement selectedElement 
                && !_defaultClassifierHints.IsPackageLike(selectedElement))
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
                        
                        _defaultClassifierHints.AddToExtentOrElement(
                            clickedItem, 
                            package);

                        var navigationHost = viewExtensionTargetInformation.NavigationHost ??
                                             throw new InvalidOperationException("NavigationHost == null");
                        NavigatorForItems.NavigateToElementDetailView(
                            viewExtensionTargetInformation.NavigationHost,
                            package);
                    });
        }
    }
}