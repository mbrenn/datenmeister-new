using System.Collections.Generic;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents;

namespace DatenMeister.Provider.InMemory
{
    [AssignFactoryForExtentType(typeof(InMemoryFactory))]
    public class InMemoryUriExtent : IUriExtent, IExtentCachesObject
    {
        private readonly object _syncObject = new object();

        private readonly string _contextUri;

        private readonly ReflectiveSequenceForExtent _reflectiveSequence;

        /// <summary>
        ///     Stores all the elements
        /// </summary>
        private readonly List<object> _elements = new List<object>();

        private readonly ExtentUrlNavigator<InMemoryElement> _extentUrlNavigator;

        public InMemoryUriExtent(string uri)
        {
            _contextUri = uri;
            _reflectiveSequence = new ReflectiveSequenceForExtent(this, new InMemoryReflectiveSequence(_elements));
            _extentUrlNavigator = new ExtentUrlNavigator<InMemoryElement>(this);
        }

        public virtual string contextURI()
        {
            return _contextUri;
        }

        public virtual IElement element(string uri)
        {
            lock (_syncObject)
            {
                return _extentUrlNavigator.element(uri);
            }
        }

        public virtual string uri(IElement element)
        {
            lock (_syncObject)
            {
                return _extentUrlNavigator.uri(element);
            }
        }

        public virtual bool useContainment()
        {
            return false;
        }

        public virtual IReflectiveSequence elements()
        {
            lock (_syncObject)
            {
                return _reflectiveSequence;
            }
        }

        public virtual bool HasObject(IObject value)
        {
            lock (_reflectiveSequence)
            {
                return _reflectiveSequence.HasObject(value);
            }
        }

        public override string ToString()
        {
            return $"{GetType().FullName}: {_elements.Count} items";
        }
    }
}