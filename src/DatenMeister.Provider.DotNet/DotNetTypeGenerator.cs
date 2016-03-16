using System;
using System.Collections.Generic;
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
            var result = _factoryForTypes.create(_umlHost.StructuredClassifiers.__Class);
            result.set(_umlHost.CommonStructure.NamedElement.name, type.Name);

            var properties = new List<IObject>();
            foreach (var property in type.GetProperties())
            {
                var umlProperty = _factoryForTypes.create(_umlHost.Classification.__Property);
                umlProperty.set(_umlHost.CommonStructure.NamedElement.name, property.Name);
                
                properties.Add(umlProperty);
            }

            result.set(_umlHost.Classification.Classifier.attribute, properties);

            return result;
        }
    }
}