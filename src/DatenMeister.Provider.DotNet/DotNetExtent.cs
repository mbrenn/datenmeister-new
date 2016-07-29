using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetExtent : IUriExtent
    {
        public bool useContainment()
        {
            throw new System.NotImplementedException();
        }

        public IReflectiveSequence elements()
        {
            throw new System.NotImplementedException();
        }

        public string contextURI()
        {
            throw new System.NotImplementedException();
        }

        public string uri(IElement element)
        {
            throw new System.NotImplementedException();
        }

        public IElement element(string uri)
        {
            throw new System.NotImplementedException();
        }
    }
}