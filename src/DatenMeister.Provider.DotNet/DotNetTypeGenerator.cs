using System;
using System.Diagnostics;
using System.Reflection;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetTypeGenerator
    {
        private readonly IFactory _factoryForTypes;

        private readonly _UML _umlHost;

        public DotNetTypeGenerator(IFactory factoryForTypes, _UML umlHost)
        {
            Debug.Assert(umlHost != null, "umlHost != null");
            Debug.Assert(factoryForTypes != null, "factoryForTypes != null");
            this._factoryForTypes = factoryForTypes;
            this._umlHost = umlHost;
        }

        public IElement CreateTypeFor(Type type)
        {
            var result = _factoryForTypes.create(_umlHost.StructuredClassifiers.__Class as IElement);
            result.set(_umlHost.CommonStructure.NamedElement.name, type.Name);

            foreach (var property in type.GetProperties())
            {
                var umlProperty = _factoryForTypes.create(_umlHost.Classification.__Property as IElement);
                umlProperty.set(_umlHost.CommonStructure.NamedElement.name, property);

                // TODO: Add it to the type
            }

            return result;
        }
    }
}