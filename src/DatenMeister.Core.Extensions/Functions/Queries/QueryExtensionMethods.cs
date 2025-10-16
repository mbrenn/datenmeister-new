using DatenMeister.Core.Extensions.Functions.Aggregation;
using DatenMeister.Core.Extensions.Functions.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;

namespace DatenMeister.Core.Extensions.Functions.Queries;

public static class QueryExtensionMethods
{
    /// <summary>
    /// Groups all properties by performing an aggregation
    /// </summary>
    /// <param name="collection">Collection to be aggregated</param>
    /// <param name="groupByColumn">The column which shall be used to identify same keys</param>
    /// <param name="aggregateColumn">The column which will be aggregated</param>
    /// <param name="aggregatorFunc">The function which creates an aggregator to combine the values</param>
    /// <param name="aggregatedColumn">The target column to which the property will be allocated</param>
    /// <returns></returns>
    public static IReflectiveCollection GroupProperties(
        this IReflectiveCollection collection,
        string groupByColumn,
        string aggregateColumn,
        Func<IAggregator> aggregatorFunc,
        string aggregatedColumn)
        =>
            new GroupByReflectiveCollection(
                collection,
                groupByColumn,
                aggregateColumn,
                aggregatorFunc,
                aggregatedColumn);
}