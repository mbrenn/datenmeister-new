using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Actions.ActionHandler
{
    public class ActionSetMethods
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(ActionSetMethods));

        /// <summary>
        /// Returns the item identified by path and workspace
        /// </summary>
        /// <param name="actionLogic">Action Logic to be used</param>
        /// <param name="workspaceId">Workspace to be used</param>
        /// <param name="path">Path to be used</param>
        /// <returns>Found Item or null</returns>
        public static IElement GetItemByWorkspaceAndPath(
            ActionLogic actionLogic, string workspaceId, string path)
        {
            var workspace = actionLogic.WorkspaceLogic.GetWorkspace(workspaceId);
            if (workspace == null)
            {
                var message = $"workspace is not found ${workspaceId}";
                Logger.Error(message);
                
                throw new InvalidOperationException(message);
            }
            
            var sourceElement = workspace.Resolve(path, ResolveType.NoMetaWorkspaces);
            if (!(sourceElement is IElement asElement))
            {
                var message = $"sourcePath is not found {path}";
                Logger.Error(message);
                
                throw new InvalidOperationException(message);
            }

            return asElement;
        }
        
        /// <summary>
        /// Returns the item identified by path and workspace
        /// </summary>
        /// <param name="actionLogic">Action Logic to be used</param>
        /// <param name="workspaceId">Workspace to be used</param>
        /// <param name="path">Path to be used</param>
        /// <returns>Found Item or null</returns>
        public static IReflectiveCollection GetCollectionByWorkspaceAndPath(
            ActionLogic actionLogic, string workspaceId, string path)
        {
            var workspace = actionLogic.WorkspaceLogic.GetWorkspace(workspaceId);
            if (workspace == null)
            {
                var message = $"workspace is not found ${workspaceId}";
                Logger.Error(message);
                
                throw new InvalidOperationException(message);
            }
            
            var sourceElement = workspace.Resolve(path, ResolveType.NoMetaWorkspaces);
            if (!(sourceElement is IReflectiveCollection asReflectiveCollection))
            {
                var message = $"sourcePath is not found {path}";
                Logger.Error(message);
                
                throw new InvalidOperationException(message);
            }

            return asReflectiveCollection;
        }
    }
}