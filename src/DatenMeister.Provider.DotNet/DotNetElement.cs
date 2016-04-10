using System.Diagnostics;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetElement : IElement
    {
        private object _value;

        /// <summary>
        /// Defines the type of the element as being stored in Uml
        /// </summary>
        private IElement _type;

        public DotNetElement(object value, IElement type)
        {
            Debug.Assert(type != null, "type != null");
            Debug.Assert(value != null, "value != null");
            _value = value;
            _type = type;
        }

        public bool @equals(object other)
        {
            throw new System.NotImplementedException();
        }

        public object get(object property)
        {
            throw new System.NotImplementedException();
        }

        public void set(object property, object value)
        {
            throw new System.NotImplementedException();
        }

        public bool isSet(object property)
        {
            throw new System.NotImplementedException();
        }

        public void unset(object property)
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