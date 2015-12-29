using System;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyUriExtent : IUriExtent
    {
        protected readonly IUriExtent Extent;

        /// <summary>
        /// Gets or sets the conversion method being used, when content of the 
        /// reflective collection is being returned. 
        /// </summary>
        public Func<IElement, IElement> PublicizeElementFunc { get; set; }

        /// <summary>
        /// Gets or sets the conversion method being used, when content of the 
        /// reflective collection is being returned. 
        /// </summary>
        public Func<IElement, IElement> PrivatizeElementFunc { get; set; }

        /// <summary>
        /// Gets or sets the conversion method being used, when content of the 
        /// reflective collection is being returned. 
        /// </summary>
        public Func<IReflectiveSequence, IReflectiveSequence> PublicizeReflectiveSequenceFunc { get; set; }

        public ProxyUriExtent(IUriExtent extent)
        {
            Extent = extent;
            PublicizeElementFunc = x => x;
            PublicizeReflectiveSequenceFunc = x => x;
            PrivatizeElementFunc = x => x;
        }

        public ProxyUriExtent ActivateObjectConversion()
        {
            PublicizeElementFunc = x => new ProxyMofElement(x);
            PublicizeReflectiveSequenceFunc = x => 
                new ProxyReflectiveSequence(x).ActivateObjectConversion();
            PrivatizeElementFunc = x =>
            {
                var element = x as ProxyMofElement;
                return element != null ? element.GetProxiedElement() : x;
            };

            return this;
        }

        public virtual string contextURI()
        {
            return Extent.contextURI();
        }

        public virtual IElement element(string uri)
        {
            return PublicizeElementFunc(Extent.element(uri));
        }

        public virtual IReflectiveSequence elements()
        {
            return PublicizeReflectiveSequenceFunc(Extent.elements());
        }

        public virtual string uri(IElement element)
        {
            return Extent.uri(PrivatizeElementFunc(element));
        }

        public virtual bool useContainment()
        {
            return Extent.useContainment();
        }
    }
}
