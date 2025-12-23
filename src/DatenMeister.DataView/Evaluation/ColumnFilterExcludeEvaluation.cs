using BurnSystems.Logging;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation;

/// <summary>
/// Implements the evaluation for the column filter exclude node
/// </summary>
public class ColumnFilterExcludeEvaluation : IDataViewNodeEvaluation
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(ColumnFilterExcludeEvaluation));

    /// <inheritdoc />
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Column.__ColumnFilterExcludeNode);
    }

    /// <inheritdoc />
    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var inputNode = viewNode.getOrDefault<IElement>(_DataViews._Column._ColumnFilterExcludeNode.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            return new PureReflectiveSequence();
        }
        
        var input = evaluation.GetElementsForViewNode(inputNode);
        
        var wrappedViewNode = new DataViews.Column.ColumnFilterExcludeNode_Wrapper(viewNode);
        var columnNames = wrappedViewNode.columnNamesComma;
        if (string.IsNullOrEmpty(columnNames))
        {
            return input;
        }

        var columns = columnNames.Split(',').Select(x=>x.Trim()).Where(x=>!string.IsNullOrEmpty(x));

        return new ColumnFilterExclude(input)
        {
            ExcludedColumns = columns.ToHashSet()
        };
    }
}