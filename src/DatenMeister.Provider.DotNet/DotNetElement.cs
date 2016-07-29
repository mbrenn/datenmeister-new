using System.Diagnostics;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetElement : IElement
    {
        private object _value;

        public DotNetElement(object value, IElement type)
        {
            Debug.Assert(type != null, "type != null");
            Debug.Assert(value != null, "value != null");
            _value = value;
            metaclass = type;
        }

        public bool @equals(object other)
        {
            throw new System.NotImplementedException();
        }

        public object get(string property)
        {
            throw new System.NotImplementedException();
        }

        public void set(string property, object value)
        {
            throw new System.NotImplementedException();
        }

        public bool isSet(string property)
        {
            throw new System.NotImplementedException();
        }

        public void unset(string property)
        {
            throw new System.NotImplementedException();
        }

        public IElement metaclass { get; }

        public IElement getMetaClass()
        {
            throw new System.NotImplementedException();
        }

        public IElement container()
        {
            throw new System.NotImplementedException();
        }
    }
}