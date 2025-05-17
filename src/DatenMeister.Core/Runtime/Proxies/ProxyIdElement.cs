using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.Runtime.Proxies
{
    public class ProxyIdElement : IElement, IHasId
    {
        private readonly IObject _element;

        public ProxyIdElement(IObject element, string id)
        {
            _element = element;
            Id = id;
        }

        public bool equals(object? other)
        {
            return _element.equals(other);
        }

        public object? get(string property)
        {
            return _element.get(property);
        }

        public void set(string property, object? value)
        {
            _element.set(property, value);
        }

        public bool isSet(string property)
        {
            return _element.isSet(property);
        }

        public void unset(string property)
        {
            _element.unset(property);
        }

        public IElement? metaclass =>
            (_element as IElement ?? throw new InvalidOperationException("not an element")).metaclass;

        public IElement? getMetaClass()
        {
            return (_element as IElement ?? throw new InvalidOperationException("not an element")).getMetaClass();
        }

        public IElement? container()
        {
            return (_element as IElement ?? throw new InvalidOperationException("not an element")).container();
        }

        public string? Id { get; }
    }
}