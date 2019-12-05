#nullable enable

using DatenMeister.Core;
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
        public IObject GetDefaultPackageClassifier(IHasExtent uriExtent)
        {
            var extent = uriExtent.Extent;
            return extent.FindInMeta<_UML>(x => x.Packages.__Package);
        }

        /// <summary>
        /// Gets the default name of the property which contains the elements of a property
        /// This name is dependent upon the element to which the object will be added and
        /// the extent in which the element will be added.  
        /// </summary>
        /// <param name="packagingElement">Element in which the element will be added</param>
        /// <returns>The name of the property to which the element will be added</returns>
        public string GetDefaultPackagePropertyName(IElement packagingElement)
        {
            return _UML._Packages._Package.packagedElement;
        }
    }
}