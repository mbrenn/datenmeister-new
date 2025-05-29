namespace DatenMeister.Core.Extensions.Functions.Interfaces;

public abstract class Aggregator<TAggregate, TItem> : IAggregator
{
    private bool _isStarted;

    internal Aggregator()
    {
    }

    /// <summary>
    /// Aggregates the values of a property into a single value
    /// </summary>
    /// <param name="value">Value to be added</param>
    /// <returns>The aggregated value</returns>
    public void Add(object value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        if (!_isStarted)
        {
            StartAggregation();
            _isStarted = true;
        }

        AggregateValue((TItem)Convert.ChangeType(value, typeof(TItem)));
    }

    public object? Result => FinalizeAggregation();

    protected abstract void StartAggregation();

    protected abstract void AggregateValue(TItem value);

    protected abstract TAggregate FinalizeAggregation();
}