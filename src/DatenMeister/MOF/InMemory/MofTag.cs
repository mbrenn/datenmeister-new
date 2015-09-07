using DatenMeister.MOF.Interface.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.MOF.Interface.Common;
using DatenMeister.MOF.Interface.Reflection;

namespace DatenMeister.MOF.InMemory
{
    public class MofTag : ITag
    {
        public string name
        {
            get;
            set;
        }

        public string value
        {
            get;
            set;
        }
        
        public IReflectiveCollection elements
        {
            get;
            set;
        }

        public IElement owner
        {
            get;
            set;
        }

    }
}
