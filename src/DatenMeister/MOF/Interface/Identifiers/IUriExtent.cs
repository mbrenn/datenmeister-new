using DatenMeister.MOF.Interface.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Interface.Identifiers
{
    public interface IUriExtent : IExtent
    {
        string contextURI();

        string uri(IElement element);

        IElement element(string uri);
    }
}
