﻿using System;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Filler;

namespace DatenMeister.Uml.TypeFactory
{
    /// <summary>
    /// This type factory is capable to create a type out of a dotnet tpye
    /// </summary>
    public class TypeFactoryDotNet
    {
        private readonly _UML _uml;
        private readonly IFactory _factory;

        public TypeFactoryDotNet(_UML uml, IFactory factory)
        {
            _uml = uml;
            _factory = factory;
        }

        public IElement CreateFromDotNetType(Type type)
        {
            var classAsIElement = _uml.StructuredClassifiers.__Class as IElement;
            var typeClass = _factory.create(classAsIElement);
            typeClass.set("name", type.Name);
            
            return typeClass;
        }
    }
}