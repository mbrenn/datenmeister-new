using DatenMeister.Runtime.Functions.Interfaces;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    public class ConcatAggregator : AggregatorByFunction<string, string>
    {
        public ConcatAggregator(string separator = ", ")
            : base (string.Empty, (x,y) => x == string.Empty ? y : $"{x}{separator}{y}" )
        {
        }   
    }
}