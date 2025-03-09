using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView.Evaluation
{
    public class SelectByWorkspaceNodeEvaluation : IDataViewNodeEvaluation
    {
        public bool IsResponsible(IElement node)
        {
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.equals(_DatenMeister.TheOne.DataViews.__SelectByWorkspaceNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {
            if (evaluation.WorkspaceLogic == null)
                throw new InvalidOperationException("WorkspaceLogic is null");

            var workspaceId =
                viewNode.getOrDefault<string>(_DatenMeister._DataViews._SelectByWorkspaceNode.workspaceId);
            var workspace = evaluation.WorkspaceLogic.GetWorkspace(workspaceId)
                ?? throw new InvalidOperationException($"Workspace {workspaceId} not found");

            return new TemporaryReflectiveCollection(GetElements(evaluation, workspace));
        }

        /// <summary>
        /// Gets an enumeration of all elements fitting to any of the extents
        /// </summary>
        /// <param name="evaluation">The used dataevaluation</param>
        /// <param name="workspace">The workspace which shall be queried</param>
        /// <returns></returns>
        private IEnumerable<IObject> GetElements(DataViewEvaluation evaluation, Workspace workspace)
        {
            foreach (var extent in workspace.extent.ToList())
            {
                foreach (var rootItem in extent.elements().OfType<IObject>())
                {
                    yield return rootItem;
                }
            }
        }
    }
}