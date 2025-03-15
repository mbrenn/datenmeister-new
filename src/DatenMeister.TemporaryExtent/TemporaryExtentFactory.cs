
using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.TemporaryExtent;

/// <summary>
/// Defines the IFactory 
/// </summary>
public class TemporaryExtentFactory : IFactory
{
    private readonly TemporaryExtentLogic _logic;
    private IFactory _internalFactory;

    public TemporaryExtentFactory(TemporaryExtentLogic logic)
    {
        _logic = logic;
        _internalFactory = new MofFactory(logic.TemporaryExtent);
    }

    public IElement package { get; }
    public IElement create(IElement? metaClass)
    {
        return _logic.CreateTemporaryElement(metaClass);
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