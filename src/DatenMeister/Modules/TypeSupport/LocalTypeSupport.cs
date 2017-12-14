using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
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
        /// <summary>
        /// Stores the workspace logic being used
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Initializes a new instance of the LocalTypeSupport class
        /// </summary>
        /// <param name="workspaceLogic">Workspace logic which is required to find the given local type support storage</param>
        public LocalTypeSupport(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Creates the extent being used to store the internal types
        /// </summary>
        /// <returns>The created uri contining the internal types</returns>
        public MofUriExtent CreateInternalTypeExtent()
        {
            // Creates the workspace and extent for the types layer which are belonging to the types  
            var extentTypes = new MofUriExtent(
                new InMemoryProvider(),
                WorkspaceNames.UriInternalTypes);
            var typeWorkspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
            extentTypes.SetExtentType("Uml.Classes");
            _workspaceLogic.AddExtent(typeWorkspace, extentTypes);
            return extentTypes;
        }

        /// <summary>
        /// Adds a local type in a certain package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public IList<IElement> AddInternalTypes(string packageName, IEnumerable<Type> types)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            var rootElements = internalTypeExtent.elements();

            var package = NamedElementMethods.GetOrCreatePackageStructure(
                rootElements,
                new MofFactory(internalTypeExtent),
                packageName,
                "name",
                _UML._CommonStructure._Namespace.member,
                _workspaceLogic.GetUmlData().Packages.__Package);

            package.set(_UML._CommonStructure._Namespace.member, new List<object>());

            IReflectiveSequence children;
            if (package.isSet(_UML._CommonStructure._Namespace.member))
            {
                children = (IReflectiveSequence) package.get(_UML._CommonStructure._Namespace.member);
            }
            else
            {
                package.set(_UML._CommonStructure._Namespace.member, new List<object>());
                children = (IReflectiveSequence) package.get(_UML._CommonStructure._Namespace.member);
            }

            return AddInternalTypes(children, types);
        }

        /// <summary>
        /// Adds a local type 
        /// </summary>
        /// <param name="types">Type to be added</param>
        /// <returns>List of created elements</returns>
        public IList<IElement> AddInternalTypes(IEnumerable<Type> types)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            var rootElements = internalTypeExtent.elements();

            return AddInternalTypes(rootElements, types);
        }

        /// <summary>
        /// Adds types to the local types
        /// </summary>
        /// <param name="rootElements">Reflective sequence which will receive the new element</param>
        /// <param name="types">Types to be added</param>
        /// <returns>List of created elements</returns>
        private IList<IElement> AddInternalTypes(IReflectiveSequence rootElements, IEnumerable<Type> types)
        {
            var result = new List<IElement>();
            var generator = new DotNetTypeGenerator(
                new MofFactory(GetInternalTypeExtent()),
                _workspaceLogic.GetUmlData());

            foreach (var type in types)
            {
                var element = generator.CreateTypeFor(type);
                rootElements.add(element);
                result.Add(element);
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
        public IElement AddInternalTypes(string packageName, Type type)
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

        /// <summary>
        /// Gets all other type extents, except the internal type extent being located in the workspace of types
        /// </summary>
        /// <returns>Enumeration of all other type extents</returns>
        private IEnumerable<IUriExtent> GetOtherTypeExtents()
        {
            var workspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);

            return workspace.extent
                .OfType<IUriExtent>()
                .Where(x => x.contextURI() != WorkspaceNames.UriInternalTypes)
                .ToList();
        }

        /// <summary>
        /// Gets the extent containing the 
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public static IUriExtent GetInternalTypeExtent(IWorkspace workspace)
        {
            return workspace.FindExtent(WorkspaceNames.UriInternalTypes);
        }
    }
}