using DatenMeister.MOF.Interface;
using DatenMeister.MOF.Interface.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.InMemory
{
    /// <summary>
    /// Implements the interface according to MOF Core Specificaton 2.5, clause 9.4
    /// </summary>
    public class MofFactory : IFactory
    {
        public IElement package
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IElement create(IElement metaClass)
        {
            return new MofElement(null, metaClass);
        }
        
        public string convertToString(IElement dataType, IObject value)
        {
            throw new NotImplementedException();
        }

        public IObject createFromString(IElement dataType, string value)
        {
            throw new NotImplementedException();
        }
    }
}
