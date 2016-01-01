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

        public ProxyUriExtent ActivateObjectConversion()
        {
            return ActivateObjectConversion(
                x => new ProxyMofElement(x),
                x => new ProxyReflectiveSequence(x),
                x => x.GetProxiedElement());
        }

        public ProxyUriExtent ActivateObjectConversion<TElementType>(
            Func<IElement, TElementType> publicizeElement,
            Func<IReflectiveSequence, IReflectiveSequence> publicizeReflectiveSequence,
            Func<TElementType, IElement> privatizeElement)
            where TElementType : class, IElement
        {
            PublicizeElementFunc = publicizeElement;
            PublicizeReflectiveSequenceFunc = publicizeReflectiveSequence;
            PrivatizeElementFunc = x =>
            {
                var element = x as TElementType;
                return element != null ?privatizeElement(element) : x;
            };

            return this;
        }
    }
}
