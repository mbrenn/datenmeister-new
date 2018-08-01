using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Uml.Helper
{
    public class PackageMethods
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Initializes a new instance of the PackageMethods
        /// </summary>
        /// <param name="workspaceLogic"></param>
        public PackageMethods(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Gets or creates a package by following the path. 
        /// </summary>
        /// <param name="rootElements">Collection in which the package shall be created</param>
        /// <param name="packagePath">Path to the package</param>
        /// <returns>Found element</returns>
        public IElement GetOrCreatePackageStructure(
            IReflectiveCollection rootElements,
            string packagePath)
        {
            var uml = _workspaceLogic.GetFromMetaLayer<_UML>(((IHasExtent)rootElements).Extent);
            return NamedElementMethods.GetOrCreatePackageStructure(
                rootElements,
                new MofFactory(rootElements),
                packagePath,
                _UML._CommonStructure._NamedElement.name,
                _UML._Packages._Package.packagedElement,
                uml.Packages.__Package);
        }

        /// <summary>
        /// Gets or creates a package by following the path. 
        /// </summary>
        /// <param name="rootElements">Collection in which the package shall be created</param>
        /// <param name="packagePath">Path to the package</param>
        /// <returns>Found element</returns>
        public IReflectiveCollection GotoPackage(
            IReflectiveCollection rootElements,
            string packagePath)
        {
            var element = GetOrCreatePackageStructure(rootElements, packagePath);
            return new MofReflectiveSequence(
                element as MofObject,
                _UML._Packages._Package.packagedElement);
        }
    }
}