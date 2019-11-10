#nullable enable
using System.Collections.Generic;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.DefaultTypes
{
    public class DefaultTypesExtension : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            var btn = GetNewPackageButton(viewExtensionTargetInformation);
            if (btn != null)
            {
                yield return btn;
            }
        }

        private static ItemButtonDefinition? GetNewPackageButton(
            ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            if (!(viewExtensionTargetInformation.NavigationGuest is ItemsTreeView treeView))
                return null;

            if (!(treeView.RootElement is MofExtent extent))
                return null;

            if (extent.Provider is DotNetProvider || extent.Provider is ExtentOfWorkspaces)
                return null;

            var selectedExtent = treeView.SelectedElement as IUriExtent;
            var metaClass = (treeView.SelectedElement as IElement)?.metaclass;
            
            // Check, if the selected element is a package or an extent
            if (metaClass?.ToString() != "Package" && selectedExtent == null)
                return null;
            
            return
                new ItemButtonDefinition(
                    "New Package",
                    x =>
                    {
                        // Create new package
                        var factory = new MofFactory(extent);
                        var localTypeSupport = GiveMe.Scope.Resolve<LocalTypeSupport>();
                        var type = localTypeSupport.InternalTypes.element(
                            "datenmeister:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package");
                        var package = factory.create(type);
                        package.set(_UML._CommonStructure._NamedElement.name, "Unnamed");

                        if (selectedExtent != null)
                        {
                            selectedExtent.elements().add(package);
                        }
                        else
                        {
                            x.AddCollectionItem(_UML._Packages._Package.packagedElement, package);
                        }

                        NavigatorForItems.NavigateToElementDetailView(
                            viewExtensionTargetInformation.NavigationHost,
                            package);
                    });
        }
    }
}