using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.TemporaryExtent;

/// <summary>
/// Defines a factory that creates elements in a temporary extent. 
/// </summary>
/// <param name="logic">The temporary extent logic to be used</param>
/// <param name="autoAdd">A value indicating whether the created elements should be automatically added
/// to the temporary extent.
/// If true, the element is added to the extent's root elements.
/// If false, the element is created but not added to the extent, requiring manual addition or usage as a contained element.</param>
public class TemporaryExtentFactory(TemporaryExtentLogic logic, bool autoAdd = false) : IFactory
{
    /// <summary>
    /// Gets the package of the factory. Returns null for temporary extent factory.
    /// </summary>
    public IElement? package => null;

    /// <summary>
    /// Creates a new element. 
    /// </summary>
    /// <param name="metaClass">The metaclass of the element to be created</param>
    /// <returns>The created element</returns>
    public IElement create(IElement? metaClass)
    {
        return logic.CreateTemporaryElement(metaClass, addToExtent: autoAdd);
    }

    /// <summary>
    /// Creates an object from a string. Not implemented for temporary extent factory.
    /// </summary>
    /// <param name="dataType">The data type of the object</param>
    /// <param name="value">The string value</param>
    /// <returns>The created object</returns>
    public IObject createFromString(IElement dataType, string value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Converts an object to a string. Not implemented for temporary extent factory.
    /// </summary>
    /// <param name="dataType">The data type of the object</param>
    /// <param name="value">The object value</param>
    /// <returns>The string representation</returns>
    public string convertToString(IElement dataType, IObject value)
    {
        throw new NotImplementedException();
    }
}