using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetFactory : IFactory
    {
        private readonly DotNetExtent _extent;
        private readonly IDotNetTypeLookup _typeLookup;

        public DotNetFactory(DotNetExtent extent, IDotNetTypeLookup typeLookup)
        {
            _extent = extent;
            _typeLookup = typeLookup;
        }

        public IElement package => null;

        public IElement create(IElement metaClass)
        {
            var type = _typeLookup.ToType(metaClass);
            var result = Activator.CreateInstance(type);
            return _typeLookup.CreateDotNetElementIfNecessary(result, null, _extent) as IElement;
        }

        public IObject createFromString(IElement dataType, string value)
        {
            throw new NotImplementedException();
        }

        public string convertToString(IElement dataType, IObject value)
        {
            throw new NotImplementedException();
        }
    }
}