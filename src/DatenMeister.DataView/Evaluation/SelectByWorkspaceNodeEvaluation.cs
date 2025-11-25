using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;

namespace DatenMeister.DataView.Evaluation;

public class SelectByWorkspaceNodeEvaluation : IDataViewNodeEvaluation
{
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Source.__SelectByWorkspaceNode);
    }

    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        if (evaluation.WorkspaceLogic == null)
            throw new InvalidOperationException("WorkspaceLogic is null");

        var workspaceId =
            viewNode.getOrDefault<string>(_DataViews._Source._SelectByWorkspaceNode.workspaceId);
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
    private IEnumerable<IObject> GetElements(DataViewEvaluation evaluation, IWorkspace workspace)
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