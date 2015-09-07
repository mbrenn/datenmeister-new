using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Interface.Reflection
{
    /// <summary>
    /// Interface for the Element according to  MOF CoreSpecification 2.5, Clause 9.2
    /// </summary>
    public interface IElement : IObject
    {
        IElement metaclass
        {
            get;
        }

        IElement getMetaClass();

        IElement container();
    }
}
