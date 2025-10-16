using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Core.EMOF.Implementation.AutoEnumerate;

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
            autoEnumerateType = AutoEnumerateType.None;
        }

        if (autoEnumerateType == AutoEnumerateType.None)
        {
            return;
        }

        // Gets the type and the associated properties

        string? id = null;
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
                    foreach (var innerElement in parentExtent.elements().GetAllDescendantsIncludingThemselves())
                    {
                        if (innerElement is IElement innerElementAsElement 
                            && innerElementAsElement is IHasId asHasId 
                            && int.TryParse(asHasId.Id, out var foundId))
                        {
                            var idNumber = foundId;
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
            if (element is ICanSetId canSetId)
            {
                canSetId.Id = id;
            }
        }
        
            
        var type = element.getMetaClass();
        if (type == null)
        {
            // Nothing to do
            return;
        }

        var properties = ClassifierMethods.GetPropertiesOfClassifier(type);
        foreach (var property in properties)
        {
            var isId = property.getOrDefault<bool>(_UML._Classification._Property.isID);

            if (isId)
            {
                var name = property.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
                    
                element.set(name, id);
            }
        }
    }
}