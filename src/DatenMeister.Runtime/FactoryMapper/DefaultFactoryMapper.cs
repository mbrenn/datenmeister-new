using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.FactoryMapper
{
    public class DefaultFactoryMapper : IFactoryMapper
    {
        private readonly Dictionary<Type, Func<IFactory>> mapping = 
            new Dictionary<Type, Func<IFactory>>(); 

        public void AddMapping(Type type, Func<IFactory> creator)
        {
            if (mapping.ContainsKey(type))
            {
                throw new InvalidOperationException($"Type {type} is already included in mapping");
            }

            mapping[type] = creator;
        }

        public IFactory FindFactoryFor(Type extentType)
        {
            Func<IFactory> result;

            if (!mapping.TryGetValue(extentType, out result))
            {
                throw new InvalidOperationException($"No factory define for extenttype '{extentType.ToString()}'.");
            }

            return result();
        }
    }
}