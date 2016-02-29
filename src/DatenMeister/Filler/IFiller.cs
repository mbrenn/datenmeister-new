using System.Collections;
using System.Collections.Generic;

namespace DatenMeister.Filler
{
    public interface IFiller<TFilledType>
    {
        void Fill(IEnumerable<object> collection, TFilledType tree);
    }
}