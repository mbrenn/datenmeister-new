using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
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
        /// Gets a package by following the path.
        /// </summary>
        /// <param name="rootElements">Collection in which the package shall be created</param>
        /// <param name="packagePath">Path to the package</param>
        /// <returns>Found element</returns>
        public IElement GetPackageStructure(
            IReflectiveCollection rootElements,
            string packagePath)
        {
            var uml = _workspaceLogic.GetFromMetaLayer<_UML>(((IHasExtent) rootElements).Extent, MetaRecursive.Recursively);
            return GetOrCreatePackageStructure(
                rootElements,
                new MofFactory(rootElements),
                packagePath,
                _UML._CommonStructure._NamedElement.name,
                _UML._Packages._Package.packagedElement,
                uml.Packages.__Package,
                false);
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
            var uml = _workspaceLogic.GetFromMetaLayer<_UML>(((IHasExtent)rootElements).Extent, MetaRecursive.Recursively);
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
        public IReflectiveCollection GetPackagedObjects(
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
            IElement metaClass = null,
            bool flagCreate = true)
        {
            if (rootElements == null) throw new ArgumentNullException(nameof(rootElements));

            var elementNames = packagePath
                .Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).ToList();

            var id = "_package";

            IElement found = null;
            foreach (var elementName in elementNames)
            {
                id += $"_{elementName}";

                // Looks for the element with the given name
                IElement childElement = null;
                foreach (var innerElement in rootElements.OfType<IElement>())
                {
                    if (!innerElement.isSet(nameProperty))
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
                    if (flagCreate)
                    {
                        childElement = factory.create(metaClass);
                        childElement.set(nameProperty, elementName);

                        // Set ID, for the new element
                        if (childElement is ICanSetId cansetId)
                        {
                            cansetId.Id = id;
                        }

                        rootElements.add(childElement);
                    }
                    else
                    {
                        return null;
                    }
                }

                // Sets and finds the child property by the given name
                IReflectiveSequence children = null;
                if (childElement.isSet(childProperty))
                {
                    children = childElement.get(childProperty) as IReflectiveSequence;
                }

                if (children == null)
                {
                    childElement.set(childProperty, Array.Empty<object>());
                    children = new MofReflectiveSequence((MofObject) childElement, childProperty);
                }

                rootElements = children;
                found = childElement;
            }

            return found;
        }

        /// <summary>
        /// Adds an object to a package
        /// </summary>
        /// <param name="package">Package to which the elements shall be added</param>
        /// <param name="element">Element to be added</param>
        public static void AddObjectToPackage(IElement package, object element)
        {
            var packagedElements = package.GetAsReflectiveCollection(_UML._Packages._Package.packagedElement);
            packagedElements.add(element);
        }

        /// <summary>
        /// Adds the element of the elements to the package
        /// </summary>
        /// <param name="package">Package to be set</param>
        /// <param name="elements">Elements to be added</param>
        public static void AddObjectsToPackage(IElement package, IEnumerable<object> elements)
        {
            var packagedElements = package.GetAsReflectiveCollection(_UML._Packages._Package.packagedElement);
            foreach (var item in elements)
            {
                packagedElements.add(item);
            }
        }

        /// <summary>
        /// Gets the packages elements of the package
        /// </summary>
        /// <param name="package">Package to be evaluated</param>
        /// <returns>ReflectiveCollection containing the packaged elements</returns>
        public static IReflectiveCollection GetPackagedObjects(IObject package)
        {
            return package.GetAsReflectiveCollection(_UML._Packages._Package.packagedElement);
        }

        /// <summary>
        /// Imports a set of element into the the target package by creating the additional
        /// subpackage structure as given in <c>packagePath</c>
        /// </summary>
        /// <param name="sourcePackage">Package containing the source element</param>
        /// <param name="target">Target of the reflective sequence in which the sub packages will be
        /// added. </param>
        /// <param name="packagePath">Path of the package that is relevant to the intended target.</param>
        public void ImportPackage(IObject sourcePackage, IReflectiveSequence target, string packagePath)
        {
            var targetPackage = GetOrCreatePackageStructure(target, packagePath);

            // We got the package, import the elements
            ImportPackage(sourcePackage, targetPackage);
        }

        /// <summary>
        /// Imports the package according the Uml rules (Ok, not now, but at sometime at one day).
        /// At the moment, it is a very simple rule to copy each element into the other branch
        /// </summary>
        /// <param name="sourcePackage">Source package containing the elements to be imported</param>
        /// <param name="targetPackage">Target package receiving the elements</param>
        /// <param name="copyOptions">Defines the options which shall be used for the importing of the package</param>
        public static void ImportPackage(IObject sourcePackage, IElement targetPackage, CopyOption copyOptions = null)
        {
            copyOptions = copyOptions ?? CopyOptions.None;
            var objectCopier = new ObjectCopier(new MofFactory(targetPackage.GetExtentOf()));
            foreach (var subElement in GetPackagedObjects(sourcePackage).OfType<IObject>())
            {
                var copiedObject = objectCopier.Copy(subElement, copyOptions);
                AddObjectToPackage(targetPackage, copiedObject);
            }
        }

        /// <summary>
        /// Imports a package by a manifest resource
        /// </summary>
        /// <param name="manifestType">Type of the assembly containing the
        /// manifest. It eases the life instead of given the assembly</param>
        /// <param name="manifestName">Name of the manifest resource stream</param>
        /// <param name="sourcePackageName">Path of the package to be imported</param>
        /// <param name="targetExtent">Extent to which the extent shall be imported</param>
        /// <param name="targetPackageName">Path within the extent that shall receive
        /// the package</param>
        public void ImportByManifest(Type manifestType, string manifestName,
            string sourcePackageName,
            IExtent targetExtent, string targetPackageName)
        {
            // Creates the package for "ManagementProvider" containing the views
            var targetPackage = GetOrCreatePackageStructure(
                targetExtent.elements(), targetPackageName);

            using (var stream = typeof(PackageMethods).GetTypeInfo()
                .Assembly.GetManifestResourceStream(manifestName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"The stream for {manifestName} could not be opened");
                }

                var document = XDocument.Load(stream);
                var pseudoProvider = new XmiProvider(document);
                var pseudoExtent = new MofUriExtent(pseudoProvider)
                {
                    Workspace = (Workspace) targetExtent.GetWorkspace()
                };

                var sourcePackage = GetOrCreatePackageStructure(
                    pseudoExtent.elements(),
                    sourcePackageName);
                PackageMethods.ImportPackage(sourcePackage, targetPackage, CopyOptions.CopyId);
            }
        }
    }
}