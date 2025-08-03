namespace DatenMeister.Core.Extensions.Functions.Interfaces;

public interface IAggregator
{
    /// <summary>
    /// Gets the current value
    /// </summary>
    object? Result { get; }

    /// <summary>
    /// Adds a value to the aggregation
    /// </summary>
    /// <param name="value">Value to be added</param>
    void Add(object value);
}