using System.Collections.Generic;

namespace DatenMeister.Core.EMOF.Interface.Common
{
    public interface IReflectiveCollection : IEnumerable<object>
    {
        bool add(object value);

        bool addAll(IReflectiveSequence value);

        void clear();

        bool remove(object value);

        int size();
    }
}