using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Helper;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetExtent : IUriExtent
    {
        private readonly object _syncObject = new object();

        private readonly string _contextUri;

        private readonly ExtentUrlNavigator<DotNetElement> _navigator;

        private readonly IReflectiveSequence _elements;

        public DotNetExtent(string contextUri, IDotNetTypeLookup typeLookup)
        {
            if (typeLookup == null) throw new ArgumentNullException(nameof(typeLookup));
            if (string.IsNullOrEmpty(contextUri))
                throw new ArgumentException("Value cannot be null or empty.", nameof(contextUri));

            _contextUri = contextUri;
            _navigator = new ExtentUrlNavigator<DotNetElement>(this);

            var reflectiveSequence =
                typeLookup.CreateDotNetReflectiveSequence(new List<object>(), null);
            _elements = new ReflectiveSequenceForExtent(
                this, 
                reflectiveSequence);

            ((IDotNetReflectiveSequence)reflectiveSequence).SetExtent(this);
        }

        public bool useContainment()
        {
            return false;
        }

        public IReflectiveSequence elements()
        {
            return _elements;
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