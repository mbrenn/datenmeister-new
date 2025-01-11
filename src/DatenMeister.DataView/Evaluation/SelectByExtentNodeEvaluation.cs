using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView.Evaluation
{
    public class SelectByExtentNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(SelectByExtentNodeEvaluation));

        public bool IsResponsible(IElement node)
        {
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.equals(_DatenMeister.TheOne.DataViews.__SelectByExtentNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {
            var workspaceLogic = evaluation.WorkspaceLogic;
            if (workspaceLogic == null)
            {
                // No workspace logic is set but source extent queries for an extent and so is dependent upon 
                // the workspacelogic
                Logger.Error("SourceExtent specified but no workspace Logic given");
                return new PureReflectiveSequence();
            }

            var workspaceName = viewNode.getOrDefault<string>(_DatenMeister._DataViews._SelectByExtentNode.workspace);
            if (string.IsNullOrEmpty(workspaceName))
            {
                workspaceName = WorkspaceNames.WorkspaceData;
            }

            var extentUri = viewNode.getOrDefault<string>(_DatenMeister._DataViews._SelectByExtentNode.extentUri);
            var workspace = workspaceLogic.GetWorkspace(workspaceName);
            if (workspace == null)
            {
                Logger.Warn($"Workspace is not found: {workspaceName}");
                return new PureReflectiveSequence();
            }

            var extent = workspace.FindExtent(extentUri);
            if (extent == null)
            {
                Logger.Warn($"Extent is not found: {extentUri}");
                return new PureReflectiveSequence();
            }

            return extent.elements();
        }
    }
}