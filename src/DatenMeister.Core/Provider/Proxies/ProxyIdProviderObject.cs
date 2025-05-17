namespace DatenMeister.Core.Provider.Proxies
{
    public class ProxyIdProviderObject : IProviderObject
    {
        private readonly IProviderObject _element;

        public ProxyIdProviderObject(IProviderObject element, string? id)
        {
            _element = element;
            Id = id;
        }

        public IProvider Provider => _element.Provider;

        public string? Id { get; set; }

        public string? MetaclassUri
        {
            get => _element.MetaclassUri;
            set => _element.MetaclassUri = value;
        }

        public bool IsPropertySet(string property)
        {
            return _element.IsPropertySet(property);
        }

        public object? GetProperty(string property, ObjectType objectType = ObjectType.None)
        {
            return _element.GetProperty(property, objectType);
        }

        public IEnumerable<string> GetProperties()
        {
            return _element.GetProperties();
        }

        public bool DeleteProperty(string property)
        {
            return _element.DeleteProperty(property);
        }

        public void SetProperty(string property, object? value)
        {
            _element.SetProperty(property, value);
        }

        public void EmptyListForProperty(string property)
        {
            _element.EmptyListForProperty(property);
        }

        public bool AddToProperty(string property, object value, int index = -1)
        {
            return _element.AddToProperty(property, value, index);
        }

        public bool RemoveFromProperty(string property, object value)
        {
            return _element.RemoveFromProperty(property, value);
        }

        public bool HasContainer()
        {
            return _element.HasContainer();
        }

        public IProviderObject? GetContainer()
        {
            return _element.GetContainer();
        }

        public void SetContainer(IProviderObject? value)
        {
            _element.SetContainer(value);
        }
    }
}