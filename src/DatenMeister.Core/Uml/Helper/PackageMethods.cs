using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.Uml.Helper
{
    public class PackageMethods
    {
        /// <summary>
        /// Gets a package by following the path.
        /// </summary>
        /// <param name="rootElements">Collection in which the package shall be created</param>
        /// <param name="packagePath">Path to the package</param>
        /// <returns>Found element</returns>
        public IElement? GetPackageStructure(
            IReflectiveCollection rootElements,
            string packagePath)
        {
            var packageClassifier = DefaultClassifierHints.GetDefaultPackageClassifier(
                (rootElements as IHasExtent)?.GetExtentOf() ??
                throw new InvalidOperationException("No Extent connected"));
            
            return GetOrCreatePackageStructure(
                rootElements,
                new MofFactory(rootElements),
                packagePath,
                _UML._CommonStructure._NamedElement.name,
                packageClassifier,
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
            return GetOrCreatePackageStructure(rootElements, packagePath, true)
                   ?? throw new NotImplementedException("Should not be null");
        }

        /// <summary>
        /// Gets or creates a package by following the path.
        /// </summary>
        /// <param name="rootElements">Collection in which the package shall be created</param>
        /// <param name="packagePath">Path to the package</param>
        /// <param name="createIfNotFound">Gets or sets the flag that the package will be automatically created
        /// in case it is not found</param>
        /// <returns>Found element</returns>
        public IElement? GetOrCreatePackageStructure(
            IReflectiveCollection rootElements,
            string packagePath,
            bool createIfNotFound)
        {
            var packageClassifier = DefaultClassifierHints.GetDefaultPackageClassifier(
                (rootElements as IHasExtent)?.GetExtentOf() ??
                throw new InvalidOperationException("No Extent connected"));
            
            return GetOrCreatePackageStructure(
                rootElements,
                new MofFactory(rootElements),
                packagePath,
                _UML._CommonStructure._NamedElement.name,
                packageClassifier,
                createIfNotFound);
        }

        /// <summary>
        /// Gets or creates a package by following the path.
        /// </summary>
        /// <param name="rootElements">Collection in which the package shall be created</param>
        /// <param name="packagePath">Path to the package</param>
        /// <returns>Found element</returns>
        public IReflectiveCollection? GetPackagedObjects(
            IReflectiveCollection rootElements,
            string packagePath)
        {
            var element = GetOrCreatePackageStructure(rootElements, packagePath);

            return element.get<IReflectiveCollection>(_UML._Packages._Package.packagedElement);
        }

        /// <summary>
        /// Gets or create the package structure and returns the last created element.
        /// The elements will be interspaced by '::'
        /// </summary>
        /// <param name="rootElements">Elements which contain the root elements. Can be the extent itself</param>
        /// <param name="factory">Factory being used to create the subitems</param>
        /// <param name="packagePath">Path of the package to be created</param>
        /// <param name="nameProperty">The name property which contain the name for the element</param>
        /// <param name="metaClass">The Metaclass being used to create the child packages</param>
        /// <param name="flagCreate">true, if the structure shall be really created.
        /// If false, null will be returned if the package is not found</param>
        public static IElement? GetOrCreatePackageStructure(
            IReflectiveCollection rootElements,
            IFactory factory,
            string packagePath,
            string nameProperty,
            IElement? metaClass = null,
            bool flagCreate = true)
        {
            if (rootElements == null) throw new ArgumentNullException(nameof(rootElements));

            var elementNames = packagePath
                .Split(new[] {"::"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).ToList();

            var id = "_package";

            IElement? found = null;
            foreach (var elementName in elementNames)
            {
                id += $"_{elementName}";

                // Looks for the element with the given name
                IElement? childElement = null;
                foreach (var innerElement in rootElements.OfType<IElement>())
                {
                    if (!innerElement.isSet(nameProperty))
                    {
                        continue;
                    }

                    if (innerElement.getOrDefault<string>(nameProperty) == elementName)
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
                IReflectiveSequence? children = null;
                var childProperty = DefaultClassifierHints.GetDefaultPackagePropertyName(childElement);
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

            return found ?? throw new InvalidOperationException("Something weird happened. Should not be null");
        }

        /// <summary>
        /// Adds an object to a package
        /// </summary>
        /// <param name="package">Package to which the elements shall be added</param>
        /// <param name="element">Element to be added</param>
        public static void AddObjectToPackage(IObject package, object element)
        {
            if (element is IObject elementAsObject)
            {
                DefaultClassifierHints.AddToExtentOrElement(package, elementAsObject);
            }
            else
            {
                var packagedElements = package.get<IReflectiveCollection>(_UML._Packages._Package.packagedElement);
                packagedElements.add(element);
            }
        }

        /// <summary>
        /// Gets the packages elements of the package
        /// </summary>
        /// <param name="package">Package to be evaluated</param>
        /// <returns>ReflectiveCollection containing the packaged elements</returns>
        public static IReflectiveCollection GetPackagedObjects(IObject package)
        {
            if (package is IExtent asExtent)
            {
                return asExtent.elements();
            }
            else
            {
                return package.get<IReflectiveCollection>(_UML._Packages._Package.packagedElement);
            }
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
        public static void ImportPackage(IObject sourcePackage, IObject targetPackage, CopyOption? copyOptions = null)
        {
            copyOptions ??= CopyOptions.None;
            var objectCopier = new ObjectCopier(new MofFactory(
                targetPackage.GetExtentOf() ??
                throw new InvalidOperationException("targetPackage does not belong to an extent")));
            
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
        /// <param name="sourcePackageName">Path of the package to be imported. Null, if the complete extent shall be imported</param>
        /// <param name="targetExtent">Extent to which the extent shall be imported</param>
        /// <param name="targetPackageName">Path within the extent that shall receive
        /// the package</param>
        /// <param name="loadingRequired">true, if the loading is required and shall throw an exception
        /// in case the loading failed. </param>
        public IObject? ImportByManifest(
            Type manifestType, 
            string manifestName,
            string? sourcePackageName,
            IExtent targetExtent,
            string targetPackageName,
            bool loadingRequired = true)
        {
            using var stream = manifestType.GetTypeInfo()
                .Assembly.GetManifestResourceStream(manifestName);
            
            if (stream == null)
            {
                throw new InvalidOperationException($"The stream for {manifestName} could not be opened");
            }

            return ImportByStream(stream, sourcePackageName, targetExtent, targetPackageName, loadingRequired);
        }

        private IObject ImportByStream(Stream stream, string sourcePackageName, IExtent targetExtent, string targetPackageName,
            bool loadingRequired)
        {
            var document = XDocument.Load(stream);
            var pseudoProvider = new XmiProvider(document);
            var pseudoExtent = new MofUriExtent(pseudoProvider)
            {
                Workspace = (Workspace?) targetExtent.GetWorkspace()
            };

            IObject? sourcePackage;
            if (!string.IsNullOrEmpty(sourcePackageName) && sourcePackageName != null)
            {
                sourcePackage = GetOrCreatePackageStructure(
                    pseudoExtent.elements(),
                    sourcePackageName,
                    false);
            }
            else
            {
                sourcePackage = pseudoExtent;
            }

            if (sourcePackage == null)
            {
                if (loadingRequired)
                {
                    throw new InvalidOperationException(
                        $"sourcePackage == null. Probably {sourcePackageName} not found");
                }
                else
                {
                    return null;
                }
            }

            if (string.IsNullOrEmpty(targetPackageName))
            {
                ImportPackage(sourcePackage, targetExtent, CopyOptions.CopyId);
            }
            else
            {
                // Creates the package for "ManagementProvider" containing the views
                var targetPackage = GetOrCreatePackageStructure(
                    targetExtent.elements(), targetPackageName);

                if (targetPackage == null)
                    throw new InvalidOperationException("targetPackage == null");

                ImportPackage(sourcePackage, targetPackage, CopyOptions.CopyId);
            }

            return sourcePackage;
        }
    }
}