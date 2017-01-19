using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
            if (factoryForTypes == null) throw new ArgumentNullException(nameof(factoryForTypes));
            if (umlHost == null) throw new ArgumentNullException(nameof(umlHost));
            _factoryForTypes = factoryForTypes;
            _umlHost = umlHost;
        }

        /// <summary>
        /// Creates a meta class for the given .Net Type
        /// </summary>
        /// <param name="type">Type to be converted</param>
        /// <returns>The created meta class</returns>
        public IElement CreateTypeFor(Type type)
        {
            var umlClass = _factoryForTypes.create(_umlHost.StructuredClassifiers.__Class);
            var umlClassAsSet = umlClass as ICanSetId;
            if (umlClassAsSet != null)
            {
                umlClassAsSet.Id = type.FullName;
            }

            umlClass.set(_UML._CommonStructure._NamedElement.name, type.Name);

            var properties = new List<IObject>();

            foreach (var property in type.GetProperties())
            {
                var umlProperty = _factoryForTypes.create(_umlHost.Classification.__Property);
                (umlProperty as MofElement)?.SetContainer(umlClass);
                umlProperty.set(_UML._CommonStructure._NamedElement.name, property.Name);
                
                properties.Add(umlProperty);
            }

            umlClass.set(_UML._Classification._Classifier.attribute, properties);

            return umlClass;
        }
    }
}