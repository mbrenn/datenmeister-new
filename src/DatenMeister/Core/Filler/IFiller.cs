using System.Collections.Generic;

namespace DatenMeister.Core.Filler
{
    public interface IFiller<TFilledType>
    {
        void Fill(IEnumerable<object?> collection, TFilledType tree);
    }
}