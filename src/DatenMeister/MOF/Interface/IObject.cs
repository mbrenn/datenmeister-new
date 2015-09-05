using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Interface
{
    /// <summary>
    /// Implements the interface according to MOF Core Specificaton 2.5, clause 9.4
    /// </summary>
    public interface IObject
    {
        bool equals(object other);

        object get(object property);

        void set(object property, object value);

        bool isSet(object property);

        void unset(object property);
    }
}
