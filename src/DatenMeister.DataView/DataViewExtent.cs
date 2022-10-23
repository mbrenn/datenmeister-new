using System;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;
using DatenMeister.Core.Runtime.Proxies.ReadOnly;

namespace DatenMeister.DataView
{
    public class DataViewExtent : IUriExtent
    {
        private readonly IElement _dataViewElement;
        private readonly DataViewLogic _dataViewLogic;

        public DataViewExtent(IElement dataViewElement, DataViewLogic dataViewLogic)
        {
            _dataViewElement = dataViewElement ?? throw new ArgumentNullException(nameof(dataViewElement));
            _dataViewLogic = dataViewLogic ?? throw new ArgumentNullException(nameof(dataViewLogic));
        }

        public bool equals(object? other) =>
            _dataViewElement.equals(other);

        public object? get(string property) =>
            _dataViewElement.get(property);

        public void set(string property, object? value)
        {
            _dataViewElement.set(property, value);
        }

        public bool isSet(string property) =>
            _dataViewElement.isSet(property);

        public void unset(string property)
        {
            _dataViewElement.unset(property);
        }

        public bool useContainment() =>
            false;

        public IReflectiveSequence elements()
        {
            var itemResult = new PureReflectiveSequence();
            var result = new ReadOnlyReflectiveSequence(itemResult);
            var viewNode = _dataViewElement.getOrDefault<IElement>(_DatenMeister._DataViews._DataView.viewNode);
            if (viewNode == null)
            {
                return result;
            }

            return new TemporaryReflectiveSequence(_dataViewLogic.GetElementsForViewNode(viewNode));
        }

        public string contextURI() =>
            _dataViewElement.getOrDefault<string>(_DatenMeister._DataViews._DataView.uri);

        public string? uri(IElement element) =>
            element.GetUri();

        public IElement? element(string uri)
        {
            return elements().FirstOrDefault(x => x != null && x.AsIElement()?.GetUri() == uri) as IElement;
        }
    }
}