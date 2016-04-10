using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.InMemory
{
    public class MofExtentReflectiveSequence : MofReflectiveSequence, IExtentCachesObject
    {
        private readonly HashSet<object> _cachedObjects = new HashSet<object>();

        public MofExtentReflectiveSequence(List<object> values) : base(values)
        {
        }

        public override bool add(object value)
        {
            lock (_cachedObjects)
            {
                var result = base.add(value);
                if (result)
                {
                    _cachedObjects.Add(value);
                }

                return result;
            }
        }

        public override void add(int index, object value)
        {
            lock (_cachedObjects)
            {
                base.add(index, value);
                _cachedObjects.Add(value);
            }
        }

        public override bool addAll(IReflectiveSequence values)
        {
            lock (_cachedObjects)
            {
                var result = base.addAll(values);
                if (result)
                {
                    foreach (var value in values)
                    {
                        _cachedObjects.Add(value);
                    }
                }

                return result;
            }
        }

        public override void clear()
        {
            lock (_cachedObjects)
            {
                base.clear();
                _cachedObjects.Clear();
            }
        }

        public override void remove(int index)
        {
            lock (_cachedObjects)
            {
                var value = get(index);

                base.remove(index);

                _cachedObjects.Remove(value);
            }
        }

        public override bool remove(object value)
        {
            lock (_cachedObjects)
            {
                var result = base.remove(value);
                if (result)
                {
                    _cachedObjects.Remove(value);
                }

                return result;
            }
        }

        public override object set(int index, object value)
        {
            lock (_cachedObjects)
            {
                var oldObject = base.set(index, value);

                _cachedObjects.Remove(value);
                _cachedObjects.Add(value);

                return oldObject;
            }
        }

        public bool HasObject(IObject value)
        {
            lock (_cachedObjects)
            {
                return _cachedObjects.Contains(value);
            }
        }
    }
}