using BurnSystems.Logging;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView.Evaluation;

/// <summary>
/// Implements the evaluation for the select by extent node
/// </summary>
public class SelectByExtentNodeEvaluation : IDataViewNodeEvaluation
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(SelectByExtentNodeEvaluation));

    /// <inheritdoc />
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Source.__SelectByExtentNode);
    }

    /// <inheritdoc />
    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var workspaceLogic = evaluation.WorkspaceLogic;
        if (workspaceLogic == null)
        {
            // No workspace logic is set, but source extent queries for an extent and so is dependent upon 
            // the workspacelogic
            Logger.Error("SourceExtent specified but no workspace Logic given");
            throw new InvalidOperationException("SourceExtent specified but no workspace Logic given");
        }

        var workspaceName = viewNode.getOrDefault<string>(_DataViews._Source._SelectByExtentNode.workspaceId);
        if (string.IsNullOrEmpty(workspaceName))
        {
            workspaceName = WorkspaceNames.WorkspaceData;
        }

        var extentUri = viewNode.getOrDefault<string>(_DataViews._Source._SelectByExtentNode.extentUri);
        var workspace = workspaceLogic.GetWorkspace(workspaceName);
        if (workspace == null)
        {
            Logger.Warn($"Workspace is not found: {workspaceName}");
            throw new InvalidOperationException($"Workspace is not found: {workspaceName}");
        }

        var extent = workspace.FindExtent(extentUri);
        if (extent == null)
        {
            Logger.Warn($"Extent is not found: {extentUri}");
            throw new InvalidOperationException($"Extent is not found: {extentUri} for workspace {workspaceName}");
        }

        return extent.elements();
    }
}