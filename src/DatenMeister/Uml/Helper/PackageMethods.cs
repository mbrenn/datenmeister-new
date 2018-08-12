using System;
using System.Collections.Generic;
using System.Linq;
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
            return GetOrCreatePackageStructure(
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

        /// <summary>
        /// Gets or create the package structure and returns the last created element.
        /// The elements will be interspaced by '::'
        /// </summary>
        /// <param name="rootElements">Elements which contain the root elements. Can be the extent itself</param>
        /// <param name="factory">Factory being used to create the subitems</param>
        /// <param name="packagePath">Path of the package to be created</param>
        /// <param name="nameProperty">The name property which contain the name for the element</param>
        /// <param name="childProperty">The child property which contain the subelements</param>
        /// <param name="metaClass">The Metaclass being used to create the child packages</param>
        public static IElement GetOrCreatePackageStructure(
            IReflectiveCollection rootElements,
            IFactory factory,
            string packagePath,
            string nameProperty,
            string childProperty,
            IElement metaClass = null)
        {
            if (rootElements == null) throw new ArgumentNullException(nameof(rootElements));

            var elementNames = packagePath
                .Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).ToList();

            IElement found = null;
            foreach (var elementName in elementNames)
            {
                // Looks for the element with the given name
                IElement childElement = null;
                foreach (var innerElement in rootElements.OfType<IElement>())
                {
                    if (innerElement.isSet(nameProperty))
                    {
                        continue;
                    }

                    if (innerElement.get(nameProperty).ToString() == elementName)
                    {
                        childElement = innerElement;
                    }
                }

                // Creates the child element
                if (childElement == null)
                {
                    childElement = factory.create(metaClass);
                    childElement.set(nameProperty, elementName);
                    rootElements.add(childElement);
                }

                // Sets and finds the child property by the given name
                IReflectiveSequence children = null;
                if (childElement.isSet(childProperty))
                {
                    children = childElement.get(childProperty) as IReflectiveSequence;
                }

                if (children == null)
                {
                    childElement.set(childProperty, new List<object>());
                    children = childElement.get(childProperty) as IReflectiveSequence;
                    if (children == null)
                    {
                        // The given MofObject does not support setting and gettingof an empty list
                        // We need to break through
                        return null;
                    }
                }

                rootElements = children;
                found = childElement;

            }

            return found;
        }
    }
}