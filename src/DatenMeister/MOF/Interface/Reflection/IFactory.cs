using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Interface.Reflection
{
    /// <summary>
    /// Interface to Factory according to MOFSpecification 2.5, Clause 9.3
    /// </summary>
    public interface IFactory
    {
        IElement create(IElement metaClass);
    }
}
