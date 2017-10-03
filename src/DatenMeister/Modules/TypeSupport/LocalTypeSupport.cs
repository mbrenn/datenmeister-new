using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.Workspaces;

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

        public IList<IElement> AddLocalType(IEnumerable<Type> types)
        {
            var result = new List<IElement>();
            var internalTypeExtent = GetInternalTypeExtent();

            var generator = new DotNetTypeGenerator(
                new MofFactory(internalTypeExtent),
                _workspaceLogic.GetUmlData());

            foreach (var type in types)
            {
                var element = generator.CreateTypeFor(type);
                internalTypeExtent.elements().add(element);
                result.Add(element);
            }

            return result;
        }

        /// <summary>
        /// Adds a local type to the default instance of the DatenMeister
        /// </summary>
        /// <param name="type">Type to be added</param>
        public IElement AddLocalType(Type type)
        {
            return AddLocalType(new[]{type}).First();
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