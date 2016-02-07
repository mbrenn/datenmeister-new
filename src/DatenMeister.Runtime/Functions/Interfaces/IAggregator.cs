using DatenMeister.EMOF.Interface.Common;

namespace DatenMeister.Runtime.Functions.Interfaces
{
    public interface IAggregator<T>
    {
        /// <summary>
        /// Aggregates the values of a property into a single value
        /// </summary>
        /// <param name="sequence">Sequence to be evaluated</param>
        /// <returns>The aggregated value</returns>
        T Aggregate(IReflectiveSequence sequence);
    }
}