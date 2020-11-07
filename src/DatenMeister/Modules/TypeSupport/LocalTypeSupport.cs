using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.EMOF;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Exceptions;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.Uml.Plugin;

namespace DatenMeister.Modules.TypeSupport
{
    /// <summary>
    /// Support for local types. Local types are types that are initialized at start-up
    /// of the DatenMeister and will not be stored into
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
    public class LocalTypeSupport : IDatenMeisterPlugin
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(LocalTypeSupport));

        /// <summary>
        /// Stores the workspace logic being used
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly ExtentCreator _extentCreator;
        private readonly PackageMethods _packageMethods;
        private readonly IScopeStorage _scopeStorage;
        private readonly IntegrationSettings _integrationSettings;

        public IUriExtent InternalTypes => GetInternalTypeExtent();

        public IUriExtent UserTypeExtent => GetUserTypeExtent();

        public LocalTypeSupport(
            IWorkspaceLogic workspaceLogic,
            IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
            
            _packageMethods = new PackageMethods();
            _extentCreator = new ExtentCreator(workspaceLogic, scopeStorage);
            _integrationSettings = scopeStorage.Get<IntegrationSettings>();
        }
        /// <summary>
        /// Initializes a new instance of the LocalTypeSupport class
        /// </summary>
        /// <param name="workspaceLogic">Workspace logic which is required to find the given local type support storage</param>
        /// <param name="extentCreator">The creator for the extents</param>
        /// <param name="packageMethods">The methods for the packages</param>
        /// <param name="scopeStorage">The Integration settings</param>
        public LocalTypeSupport(
            IWorkspaceLogic workspaceLogic,
            ExtentCreator extentCreator,
            PackageMethods packageMethods,
            IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
            
            _packageMethods = packageMethods;
            _extentCreator = extentCreator;
            _integrationSettings = scopeStorage.Get<IntegrationSettings>();
        }

        /// <summary>
        /// Creates the extent being used to store the internal types
        /// </summary>
        /// <returns>The created uri containing the internal types</returns>
        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    CreateInternalTypeExtent();
                    
                    var defaultTypeIntegrator = new DefaultTypeIntegrator(_workspaceLogic, _scopeStorage);
                    defaultTypeIntegrator.CreateDefaultTypesForTypesWorkspace();
                    break;
                case PluginLoadingPosition.AfterLoadingOfExtents:
                    // Creates the extent for the user types which is permanently stored on disk. The user is capable to create his own types
                    CreatesUserTypeExtent();
                    break;
            }
        }

        private void CreateInternalTypeExtent()
        {
            // Creates the workspace and extent for the types layer which are belonging to the types
            var extentTypes = new MofUriExtent(
                new InMemoryProvider(),
                WorkspaceNames.UriExtentInternalTypes);
            extentTypes.SlimUmlEvaluation = true;
            var typeWorkspace = _workspaceLogic.GetTypesWorkspace();
            extentTypes.GetConfiguration().ExtentType = UmlPlugin.ExtentType;
            _workspaceLogic.AddExtent(typeWorkspace, extentTypes);
        }

        /// <summary>
        /// Creates the user type extent storing the types for the user.
        /// If the extent is already existing, debugs the number of found extents
        /// </summary>
        private void CreatesUserTypeExtent()
        {
            var foundExtent = _extentCreator.GetOrCreateXmiExtentInInternalDatabase(
                WorkspaceNames.WorkspaceTypes,
                WorkspaceNames.UriExtentUserTypes,
                "DatenMeister.Types_User",
                UmlPlugin.ExtentType,
                _integrationSettings.InitializeDefaultExtents ? ExtentCreationFlags.CreateOnly : ExtentCreationFlags.LoadOrCreate
            );
            
            if( foundExtent == null )
                throw new InvalidOperationException("Extent for users is not found");

            // Creates the user types, if not existing
            var numberOfTypes = foundExtent.elements().Count();
            Logger.Debug($"Loaded the extent for user types, containing of {numberOfTypes} types");
        }

        /// <summary>
        /// Adds a local type in a certain package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="types"></param>
        /// <returns>Elements being added to the external types</returns>
        public IList<IElement> AddInternalTypes(string packageName, IEnumerable<Type> types)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            var rootElements = internalTypeExtent.elements();

            var package = PackageMethods.GetOrCreatePackageStructure(
                rootElements,
                new MofFactory(internalTypeExtent),
                packageName,
                "name",
                _UML.TheOne.Packages.__Package);
            if (package == null)
                throw new InvalidOperationException("package == null");

            package.set(_UML._Packages._Package.packagedElement, new List<object>());

            IReflectiveSequence children;
            if (package.isSet(_UML._Packages._Package.packagedElement))
            {
                children = package.get<IReflectiveSequence>(_UML._Packages._Package.packagedElement);
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
        public IList<IElement> AddInternalTypes(IEnumerable<Type> types, string? packageName = null)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            IReflectiveCollection? rootElements = internalTypeExtent.elements();
            if (packageName != null)
            {
                rootElements = _packageMethods.GetPackagedObjects(rootElements, packageName);
            }

            if (rootElements == null)
            {
                throw new InvalidOperationException("Packaged object could not be created");
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
            var internalTypeExtent = (MofExtent) GetInternalTypeExtent();
            var generator = new DotNetTypeGenerator(
                internalTypeExtent);

            foreach (var type in types)
            {
                var element = generator.CreateTypeFor(type);
                rootElements.add(element); // Adds to the extent
                result.Add(element); // Adds to the internal return list

                var uri = element.GetUri();
                if (uri != null && !string.IsNullOrEmpty(uri))
                {
                    internalTypeExtent.TypeLookup.Add(uri, type);
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a local type to the default instance of the DatenMeister
        /// </summary>
        /// <param name="type">Type to be added</param>
        public IElement AddInternalTypes(Type type)
        {
            return AddInternalTypes(new[] {type}).First();
        }

        /// <summary>
        /// Adds a local type to the default instance of the DatenMeister
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <param name="type">Type to be added</param>
        public IElement AddInternalType(string packageName, Type type)
        {
            return AddInternalTypes(packageName, new[] {type}).First();
        }

        /// <summary>
        /// Gets the metaclass within the internal type and user-defined type extent
        /// </summary>
        /// <param name="type">Type to be defined</param>
        /// <returns>The element of the meta class</returns>
        public IElement? GetMetaClassFor(Type type)
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
        public IElement? GetMetaClassFor(string fullName)
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

        /// <summary>
        /// Gets the extent containing the internal types
        /// </summary>
        /// <returns>Extent containing the internal types which are rebuilt at each DatenMeister start-up</returns>
        public IUriExtent GetInternalTypeExtent()
        {
            return GetInternalTypeExtent(_workspaceLogic);
        }

        /// <summary>
        /// Gets the extent containing the types being created by the user
        /// </summary>
        /// <returns>Extent containing elements for the user and which are stored persistently. </returns>
        public IUriExtent GetUserTypeExtent()
        {
            var workspace = _workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceTypes);
            if (workspace == null)
                throw new InvalidOperationException("Types workspace does not exist");
            
            var internalTypeExtent = GetUserTypeExtent(workspace);
            return internalTypeExtent;
        }

        /// <summary>
        /// Gets all other type extents, except the internal type extent being located in the workspace of types
        /// </summary>
        /// <returns>Enumeration of all other type extents</returns>
        private IEnumerable<IUriExtent> GetOtherTypeExtents()
        {
            var workspace = _workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceTypes);
            if (workspace == null)
                throw new InvalidOperationException("Types workspace does not exist");

            return workspace.extent
                .OfType<IUriExtent>()
                .Where(x => x.contextURI() != WorkspaceNames.UriExtentInternalTypes)
                .ToList();
        }

        /// <summary>
        /// Gets the extent containing the
        /// </summary>
        /// <param name="workspaceLogic">The workspace logic being used</param>
        /// <returns></returns>
        public static IUriExtent GetInternalTypeExtent(IWorkspaceLogic workspaceLogic)
        {
            var workspace = workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceTypes);
            if (workspace == null)
                throw new InvalidOperationException("Types workspace does not exist");
            
            return GetInternalTypeExtent(workspace);
        }

        /// <summary>
        /// Gets the extent containing the internal Extent
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public static IUriExtent GetInternalTypeExtent(IWorkspace workspace) =>
            workspace.FindExtent(WorkspaceNames.UriExtentInternalTypes);

        /// <summary>
        /// Gets the extent containing the
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public static IUriExtent GetUserTypeExtent(IWorkspace workspace) =>
            workspace.FindExtent(WorkspaceNames.UriExtentUserTypes);

        /// <summary>
        /// Creates a new instance of the metaclass by looking through the internal extent
        /// </summary>
        /// <param name="metaClassId">Id of the metaclass item that is describing the created instance</param>
        /// <param name="factory">The factory that is used to create the item</param>
        /// <returns>The created internal type</returns>
        public IObject CreateInternalType(string metaClassId, IFactory? factory = null)
        {
            var foundMetaClass = InternalTypes.element(metaClassId);
            if (foundMetaClass == null)
            {
                throw new DatenMeisterRawException($"Element of type {metaClassId} was not found");
            }

            return (factory ?? InMemoryObject.TemporaryFactory).create(foundMetaClass);
        }
    }
}