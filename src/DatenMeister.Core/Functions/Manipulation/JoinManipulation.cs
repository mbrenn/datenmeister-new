using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.Functions.Manipulation;

public static class JoinManipulation
{
    /// <summary>
    /// Matches elements between two collections based on specific fields and merges the properties
    /// from the matching elements into the source collection.
    /// </summary>
    /// <param name="sourceElements">
    /// The collection of source elements where properties will be updated based on matching values.
    /// </param>
    /// <param name="elementsToBeJoined">
    /// The collection of elements to be joined with the source collection.
    /// Properties from matching elements in this collection will be merged into the source elements.
    /// </param>
    /// <param name="sourceFieldName">
    /// The field name in the source elements used for matching with the target collection.
    /// </param>
    /// <param name="targetFieldName">
    /// The field name in the elementsToBeJoined used for matching with the source collection.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when there are duplicate values in the targetFieldName property
    /// of elements in elementsToBeJoined or when the target element is not of a valid type.
    /// </exception>
    public static void JoinSimple(
        IEnumerable<IObject> sourceElements, 
        IEnumerable<IObject> elementsToBeJoined,
        string sourceFieldName, 
        string targetFieldName)
    {
        // Create Dictionary with the targetFieldName properties and take care that
        // the there is only one lement in elementsToBeJoined with that property
        // This Dictionary shall support the indexing
        var index = new Dictionary<object, IObject>();
        foreach (var element in elementsToBeJoined)
        {
            // If element is not set, we cannot join it
            if(!element.isSet(targetFieldName)) continue;
            
            var targetValue = element.get(targetFieldName);
            if(targetValue == null) continue;

            if (!index.TryAdd(targetValue, element))
            {
                throw new InvalidOperationException(
                    $"There is more than one element with the same value: {targetFieldName}={targetValue}");
            }
        }
        
        // Ok, now perform the join
        foreach (var element in sourceElements)
        {
            var sourceValue = element.get(sourceFieldName);
            if (sourceValue == null) continue;

            if (!index.TryGetValue(sourceValue, out var targetElement))
                continue;

            if(targetElement is not IObjectAllProperties asFieldProperties )
                throw new InvalidOperationException("targetElement is not of type IObjectAllProperties");
                
            // Everything seems to be OK, so we can copy
            foreach (var property in asFieldProperties.getPropertiesBeingSet())
            {
                if (property == targetFieldName) continue;
                element.set(property, targetElement.get(property));
            }
        }
    }
}