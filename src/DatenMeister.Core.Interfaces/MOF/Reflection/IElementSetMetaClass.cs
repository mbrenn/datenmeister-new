namespace DatenMeister.Core.Interfaces.MOF.Reflection;

public interface IElementSetMetaClass
{
    /// <summary>
    /// Sets the container of the given element in a hard way
    /// </summary>
    IObject Container { set; }

    /// <summary>
    ///     Offers to set the meta class after creation
    /// </summary>
    /// <param name="metaClass">Metaclass to be set</param>
    void SetMetaClass(IElement? metaClass);
}