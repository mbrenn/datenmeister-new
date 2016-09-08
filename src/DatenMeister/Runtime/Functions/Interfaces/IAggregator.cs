namespace DatenMeister.Runtime.Functions.Interfaces
{
    public interface IAggregator<T>
    {
        /// <summary>
        /// Adds a value to the aggregation
        /// </summary>
        /// <param name="value">Value to be added</param>
        void Add(T value);

        /// <summary>
        /// Gets the current value
        /// </summary>
        T Result { get; }
    }
}