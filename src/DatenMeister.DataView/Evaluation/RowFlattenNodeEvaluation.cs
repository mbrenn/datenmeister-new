using BurnSystems.Logging;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation;

/// <summary>
/// Implements the evaluation for the row flatten node
/// </summary>
public class RowFlattenNodeEvaluation : IDataViewNodeEvaluation
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(RowFlattenNodeEvaluation));

    /// <inheritdoc />
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Row.__RowFlattenNode);
    }

    /// <inheritdoc />
    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var inputNode = viewNode.getOrDefault<IElement>(_DataViews._Row._RowFlattenNode.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            return new PureReflectiveSequence();
        }

        return evaluation.GetElementsForViewNode(inputNode).GetAllDescendantsIncludingThemselves();
    }
}