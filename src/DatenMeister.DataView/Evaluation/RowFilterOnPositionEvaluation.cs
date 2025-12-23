using BurnSystems.Logging;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation;

/// <summary>
/// Implements the evaluation for the row filter on position node
/// </summary>
public class RowFilterOnPositionEvaluation : IDataViewNodeEvaluation
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(RowFilterOnPositionEvaluation));

    /// <inheritdoc />
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Row.__RowFilterOnPositionNode);
    }

    /// <inheritdoc />
    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var wrappedViewNode = new DataViews.Row.RowFilterOnPositionNode_Wrapper(viewNode);
        
        var inputNode = viewNode.getOrDefault<IElement>(_DataViews._Row._RowFilterOnPositionNode.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            return new PureReflectiveSequence();
        }
        
        var input = evaluation.GetElementsForViewNode(inputNode);
        return new RowFilterOnPosition(input, wrappedViewNode.position, wrappedViewNode.amount);
    }
}