using System;
using System.Collections.Generic;
using BurnSystems.Logging;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Types.Plugin;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.ListViews;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.TypeManager
{
    public class TypeManagerViewExtension : IViewExtensionFactory
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(TypeManagerViewExtension));

        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionInfo viewExtensionInfo)
        {
            var navigationHost = viewExtensionInfo.NavigationHost ??
                                 throw new InvalidOperationException("NavigationHost == null");

            if (viewExtensionInfo.GetMainApplicationWindow() != null)
            {
                yield return new ApplicationMenuButtonDefinition(
                    "Goto User Types", async () => await NavigatorForItems.NavigateToItemsInExtent(
                        navigationHost,
                        WorkspaceNames.WorkspaceTypes,
                        WorkspaceNames.UriExtentUserTypes),
                    string.Empty,
                    NavigationCategories.DatenMeisterNavigation);

                yield return new ApplicationMenuButtonDefinition(
                    "Goto Types", async () => await NavigatorForExtents.NavigateToExtentList(
                        navigationHost,
                        WorkspaceNames.WorkspaceTypes),
                    string.Empty,
                    NavigationCategories.DatenMeisterNavigation);
            }

            var packageMetaClass = _UML.TheOne.Packages.__Package;
            var classMetaClass = _UML.TheOne.StructuredClassifiers.__Class;
            var enumerationMetaClass = _UML.TheOne.SimpleClassifiers.__Enumeration;
            var enumerationLiteralMetaClass = _UML.TheOne.SimpleClassifiers.__EnumerationLiteral;
            var propertyMetaClass = _UML.TheOne.Classification.__Property;
            var packageInternalClass = _DatenMeister.TheOne.CommonTypes.Default.__Package;
            
            var isExtentInListView = viewExtensionInfo.IsExtentInListViewControl(UmlPlugin.ExtentType);
            var isInPackage = viewExtensionInfo.IsItemOfExtentTypeInListViewControl(
                _UML._Packages._Package.packagedElement,
                new[] {packageMetaClass, packageInternalClass},
                UmlPlugin.ExtentType);

            var isInClass = viewExtensionInfo.IsItemOfExtentTypeInListViewControl(
                _UML._StructuredClassifiers._Class.ownedAttribute,
                new[] {classMetaClass});

            var isInEnumeration = viewExtensionInfo.IsItemOfExtentTypeInListViewControl(
                _UML._SimpleClassifiers._Enumeration.ownedLiteral,
                new[] {enumerationMetaClass});

            if (isInPackage || isExtentInListView)
            {
                yield return new NewInstanceViewExtension(classMetaClass);
                yield return new NewInstanceViewExtension(enumerationMetaClass);
            }

            if (isInClass)
            {
                yield return new NewInstanceViewExtension(propertyMetaClass);
            }

            if (isInEnumeration)
            {
                yield return new NewInstanceViewExtension(enumerationLiteralMetaClass);
            }
        }
    }
}