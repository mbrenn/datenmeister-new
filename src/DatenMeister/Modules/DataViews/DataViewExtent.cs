using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.DataViews;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Proxies;
using DatenMeister.Runtime.Proxies.ReadOnly;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DataViews
{
    public class DataViewExtent : IUriExtent
    {
        private readonly IElement _dataViewElement;
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly DataViewLogic _dataViewLogic;

        public DataViewExtent(IElement dataViewElement, IWorkspaceLogic workspaceLogic, DataViewLogic dataViewLogic)
        {
            _dataViewElement = dataViewElement ?? throw new ArgumentNullException(nameof(dataViewElement));
            _workspaceLogic = workspaceLogic ?? throw new ArgumentNullException(nameof(workspaceLogic));
            _dataViewLogic = dataViewLogic ?? throw new ArgumentNullException(nameof(dataViewLogic));
        }

        public bool @equals(object other) =>
            _dataViewElement.@equals(other);

        public object get(string property) =>
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
            var viewNode = _dataViewElement.getOrDefault<IElement>(_DataViews._DataView.viewNode);
            if (viewNode == null)
            {
                return result;
            }
            else
            {
                return _dataViewLogic.GetElementsForViewNode(viewNode);
            }
        }

        public string contextURI() =>
            _dataViewElement.getOrDefault<string>(_DataViews._DataView.uri);

        public string uri(IElement element) =>
            element.GetUri();

        public IElement element(string uri)
        {
            return elements().FirstOrDefault(x => x.AsIElement()?.GetUri() == uri) as IElement;
        }
    }
}