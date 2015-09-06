using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Interface.Common
{
    public interface IReflectiveSequence : IReflectiveCollection
    {
        void add(int index, object value);

        object get(int index);

        void remove(int index);

        object set(int index, object value);        
    }
}
