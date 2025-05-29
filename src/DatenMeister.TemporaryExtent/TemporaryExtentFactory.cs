using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.TemporaryExtent;

/// <summary>
/// Defines the IFactory 
/// </summary>
public class TemporaryExtentFactory(TemporaryExtentLogic logic) : IFactory
{
    public IElement? package => null;

    public IElement create(IElement? metaClass)
    {
        return logic.CreateTemporaryElement(metaClass, addToExtent: false);
    }

    public IObject createFromString(IElement dataType, string value)
    {
        throw new NotImplementedException();
    }

    public string convertToString(IElement dataType, IObject value)
    {
        throw new NotImplementedException();
    }
}