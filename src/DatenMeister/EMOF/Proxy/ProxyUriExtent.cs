using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyUriExtent : IUriExtent
    {
        protected IUriExtent Extent;

        public ProxyUriExtent(IUriExtent extent)
        {
            Extent = extent;
        }

        public virtual string contextURI()
        {
            return Extent.contextURI();
        }

        public virtual IElement element(string uri)
        {
            return Extent.element(uri);
        }

        public virtual IReflectiveSequence elements()
        {
            return Extent.elements();
        }

        public virtual string uri(IElement element)
        {
            return Extent.uri(element);
        }

        public virtual bool useContainment()
        {
            return Extent.useContainment();
        }
    }
}
