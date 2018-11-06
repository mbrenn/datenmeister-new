﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.TypeSupport
{
    /// <summary>
    /// Support for local types. Local types are types that are initialized at start-up
    /// of the DatenMeister and will not be stored into 
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LocalTypeSupport
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(LocalTypeSupport));

        /// <summary>
        /// Stores the workspace logic being used
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly ExtentCreator _extentCreator;
        private readonly PackageMethods _packageMethods;

        public IUriExtent InternalTypes => GetInternalTypeExtent();

        public IUriExtent UserTypeExtent => GetUserTypeExtent(); 

        /// <summary>
        /// Initializes a new instance of the LocalTypeSupport class
        /// </summary>
        /// <param name="workspaceLogic">Workspace logic which is required to find the given local type support storage</param>
        public LocalTypeSupport(IWorkspaceLogic workspaceLogic, ExtentCreator extentCreator, PackageMethods packageMethods)
        {
            _workspaceLogic = workspaceLogic;
            _extentCreator = extentCreator;
            _packageMethods = packageMethods;
        }

        /// <summary>
        /// Creates the extent being used to store the internal types
        /// </summary>
        /// <returns>The created uri contining the internal types</returns>
        public void Initialize()
        {
            CreateInternalTypeExtent();

            // Creates the extent for the user types which is permanently stored on disk. The user is capable to create his own types
            CreatesUserTypeExtent();
        }

        private void CreateInternalTypeExtent()
        {
            // Creates the workspace and extent for the types layer which are belonging to the types  
            var extentTypes = new MofUriExtent(
                new InMemoryProvider(),
                WorkspaceNames.UriInternalTypesExtent);
            var typeWorkspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
            extentTypes.SetExtentType("Uml.Classes");
            _workspaceLogic.AddExtent(typeWorkspace, extentTypes);

            // Copies the Primitive Types to the internal types, so it is available for everybody. 
            var foundPackage = _packageMethods.GetOrCreatePackageStructure(extentTypes.elements(), "PrimitiveTypes");
            CopyMethods.CopyToElementsProperty(
                _workspaceLogic.GetUmlWorkspace()
                    .FindElementByUri("datenmeister:///_internal/xmi/primitivetypes?PrimitiveTypes")
                    .get(_UML._Packages._Package.packagedElement) as IReflectiveCollection,
                foundPackage,
                _UML._Packages._Package.packagedElement);
        }

        /// <summary>
        /// Creates the user type extent storing the types for the user. 
        /// If the extent is already existing, debugs the number of found extents
        /// </summary>
        private void CreatesUserTypeExtent()
        {
            var foundExtent = _extentCreator.GetOrCreateXmiExtentInInternalDatabase(
                WorkspaceNames.NameTypes,
                WorkspaceNames.UriUserTypesExtent,
                "DatenMeister.Types_User",
                "Uml.Classes"
            );

            // Creates the user types, if not existing
            var numberOfTypes = foundExtent.elements().Count();
            Logger.Debug($"Loaded the extent for user types, containing of {numberOfTypes} types");
        }

        /// <summary>
        /// Adds a local type in a certain package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="types"></param>
        /// <returns>Elements beng added to the external types</returns>
        public IList<IElement> AddInternalTypes(string packageName, IEnumerable<Type> types)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            var rootElements = internalTypeExtent.elements();

            var package = PackageMethods.GetOrCreatePackageStructure(
                rootElements,
                new MofFactory(internalTypeExtent),
                packageName,
                "name",
                _UML._Packages._Package.packagedElement,
                _workspaceLogic.GetUmlData().Packages.__Package);

            package.set(_UML._Packages._Package.packagedElement, new List<object>());

            IReflectiveSequence children;
            if (package.isSet(_UML._Packages._Package.packagedElement))
            {
                children = (IReflectiveSequence) package.get(_UML._Packages._Package.packagedElement);
            }
            else
            {
                throw new InvalidOperationException("Extent does not support setting of empty lists");
            }

            return AddInternalTypes(children, types);
        }

        /// <summary>
        /// Adds a local type 
        /// </summary>
        /// <param name="types">Type to be added</param>
        /// <param name="packageName">Defines the package name to which the elements shall be created</param>
        /// <returns>List of created elements</returns>
        public IList<IElement> AddInternalTypes(IEnumerable<Type> types, string packageName = null)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            IReflectiveCollection rootElements = internalTypeExtent.elements();
            if (packageName != null)
            {
                rootElements = _packageMethods.GotoPackage(rootElements, packageName);
            }

            return AddInternalTypes(rootElements, types);
        }

        /// <summary>
        /// Adds types to the local types
        /// </summary>
        /// <param name="rootElements">Reflective sequence which will receive the new element</param>
        /// <param name="types">Types to be added</param>
        /// <returns>List of created elements</returns>
        private IList<IElement> AddInternalTypes(IReflectiveCollection rootElements, IEnumerable<Type> types)
        {
            var result = new List<IElement>();
            var generator = new DotNetTypeGenerator(
                new MofFactory(GetInternalTypeExtent()),
                _workspaceLogic.GetUmlData());

            foreach (var type in types)
            {
                var element = generator.CreateTypeFor(type);
                rootElements.add(element); // Adds to the extent
                result.Add(element); // Adds to the internal return list
            }

            return result;
        }

        /// <summary>
        /// Adds a local type to the default instance of the DatenMeister
        /// </summary>
        /// <param name="type">Type to be added</param>
        public IElement AddInternalTypes(Type type)
        {
            return AddInternalTypes(new[]{type}).First();
        }

        /// <summary>
        /// Adds a local type to the default instance of the DatenMeister
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <param name="type">Type to be added</param>
        public IElement AddInternalType(string packageName, Type type)
        {
            return AddInternalTypes(packageName, new[] { type }).First();
        }

        /// <summary>
        /// Gets the metaclass within the internal type and user-defined type extent
        /// </summary>
        /// <param name="type">Type to be defined</param>
        /// <returns>The element of the meta class</returns>
        public IElement GetMetaClassFor(Type type)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            var found = internalTypeExtent.element(internalTypeExtent.contextURI() + "#" + type.FullName);

            if (found != null)
            {
                return found;
            }

            foreach (var extent in GetOtherTypeExtents())
            {
                found = extent.element(internalTypeExtent.contextURI() + "#" + type.FullName);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a specific meta class by having the full name (Package.PackageB.Name)
        /// </summary>
        /// <param name="fullName">The full name, describing the parents and children</param>
        /// <returns></returns>
        public IElement GetMetaClassFor(string fullName)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            var found = NamedElementMethods.GetByFullName(internalTypeExtent.elements(), fullName);

            if (found != null)
            {
                return found;
            }

            foreach (var extent in GetOtherTypeExtents())
            {
                found = NamedElementMethods.GetByFullName(extent.elements(), fullName);
                if (found != null)
                {
                    return found;
                }
            }

            return null;

        }

        private IUriExtent GetInternalTypeExtent()
        { 
            var workspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
            var internalTypeExtent = GetInternalTypeExtent(workspace);
            return internalTypeExtent;
        }

        private IUriExtent GetUserTypeExtent()
        {
            var workspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
            var internalTypeExtent = GetUserTypeExtent(workspace);
            return internalTypeExtent;
        }

        /// <summary>
        /// Gets all other type extents, except the internal type extent being located in the workspace of types
        /// </summary>
        /// <returns>Enumeration of all other type extents</returns>
        private IEnumerable<IUriExtent> GetOtherTypeExtents()
        {
            var workspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);

            return workspace.extent
                .OfType<IUriExtent>()
                .Where(x => x.contextURI() != WorkspaceNames.UriInternalTypesExtent)
                .ToList();
        }

        /// <summary>
        /// Gets the extent containing the 
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public static IUriExtent GetInternalTypeExtent(IWorkspace workspace)
        {
            return workspace.FindExtent(WorkspaceNames.UriInternalTypesExtent);
        }

        /// <summary>
        /// Gets the extent containing the 
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public static IUriExtent GetUserTypeExtent(IWorkspace workspace)
        {
            return workspace.FindExtent(WorkspaceNames.UriUserTypesExtent);
        }
    }
}