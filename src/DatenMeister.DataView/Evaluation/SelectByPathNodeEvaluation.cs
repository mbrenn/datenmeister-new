﻿using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation
{
    public class SelectByPathNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(SelectByPathNodeEvaluation));

        public bool IsResponsible(IElement node)
        {
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.equals(_DatenMeister.TheOne.DataViews.__SelectByPathNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {
            var workspaceId = viewNode.getOrDefault<string>(_DatenMeister._DataViews._SelectByPathNode.workspaceId);
            var path = viewNode.getOrDefault<string>(_DatenMeister._DataViews._SelectByPathNode.path);
            if (workspaceId == null)
            {
                Logger.Warn("Workspace is not set.");
                throw new InvalidOperationException("Workspace is not set");
            }

            var workspace = evaluation.WorkspaceLogic!.GetWorkspace(workspaceId);
            if (workspace == null)
            {
                Logger.Warn($"Workspace is not found: {workspaceId}");
                throw new InvalidOperationException($"Workspace is not found: {workspaceId}");
            }

            var found = workspace.Resolve(path, ResolveType.NoMetaWorkspaces, true);

            if (found is IElement element)
            {
                return new PureReflectiveSequence(new[] { element });
            }
            else if (found is IReflectiveCollection collection)
            {
                return collection;
            }
            else if (found is IExtent extent)
            {
                return extent.elements();
            }
            else if (found == null)
            {
                throw new InvalidOperationException("Not found: " + path);
            }
            else
            {
                throw new InvalidOperationException("Unknown return type: " + found.GetType().ToString());
            }
        }
    }
}