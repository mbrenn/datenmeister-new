using System;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetExtent : IUriExtent
    {
        private readonly object _syncObject = new object();

        private readonly string _contextUri;

        private readonly ExtentUrlNavigator<DotNetElement> _navigator;

        public DotNetExtent(string contextUri)
        {
            _contextUri = contextUri;
            _navigator = new ExtentUrlNavigator<DotNetElement>(this);
        }

        public bool useContainment()
        {
            return false;
        }

        public IReflectiveSequence elements()
        {
            throw new System.NotImplementedException();
        }

        public string contextURI()
        {
            return _contextUri;
        }

        public string uri(IElement element)
        {
            lock (_syncObject)
            {
                return _navigator.uri(element);
            }
        }

        public IElement element(string uri)
        {
            lock (_syncObject)
            {
                return _navigator.element(uri);
            }
        }
    }
}