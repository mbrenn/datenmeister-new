#nullable enable

using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DefaultTypes
{
    /// <summary>
    /// Stores the default classifier hints which allow a harmonized identification
    /// of common classifier, like packages, 
    /// </summary>
    public class DefaultClassifierHints
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(DefaultClassifierHints));
        
        private IWorkspaceLogic _workspaceLogic;

        public DefaultClassifierHints(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Gets the default package classifier for a given extent
        /// </summary>
        /// <param name="uriExtent">Extent to be used</param>
        /// <returns>The found extent</returns>
        public IElement? GetDefaultPackageClassifier(IHasExtent uriExtent)
        {
            var extent = uriExtent.Extent;
            
            // First look into the standard uml meta classes
            var findByUrl = extent.FindInMeta<_UML>(x => x.Packages.__Package);
            if (findByUrl == null)
            {
                // If not found, check for the default package model in the types workspace
                findByUrl = extent.GetUriResolver()
                    .Resolve("datenmeister:///_internal/types/internal#DatenMeister.Modules.DefaultTypes.Model.Package", ResolveType.OnlyMetaClasses);
            }

            if (findByUrl == null)
            {
                Logger.Warn("No default package was found in the given extent");
            }

            return findByUrl;
        }

        /// <summary>
        /// Gets the default name of the property which contains the elements of a property
        /// This name is dependent upon the element to which the object will be added and
        /// the extent in which the element will be added.  
        /// </summary>
        /// <param name="packagingElement">Element in which the element will be added</param>
        /// <returns>The name of the property to which the element will be added</returns>
        public string GetDefaultPackagePropertyName(IObject packagingElement)
        {
            return _UML._Packages._Package.packagedElement;
        }

        /// <summary>
        /// Adds the given child to the container by considering whether the container
        /// is an extent or is a MOF object. If an extent, it will be simply added as a child
        /// otherwise, it will be added to the property
        /// </summary>
        /// <param name="container">Container to which the element will be added</param>
        /// <param name="child">Child element which will be added</param>
        public void AddToExtentOrElement(IObject container, IObject child)
        {
            if (container is IExtent extent)
            {
                extent.elements().add(child);
            }
            else
            {
                var propertyName = GetDefaultPackagePropertyName(container);
                container.AddCollectionItem(propertyName, child);
            }
        }
        
        /// <summary>
        /// Gets the information whether the given element is a package to which
        /// an additional object can be added as an enumeration. If this method returns true,
        /// an element can be added via AddToExtentOrElement
        /// </summary>
        /// <param name="value">Element which shall be checked</param>
        /// <returns>true, if the given element is a package</returns>
        public bool IsPackageLike(IObject value)
        {
            // At the moment, every element is a package
            return true;
        }
    }
}