using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public interface ITableFormFactory
{
    IElement? CreateTableFormForCollection(IReflectiveCollection collection, FormFactoryContext context);

    /// <summary>
    ///     Creates the list form for a specific meta class.
    /// </summary>
    /// <param name="metaClass">Metaclass to be handled</param>
    /// <param name="context">Configuration of the metaclass. </param>
    /// <returns></returns>
    IElement? CreateTableFormForMetaClass(IElement metaClass, FormFactoryContext context);

    IElement? CreateTableFormForProperty(IObject? element, string propertyName, IElement? propertyType,
        FormFactoryContext context);


    public IElement? CreateTableFormForPropertyValues(IObject element, string propertyName,
        FormFactoryContext context)
    {
        return CreateTableFormForProperty(element, propertyName, null, context);
    }
    
}