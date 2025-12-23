using BurnSystems.Logging;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.DataView.Evaluation;

/// <summary>
/// Implements the evaluation for the column filter include only node
/// </summary>
public class ColumnFilterIncludeOnlyEvaluation : IDataViewNodeEvaluation
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(ColumnFilterIncludeOnlyEvaluation));

    /// <inheritdoc />
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Column.__ColumnFilterIncludeOnlyNode);
    }

    /// <inheritdoc />
    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var input = evaluation.GetInputNode(viewNode);

        var wrappedViewNode = new DataViews.Column.ColumnFilterIncludeOnlyNode_Wrapper(viewNode);
        var columnNames = wrappedViewNode.columnNamesComma;
        if (string.IsNullOrEmpty(columnNames))
        {
            return input;
        }

        var columns = columnNames.Split(',').Select(x=>x.Trim()).Where(x=>!string.IsNullOrEmpty(x));

        return new ColumnFilterIncludeOnly(input)
        {
            IncludeColumns = columns.ToHashSet()
        };
    }
}