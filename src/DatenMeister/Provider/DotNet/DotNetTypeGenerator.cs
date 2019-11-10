using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Provider.DotNet
{
    /// <summary>
    /// Defines the options for the DotNet Type Generator
    /// </summary>
    public class DotNetTypeGeneratorOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether the properties
        /// of the inherited classes shall be integrated into the existing class or
        /// whether the generalizations shall be used.
        /// </summary>
        public bool IntegrateInheritedProperties { get; set; } = true;
    }

    /// <summary>
    /// Takes a .Net Type and converts it to a UML metaclass which can be used within
    /// the DatenMeister.
    /// </summary>
    public class DotNetTypeGenerator
    {
        private readonly IFactory _factoryForTypes;

        private readonly _UML _umlHost;

        private readonly IExtent _targetExtent;

        public IUriResolver UriResolver => _targetExtent as IUriResolver;

        /// <summary>
        /// Initializes a new instance of the DotNetTypeGenerator class
        /// </summary>
        /// <param name="factoryForTypes">The factory being used to create the instances for
        /// class, properties and other MOF elements</param>
        /// <param name="umlHost">The UML reference storing the metaclass for class, properties, etc. </param>
        /// <param name="targetExtent">Stores the extent into which the elements will be added</param>
        public DotNetTypeGenerator(IFactory factoryForTypes, _UML umlHost, IExtent targetExtent = null)
        {
            _factoryForTypes = factoryForTypes ?? throw new ArgumentNullException(nameof(factoryForTypes));
            _umlHost = umlHost ?? throw new ArgumentNullException(nameof(umlHost));
            _targetExtent = targetExtent;
        }

        /// <summary>
        /// Initializes a new instance of the DotNetTypeGenerator class
        /// </summary>
        /// <param name="umlHost">The UML reference storing the metaclass for class, properties, etc. </param>
        /// <param name="targetExtent">Stores the extent into which the elements will be added</param>
        public DotNetTypeGenerator(IExtent targetExtent, _UML umlHost)
        {
            _targetExtent = targetExtent ?? throw new ArgumentNullException(nameof(targetExtent));
            _factoryForTypes = new MofFactory(_targetExtent);
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
        public IElement CreateTypeFor(Type type, DotNetTypeGeneratorOptions options = null)
        {
            options ??= new DotNetTypeGeneratorOptions();

            if (type.IsClass)
            {
                var umlClass = _factoryForTypes.create(_umlHost.StructuredClassifiers.__Class);
                if (umlClass is ICanSetId umlClassAsSet)
                {
                    umlClassAsSet.Id = type.FullName;
                }

                umlClass.set(_UML._CommonStructure._NamedElement.name, type.Name);

                // Goes through the generalizations, if configured
                if (options.IntegrateInheritedProperties)
                {
                    var generalization = type.BaseType;
                    if (generalization != null && generalization != typeof(object))
                    {
                        var generalizedClass = ((MofExtent) _targetExtent).ToResolvedElement(generalization);

                        if (generalizedClass == null)
                        {
                            throw new InvalidOperationException(
                                $"Generalization for {type} -> {generalization} was not found");
                        }

                        // We got a generalization
                        ClassifierMethods.AddGeneralization(_umlHost, umlClass, generalizedClass);
                    }
                }

                // Goes through all the properties
                var properties = new List<IObject>();
                foreach (var property in type.GetProperties())
                {
                    // If property is inherited, do not include the property, if not configured
                    if (property.DeclaringType != type && options.IntegrateInheritedProperties)
                        continue;

                    var umlProperty = _factoryForTypes.create(_umlHost.Classification.__Property);
                    if (umlProperty is MofElement propertyAsElement)
                    {
                        propertyAsElement.Container = umlClass;
                    }

                    umlProperty.set(_UML._CommonStructure._NamedElement.name, property.Name);

                    // Ok, now we start to set the types... it will be fun
                    if (UriResolver != null)
                    {
                        SetProperty(property.PropertyType, umlProperty);
                    }

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

        /// <summary>
        /// Sets the type information for the given property, depending on the property information.
        /// </summary>
        /// <param name="property">Property that is evaluated</param>
        /// <param name="umlProperty">Property which will have the property type stored according UML</param>
        private void SetProperty(Type property, IObject umlProperty)
        {
            if (property == typeof(string))
            {
                var stringType = UriResolver.Resolve(CoreTypeNames.StringType,
                    ResolveType.NoMetaWorkspaces);
                umlProperty.set(_UML._CommonStructure._TypedElement.type, stringType);
            }
            else if (property == typeof(int) || property == typeof(long) || property == typeof(short))
            {
                var integerType = UriResolver.Resolve(CoreTypeNames.IntegerType,
                    ResolveType.NoMetaWorkspaces);
                umlProperty.set(_UML._CommonStructure._TypedElement.type, integerType);
            }
            else if (property == typeof(bool))
            {
                var booleanType = UriResolver.Resolve(CoreTypeNames.BooleanType,
                    ResolveType.NoMetaWorkspaces);
                umlProperty.set(_UML._CommonStructure._TypedElement.type, booleanType);
            }
            else if (property == typeof(double) || property == typeof(float))
            {
                var realType = UriResolver.Resolve(CoreTypeNames.RealType,
                    ResolveType.NoMetaWorkspaces);
                umlProperty.set(_UML._CommonStructure._TypedElement.type, realType);
            }
            else if (property == typeof(DateTime))
            {
                var dateTimeType = UriResolver.Resolve(CoreTypeNames.DateTimeType,
                    ResolveType.NoMetaWorkspaces);
                umlProperty.set(_UML._CommonStructure._TypedElement.type, dateTimeType);
            }
            else if (property.IsEnum)
            {
                var typeUri = (_targetExtent as MofExtent)?.TypeLookup.ToElement(property);
                if (typeUri != null)
                {
                    var enumType = UriResolver.Resolve(typeUri, ResolveType.NoMetaWorkspaces);
                    umlProperty.set(_UML._CommonStructure._TypedElement.type, enumType);
                }
                else
                {
                    umlProperty.set(_UML._CommonStructure._TypedElement.type,
                        new MofObjectShadow($"#{property.FullName}"));
                }
            }
            else if (property == typeof(IObject) || property == typeof(IElement))
            {
                // Element is not defined by the given type, so set null
                umlProperty.set(_UML._CommonStructure._TypedElement.type, null);                
            }
            else
            {
                // Ok... new type
                // If type is enumeration, get the original type
                var propertyType = GetAnyElementType(property);
                if (propertyType != property)
                {
                    SetProperty(propertyType, umlProperty);
                    umlProperty.set(_UML._CommonStructure._MultiplicityElement.upper, 2);
                }
                else
                {
                    var propertyMofType = ((MofExtent) _targetExtent).TypeLookup.ToElement(propertyType);

                    if (propertyMofType != null)
                    {
                        var enumType = UriResolver.Resolve(propertyMofType, ResolveType.NoMetaWorkspaces);
                        umlProperty.set(_UML._CommonStructure._TypedElement.type, enumType);
                    }
                    else
                    {
                        umlProperty.set(_UML._CommonStructure._TypedElement.type,
                            new MofObjectShadow($"#{WebUtility.UrlEncode(propertyType.FullName)}"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the type of the enumeration, if the type is an enumeration.
        /// Otherwise the type itself will be returned
        /// </summary>
        /// <param name="originalType"></param>
        /// <returns></returns>
        public static Type GetAnyElementType(Type originalType)
        {
            var type = originalType;
            // Short-circuit for Array types
            if (typeof(Array).IsAssignableFrom(type))
            {
                return type.GetElementType();
            }

            while (true)
            {
                // Type is IEnumerable<T>
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return type.GetGenericArguments().First();
                }

                // Type implements/extends IEnumerable<T>
                var elementType = (from subType in type.GetInterfaces()
                    let retType = GetAnyElementType(subType)
                    where retType != subType
                    select retType).FirstOrDefault();

                if (elementType != null)
                {
                    return elementType;
                }

                if (type.BaseType == null)
                {
                    // Ok, we found nothing... return the original one
                    return originalType;
                }

                type = type.BaseType;
            }
        }
    }
}