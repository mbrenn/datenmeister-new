using System;
using System.Collections.Generic;
using BurnSystems.Logging;
using DatenMeister.Integration;
using DatenMeister.Models.EMOF;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Plugin;
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
            }

            var uml = GiveMe.Scope.GetUmlData();
            var packageMetaClass = uml.Packages.__Package;
            var classMetaClass = uml.StructuredClassifiers.__Class;
            var enumerationMetaClass = uml.SimpleClassifiers.__Enumeration;
            var enumerationLiteralMetaClass = uml.SimpleClassifiers.__EnumerationLiteral;
            var propertyMetaClass = uml.Classification.__Property;
            if (packageMetaClass == null || classMetaClass == null || enumerationMetaClass == null)
            {
                Logger.Warn("UML Classes or UML Enumeration not found in meta extent");
            }
            else
            {
                var isExtentInListView = viewExtensionInfo.IsExtentInListViewControl(UmlPlugin.ExtentType);
                var isInPackage = viewExtensionInfo.IsItemOfExtentTypeInListViewControl(
                    _UML._Packages._Package.packagedElement,
                    new[] {packageMetaClass},
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
}