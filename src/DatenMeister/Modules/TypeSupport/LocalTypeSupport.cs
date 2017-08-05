using System;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
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
        /// <summary>
        /// Adds a local type to the default instance of the DatenMeister
        /// </summary>
        /// <param name="scope">Scope to be added</param>
        /// <param name="type">Type to be added</param>
        public static void AddLocalType(ILifetimeScope scope, Type type)
        {
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
            var workspace = workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
            var internalTypeExtent = GetInternalTypeExtent(workspace);
            var generator = new DotNetTypeGenerator(
                new MofFactory(internalTypeExtent),
                scope.GetUmlData());

            var element = generator.CreateTypeFor(type);
            internalTypeExtent.elements().add(element);
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