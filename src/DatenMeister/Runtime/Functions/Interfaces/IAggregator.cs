namespace DatenMeister.Runtime.Functions.Interfaces
{
    public interface IAggregator
    {
        /// <summary>
        /// Adds a value to the aggregation
        /// </summary>
        /// <param name="value">Value to be added</param>
        void Add(object value);

        /// <summary>
        /// Gets the current value
        /// </summary>
        object? Result { get; }
    }
}