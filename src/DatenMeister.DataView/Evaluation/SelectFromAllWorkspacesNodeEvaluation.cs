using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.DataView.Evaluation;

/// <summary>
/// Implements the evaluation for the select from all workspaces node
/// </summary>
public class SelectFromAllWorkspacesNodeEvaluation : IDataViewNodeEvaluation
{
    /// <inheritdoc />
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Source.__SelectFromAllWorkspacesNode);
    }

    /// <inheritdoc />
    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        if (evaluation.WorkspaceLogic == null)
            throw new InvalidOperationException("WorkspaceLogic is null");

        return new TemporaryReflectiveCollection(GetElements(evaluation));
    }
        
    /// <summary>
    /// Gets an enumeration of all elements fitting to any of the extents
    /// </summary>
    /// <param name="evaluation"></param>
    /// <returns></returns>
    private IEnumerable<IObject> GetElements(DataViewEvaluation evaluation)
    {
        foreach (var workspace in evaluation.WorkspaceLogic!.Workspaces.ToList())
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