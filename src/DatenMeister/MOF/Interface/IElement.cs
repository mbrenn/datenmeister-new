using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Interface
{
    /// <summary>
    /// Interface for the Element according to  MOF CoreSpecification 2.5, Clause 9.2
    /// </summary>
    public interface IElement : IObject
    {
        IObject getMetaClass();

        IElement container();
    }
}
