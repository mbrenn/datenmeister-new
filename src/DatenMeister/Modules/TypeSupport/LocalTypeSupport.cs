using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
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
        private readonly IWorkspaceLogic _workspaceLogic;

        public LocalTypeSupport(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Adds a local type in a certain package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public IList<IElement> AddLocalTypes(string packageName, IEnumerable<Type> types)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            var rootElements = internalTypeExtent.elements();

            var package = NamedElementMethods.GetOrCreatePackageStructure(
                rootElements,
                new MofFactory(internalTypeExtent),
                packageName,
                "name",
                "children",
                "Package");

            package.set("children", new List<object>());
            var children = (IReflectiveSequence) package.get("children");
            return AddLocalTypes(children, types);
        }

        /// <summary>
        /// Adds a local type 
        /// </summary>
        /// <param name="types">Type to be added</param>
        /// <returns>List of created elements</returns>
        public IList<IElement> AddLocalTypes(IEnumerable<Type> types)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            var rootElements = internalTypeExtent.elements();

            return AddLocalTypes(rootElements, types);
        }

        /// <summary>
        /// Adds types to the local types
        /// </summary>
        /// <param name="rootElements">Reflective sequence which will receive the new element</param>
        /// <param name="types">Types to be added</param>
        /// <returns>List of created elements</returns>
        private IList<IElement> AddLocalTypes(IReflectiveSequence rootElements, IEnumerable<Type> types)
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
        public IElement AddLocalTypes(Type type)
        {
            return AddLocalTypes(new[]{type}).First();
        }

        /// <summary>
        /// Adds a local type to the default instance of the DatenMeister
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <param name="type">Type to be added</param>
        public IElement AddLocalTypes(string packageName, Type type)
        {
            return AddLocalTypes(packageName, new[] { type }).First();
        }

        public IElement GetMetaClassFor(Type type)
        {
            var internalTypeExtent = GetInternalTypeExtent();
            var found = internalTypeExtent.element(internalTypeExtent.contextURI() + "#" + type.FullName);
            return found;
        }

        private IUriExtent GetInternalTypeExtent()
        { 
            var workspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
            var internalTypeExtent = GetInternalTypeExtent(workspace);
            return internalTypeExtent;
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