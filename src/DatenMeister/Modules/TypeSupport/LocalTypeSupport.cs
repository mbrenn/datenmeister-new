using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Plugins;
using DatenMeister.Integration;
using DatenMeister.Modules.FastViewFilter;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
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
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping|PluginLoadingPosition.AfterLoadingOfExtents)]
    public class LocalTypeSupport : IDatenMeisterPlugin
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(LocalTypeSupport));

        /// <summary>
        /// Stores the workspace logic being used
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly ExtentCreator _extentCreator;
        private readonly PackageMethods _packageMethods;
        private readonly IntegrationSettings _integrationSettings;

        public IUriExtent InternalTypes => GetInternalTypeExtent();

        public IUriExtent UserTypeExtent => GetUserTypeExtent();

        /// <summary>
        /// Initializes a new instance of the LocalTypeSupport class
        /// </summary>
        /// <param name="workspaceLogic">Workspace logic which is required to find the given local type support storage</param>
        /// <param name="extentCreator">The creator for the extents</param>
        /// <param name="packageMethods">The methods for the packages</param>
        /// <param name="integrationSettings">The Integration settings</param>
        public LocalTypeSupport(
            IWorkspaceLogic workspaceLogic,
            ExtentCreator extentCreator,
            PackageMethods packageMethods,
            IntegrationSettings integrationSettings)
        {
            _workspaceLogic = workspaceLogic;
            _extentCreator = extentCreator;
            _packageMethods = packageMethods;
            _integrationSettings = integrationSettings;
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
                WorkspaceNames.UriInternalTypesExtent);
            var typeWorkspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
            extentTypes.SetExtentType("Uml.Classes");
            _workspaceLogic.AddExtent(typeWorkspace, extentTypes);

            // Copies the Primitive Types to the internal types, so it is available for everybody, we will create a new extent for this

            var primitiveTypes = new MofUriExtent(
                new InMemoryProvider(),
                WorkspaceNames.UriPrimitiveTypesExtent);
            primitiveTypes.AddAlternativeUri(WorkspaceNames.StandardPrimitiveTypeNamespace);
            primitiveTypes.AddAlternativeUri(WorkspaceNames.StandardPrimitiveTypeNamespaceAlternative);

            if (!_integrationSettings.PerformSlimIntegration)
            {
                var foundPackage =
                    _packageMethods.GetOrCreatePackageStructure(primitiveTypes.elements(), "PrimitiveTypes");
                _workspaceLogic.AddExtent(typeWorkspace, primitiveTypes);

                CopyMethods.CopyToElementsProperty(
                    _workspaceLogic.GetUmlWorkspace()
                        .FindElementByUri("datenmeister:///_internal/xmi/primitivetypes?PrimitiveTypes")
                        .get(_UML._Packages._Package.packagedElement) as IReflectiveCollection,
                    foundPackage,
                    _UML._Packages._Package.packagedElement,
                    CopyOptions.CopyId);

                // Create the Primitive Type for the .Net-Type: DateTime
                var internalUserExtent = GetInternalTypeExtent();
                var factory = new MofFactory(internalUserExtent);
                var package =
                    _packageMethods.GetOrCreatePackageStructure(internalUserExtent.elements(), "PrimitiveTypes");
                var umlData = _workspaceLogic.GetUmlData();

                var dateTime = factory.create(umlData.SimpleClassifiers.__PrimitiveType);
                ((ICanSetId) dateTime).Id = "PrimitiveTypes.DateTime";
                dateTime.set(_UML._CommonStructure._NamedElement.name, "DateTime");
                PackageMethods.AddObjectToPackage(package, dateTime);
            }
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
                "Uml.Classes",
                _integrationSettings.InitializeDefaultExtents ? ExtentCreationFlags.CreateOnly : ExtentCreationFlags.LoadOrCreate
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
                rootElements = _packageMethods.GetPackagedObjects(rootElements, packageName);
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
                internalTypeExtent,
                _workspaceLogic.GetUmlData());

            foreach (var type in types)
            {
                var element = generator.CreateTypeFor(type);
                rootElements.add(element); // Adds to the extent
                result.Add(element); // Adds to the internal return list

                internalTypeExtent.TypeLookup.Add(element.GetUri(), type);
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

        /// <summary>
        /// Gets the extent containing the internal types
        /// </summary>
        /// <returns>Extent containing the internal types which are rebuilt at each DatenMeister start-up</returns>
        public IUriExtent GetInternalTypeExtent()
        { 
            var workspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
            var internalTypeExtent = GetInternalTypeExtent(workspace);
            return internalTypeExtent;
        }

        /// <summary>
        /// Gets the extent containing the types being created by the user
        /// </summary>
        /// <returns>Extent containing elements for the user and which are stored persistently. </returns>
        public IUriExtent GetUserTypeExtent()
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