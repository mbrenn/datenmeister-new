using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.TypeSupport
{
    /// <summary>
    /// Support for local types. Local types are types that are initialized at start-up
    /// of the DatenMeister and will not be stored into 
    /// </summary>
    public static class LocalTypeSupport
    {

        public static IList<IElement> AddLocalType(ILifetimeScope scope, IEnumerable<Type> types)
        {
            var result = new List<IElement>();
            var internalTypeExtent = GetInternalTypeExtent(scope);

            var generator = new DotNetTypeGenerator(
                new MofFactory(internalTypeExtent),
                scope.GetUmlData());

            foreach (var type in types)
            {
                var element = generator.CreateTypeFor(type);
                internalTypeExtent.elements().add(element);
                result.Add(element);
            }

            return result;
        }

        private static IUriExtent GetInternalTypeExtent(ILifetimeScope scope)
        {
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
            var workspace = workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
            var internalTypeExtent = GetInternalTypeExtent(workspace);
            return internalTypeExtent;
        }

        /// <summary>
        /// Adds a local type to the default instance of the DatenMeister
        /// </summary>
        /// <param name="scope">Scope to be added</param>
        /// <param name="type">Type to be added</param>
        public static IElement AddLocalType(ILifetimeScope scope, Type type)
        {
            return AddLocalType(scope, new[]{type}).First();
        }

        public static IElement GetMetaClassFor(ILifetimeScope scope, Type type)
        {
            var internalTypeExtent = GetInternalTypeExtent(scope);
            var found = internalTypeExtent.element(internalTypeExtent.contextURI() + "#" + type.FullName);
            return found;
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