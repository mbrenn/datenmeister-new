using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Helper;
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
        private readonly IDotNetTypeLookup _typeLookup;

        private readonly ExtentUrlNavigator<DotNetElement> _navigator;

        private readonly IReflectiveSequence _elements;

        public DotNetExtent(string contextUri, IDotNetTypeLookup typeLookup)
        {
            if (typeLookup == null) throw new ArgumentNullException(nameof(typeLookup));
            if (string.IsNullOrEmpty(contextUri))
                throw new ArgumentException("Value cannot be null or empty.", nameof(contextUri));

            _contextUri = contextUri;
            _typeLookup = typeLookup;
            _navigator = new ExtentUrlNavigator<DotNetElement>(this);
            _elements = new ReflectiveSequenceForExtent(
                this, 
                _typeLookup.CreateDotNetReflectiveSequence(new List<object>()));
        }

        public bool useContainment()
        {
            return false;
        }

        public IReflectiveSequence elements()
        {
            return _elements;
            throw new NotImplementedException();
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