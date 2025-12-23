using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.DataView.Evaluation;

/// <summary>
/// Implements the evaluation for the row filter node
/// </summary>
public class RowOrderByNodeEvaluation : IDataViewNodeEvaluation
{
    /// <inheritdoc />
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Row.__RowOrderByNode);
    }

    /// <inheritdoc />
    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var input = evaluation.GetInputNode(viewNode);

        var property = viewNode.getOrDefault<string>(_DataViews._Row._RowOrderByNode.propertyName);
        var orderByDescending = viewNode.getOrDefault<bool>(_DataViews._Row._RowOrderByNode.orderDescending);

        return new RowOrderByProperties(input, 
        [orderByDescending ? "!" + property : property]);
    }
}