using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies
{
    public class ProxyUriExtent : IUriExtent
    {
        protected readonly IUriExtent Extent;

        /// <summary>
        /// Gets or sets the conversion method being used, when content of the
        /// reflective collection is being returned.
        /// </summary>
        public Func<IElement?, IElement?> PublicizeElementFunc { get; set; }

        /// <summary>
        /// Gets or sets the conversion method being used, when content of the
        /// reflective collection is being returned.
        /// </summary>
        public Func<IElement?, IElement?> PrivatizeElementFunc { get; set; }

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

        public virtual string contextURI() => Extent.contextURI();

        public virtual IElement? element(string uri) => PublicizeElementFunc(Extent.element(uri));

        public virtual IReflectiveSequence elements() => PublicizeReflectiveSequenceFunc(Extent.elements());

        public virtual string? uri(IElement element)
        {
            var convertedElement = PrivatizeElementFunc(element);
            if (convertedElement == null) return null;
            return Extent.uri(convertedElement);
        }

        public virtual bool useContainment() => Extent.useContainment();

        public ProxyUriExtent ActivateObjectConversion()
        {
            return ActivateObjectConversion(
                x => x == null ? null : new ProxyMofElement((MofElement) x),
                x => new ProxyReflectiveSequence(x),
                x => x?.GetProxiedElement());
        }

        public ProxyUriExtent ActivateObjectConversion<TElementType>(
            Func<IElement?, TElementType?> publicizeElement,
            Func<IReflectiveSequence, IReflectiveSequence> publicizeReflectiveSequence,
            Func<TElementType?, IElement?> privatizeElement)
            where TElementType : class, IElement
        {
            PublicizeElementFunc = publicizeElement;
            PublicizeReflectiveSequenceFunc = publicizeReflectiveSequence;
            PrivatizeElementFunc = x => x is TElementType element ? privatizeElement(element) : x;

            return this;
        }

        /// <inheritdoc />
        public bool @equals(object? other) => Extent.@equals(other);

        /// <inheritdoc />
        public object? get(string property) => Extent.get(property);

        /// <inheritdoc />
        public void set(string property, object? value)
        {
            Extent.set(property, value);
        }

        /// <inheritdoc />
        public bool isSet(string property) => Extent.isSet(property);

        /// <inheritdoc />
        public void unset(string property)
        {
            Extent.unset(property);
        }
    }
}
