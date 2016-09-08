using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetFactory : IFactory
    {
        private readonly IDotNetTypeLookup _typeLookup;

        public DotNetFactory(IDotNetTypeLookup typeLookup)
        {
            _typeLookup = typeLookup;
        }

        public IElement package => null;

        public IElement create(IElement metaClass)
        {
            var type = _typeLookup.ToType(metaClass);
            var result = Activator.CreateInstance(type);
            return _typeLookup.CreateDotNetElementIfNecessary(result, null, null) as IElement;
        }

        public IObject createFromString(IElement dataType, string value)
        {
            throw new System.NotImplementedException();
        }

        public string convertToString(IElement dataType, IObject value)
        {
            throw new System.NotImplementedException();
        }
    }
}