using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Core.EMOF.Implementation.AutoEnumerate
{
    public static class AutoEnumerateHandler
    {


        /// <summary>
        /// Saves the type of the extent
        /// </summary>
        public const string AutoEnumerateTypeProperty = "__AutoEnumerateType";
        
        /// <summary>
        /// Saves the type of the extent
        /// </summary>
        public const string AutoEnumerateTypeValue = "__AutoEnumerateValue";
        
        /// <summary>
        /// This method needs to be called when a new item is created.
        /// It will check whether one of the items has an auto enumerated property 
        /// </summary>
        /// <param name="parentExtent">Extent of the parent</param>
        /// <param name="element">Element to be added</param>
        public static void HandleNewItem(IExtent parentExtent, IElement element)
        {
            var autoEnumerateType = parentExtent.GetConfiguration().AutoEnumerateType;
            if (autoEnumerateType != AutoEnumerateType.Guid && autoEnumerateType != AutoEnumerateType.Ordinal)
            {
                autoEnumerateType = AutoEnumerateType.Guid;
            }

            // Gets the type and the associated properties
            var type = element.getMetaClass();
            if (type == null)
            {
                // Nothing to do
                return;
            }

            var properties = ClassifierMethods.GetPropertiesOfClassifier(type);

            string? id = null;
            foreach (var property in properties)
            {
                var isId = property.getOrDefault<bool>(_UML._Classification._Property.isID);

                if (isId)
                {
                    var name = property.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
                    switch (autoEnumerateType)
                    {
                        case AutoEnumerateType.Guid:
                            id ??= Guid.NewGuid().ToString();
                            break;
                        case AutoEnumerateType.Ordinal:
                        {
                            var lastValue = parentExtent.getOrDefault<int>(AutoEnumerateTypeValue);
                            if (lastValue == 0)
                            {
                                foreach (var innerElement in parentExtent.elements().GetAllDescendants())
                                {
                                    if (innerElement is IElement innerElementAsElement)
                                    {
                                        var idNumber = innerElementAsElement.getOrDefault<int>(name);
                                        if (idNumber > lastValue)
                                        {
                                            lastValue = idNumber;
                                        }
                                    }
                                }
                            }

                            if (id == null)
                            {
                                lastValue += 1;
                                id = lastValue.ToString();
                                parentExtent.set(AutoEnumerateTypeValue, lastValue);
                            }

                            break;
                        }
                    }

                    // Sets the id
                    if (id != null)
                    {
                        element.set(name, id);

                        if (element is ICanSetId canSetId)
                        {
                            canSetId.Id = id;
                        }
                    }
                }
            }
        }
    }
}