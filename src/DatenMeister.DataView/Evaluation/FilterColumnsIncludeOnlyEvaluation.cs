using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation;

public class FilterColumnsIncludeOnlyEvaluation : IDataViewNodeEvaluation
{
    private static readonly ILogger Logger = new ClassLogger(typeof(FilterColumnsIncludeOnlyEvaluation));

    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.__FilterColumnsIncludeOnly);
    }

    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var inputNode = viewNode.getOrDefault<IElement>(_DataViews._FilterByMetaclassNode.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            return new PureReflectiveSequence();
        }

        var input = evaluation.GetElementsForViewNode(inputNode);

        var wrappedViewNode = new DataViews.FilterColumnsIncludeOnly_Wrapper(viewNode);
        var columnNames = wrappedViewNode.columnNamesComma;
        if (string.IsNullOrEmpty(columnNames))
        {
            return input;
        }

        var columns = columnNames.Split(',').Select(x=>x.Trim()).Where(x=>!string.IsNullOrEmpty(x));

        return new FilterColumnsIncludeOnly(input)
        {
            IncludeColumns = columns.ToHashSet()
        };
    }
}