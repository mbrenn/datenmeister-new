using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetElement : IElement
    {
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