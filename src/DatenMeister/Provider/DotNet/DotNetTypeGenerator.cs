using System;
using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;

namespace DatenMeister.Provider.DotNet
{
    /// <summary>
    /// Takes a .Net Type and converts it to a UML metaclass which can be used within 
    /// the DatenMeister.
    /// </summary>
    public class DotNetTypeGenerator
    {
        private readonly IFactory _factoryForTypes;

        private readonly _UML _umlHost;

        /// <summary>
        /// Initializes a new instance of the DotNetTypeGenerator class
        /// </summary>
        /// <param name="factoryForTypes">The factory being used to create the instances for
        /// class, properties and other MOF elements</param>
        /// <param name="umlHost">The UML reference storing the metaclass for class, properties, etc. </param>
        public DotNetTypeGenerator(IFactory factoryForTypes, _UML umlHost)
        {
            _factoryForTypes = factoryForTypes ?? throw new ArgumentNullException(nameof(factoryForTypes));
            _umlHost = umlHost ?? throw new ArgumentNullException(nameof(umlHost));
        }

        public IEnumerable<IElement> CreateTypesFor(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var element = CreateTypeFor(type);
                if (element != null)
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Creates a meta class for the given .Net Type
        /// </summary>
        /// <param name="type">Type to be converted</param>
        /// <returns>The created meta class</returns>
        public IElement CreateTypeFor(Type type)
        {
            if (type.IsClass)
            {
                var umlClass = _factoryForTypes.create(_umlHost.StructuredClassifiers.__Class);
                if (umlClass is ICanSetId umlClassAsSet)
                {
                    umlClassAsSet.Id = type.FullName;
                }

                umlClass.set(_UML._CommonStructure._NamedElement.name, type.Name);

                var properties = new List<IObject>();

                foreach (var property in type.GetProperties())
                {
                    var umlProperty = _factoryForTypes.create(_umlHost.Classification.__Property);
                    if (umlProperty is MofElement propertyAsElement)
                    {
                        propertyAsElement.Container = umlClass;
                    }

                    umlProperty.set(_UML._CommonStructure._NamedElement.name, property.Name);

                    properties.Add(umlProperty);
                }

                umlClass.set(_UML._StructuredClassifiers._StructuredClassifier.ownedAttribute, properties);

                return umlClass;
            }

            if (type.IsEnum)
            {
                var enumClass = _factoryForTypes.create(_umlHost.SimpleClassifiers.__Enumeration);
                if (enumClass is ICanSetId umlClassAsSet)
                {
                    umlClassAsSet.Id = type.FullName;
                }

                enumClass.set(_UML._CommonStructure._NamedElement.name, type.Name);

                var enumValues = new List<IObject>();
                foreach (var enumValue in type.GetEnumValues())
                {
                    var enumValueClass = _factoryForTypes.create(_umlHost.SimpleClassifiers.__EnumerationLiteral);
                    if (enumValueClass is ICanSetId umlValueClassAsSet)
                    {
                        umlValueClassAsSet.Id = $"{type.FullName}-{enumValue}";
                    }

                    enumValueClass.set(_UML._CommonStructure._NamedElement.name, enumValue.ToString());

                    enumValues.Add(enumValueClass);
                }

                enumClass.set(_UML._SimpleClassifiers._Enumeration.ownedLiteral, enumValues);
                return enumClass;
            }

            return null;
        }
    }
}