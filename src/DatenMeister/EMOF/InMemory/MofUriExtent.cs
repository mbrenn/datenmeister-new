using System.Collections.Generic;
using DatenMeister.EMOF.Attributes;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.InMemory
{
    [AssignFactoryForExtentType(typeof(MofFactory))]
    public class MofUriExtent : IUriExtent, IExtentCachesObject
    {
        private readonly object _syncObject = new object();

        private readonly string _contextUri;

        private readonly ReflectiveSequenceForExtent _reflectiveSequence;

        /// <summary>
        ///     Stores all the elements
        /// </summary>
        private readonly List<object> _elements = new List<object>();

        private readonly ExtentUrlNavigator<MofElement> _extentUrlNavigator;

        public MofUriExtent(string uri)
        {
            _contextUri = uri;
            _reflectiveSequence = new ReflectiveSequenceForExtent(this, new MofReflectiveSequence(_elements));
            _extentUrlNavigator = new ExtentUrlNavigator<MofElement>(this);
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
    }
}