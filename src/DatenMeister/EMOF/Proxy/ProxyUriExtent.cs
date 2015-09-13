using DatenMeister.EMOF.Interface.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyUriExtent : IUriExtent
    {
        private IUriExtent _extent;

        public ProxyUriExtent ( IUriExtent extent)
        {
            _extent = extent;
        }

        public string contextURI()
        {
            return _extent.contextURI();
        }

        public IElement element(string uri)
        {
            return _extent.element(uri);
        }

        public IReflectiveSequence elements()
        {
            return _extent.elements();
        }

        public string uri(IElement element)
        {
            return _extent.uri(element);
        }

        public bool useContainment()
        {
            return _extent.useContainment();
        }
    }
}
