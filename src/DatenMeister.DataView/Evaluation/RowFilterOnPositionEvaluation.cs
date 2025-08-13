using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation;

public class RowFilterOnPositionEvaluation : IDataViewNodeEvaluation
{
    private static readonly ILogger Logger = new ClassLogger(typeof(RowFilterOnPositionEvaluation));

    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.__RowFilterOnPositionNode);
    }

    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var wrappedViewNode = new DataViews.RowFilterOnPositionNode_Wrapper(viewNode);
        
        var inputNode = viewNode.getOrDefault<IElement>(_DataViews._ColumnFilterExcludeNode.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            return new PureReflectiveSequence();
        }
        
        var input = evaluation.GetElementsForViewNode(inputNode);
        return new RowFilterOnPosition(input, wrappedViewNode.position, wrappedViewNode.amount);
    }
}