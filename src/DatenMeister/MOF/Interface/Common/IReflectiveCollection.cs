using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Interface.Common
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
