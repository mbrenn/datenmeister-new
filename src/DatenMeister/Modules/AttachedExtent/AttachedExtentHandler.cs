using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.AttachedExtent;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.AttachedExtent
{
    /// <summary>
    /// Defines the handler to support the attached extents. 
    /// </summary>
    public class AttachedExtentHandler
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        public const string AttachedExtentProperty = "DatenMeister.AttachedExtent";

        public AttachedExtentHandler(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Gets the configuration for the attached extent
        /// </summary>
        /// <param name="attachedExtent">Attached extent to be parsed</param>
        /// <returns>The found extent</returns>
        public AttachedExtentConfiguration? GetConfiguration(IExtent attachedExtent)
        {
            var attachedExtentConfiguration = attachedExtent.getOrDefault<IElement>(AttachedExtentProperty);
            if (attachedExtentConfiguration == null)
            {
                return null;
            }

            return DotNetConverter.ConvertToDotNetObject<AttachedExtentConfiguration>(attachedExtentConfiguration);
        }

        public void SetConfiguration(IUriExtent attachedExtent, AttachedExtentConfiguration configuration)
        {
            attachedExtent.set(
                AttachedExtentProperty, 
                DotNetConverter.ConvertToMofObject(attachedExtent, configuration));
        }

        /// <summary>
        /// Gets the original extent 
        /// </summary>
        /// <param name="attachedExtent">Attached extent which might be connected to an original extent</param>
        /// <returns>Found extent or null if not found</returns>
        public IUriExtent? GetOriginalExtent(IUriExtent attachedExtent)
        {
            var configuration = GetConfiguration(attachedExtent);
            if (configuration == null)
            {
                return null;
            }

            var workspace = configuration.referencedWorkspace;
            var extent = configuration.referencedExtent;
            if (workspace == null || extent == null)
            {
                return null;
            }
            
            return _workspaceLogic.FindExtent(workspace, extent) as IUriExtent;
        }

        /// <summary>
        /// Finds all attached extents to the original extents. 
        /// </summary>
        /// <param name="originalExtent">The original extent whose attached extents are looked for</param>
        /// <returns></returns>
        public IEnumerable<IUriExtent> FindAttachedExtents(IUriExtent originalExtent)
        {
            var workspaceName = originalExtent.GetWorkspace()?.id ?? string.Empty;
            var extentName = originalExtent.contextURI();
            if (workspaceName == null || extentName == null) yield break;

            var foundExtents =
                from workspace in _workspaceLogic.Workspaces
                from extent in workspace.extent
                let configuration = GetConfiguration(extent)
                where configuration != null
                      && configuration.referencedWorkspace == workspaceName
                      && configuration.referencedExtent == extentName
                let uriExtent = extent as IUriExtent
                where uriExtent != null
                select uriExtent;

            foreach (var foundExtent in foundExtents)
            {
                yield return foundExtent;
            }
        }
    }
}