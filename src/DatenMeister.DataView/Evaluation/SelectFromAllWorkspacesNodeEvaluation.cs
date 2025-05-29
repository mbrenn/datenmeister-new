using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.DataView.Evaluation;

public class SelectFromAllWorkspacesNodeEvaluation : IDataViewNodeEvaluation
{
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DatenMeister.TheOne.DataViews.__SelectFromAllWorkspacesNode);
    }

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