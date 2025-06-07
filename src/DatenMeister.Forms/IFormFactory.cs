using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms;

public interface IFormFactory
{
    IElement? CreateRowFormForItem(IObject element, FormFactoryConfiguration configuration);

    /// <summary>
    /// Creates the detail form by a certain metaclass
    /// </summary>
    /// <param name="metaClass">Metaclass to be used</param>
    /// <param name="creationMode">Creation Mode to be used</param>
    /// <returns></returns>
    IElement? CreateRowFormByMetaClass(IElement metaClass, FormFactoryConfiguration? creationMode);

    IElement? CreateTableFormForCollection(IReflectiveCollection collection, FormFactoryConfiguration configuration);

    /// <summary>
    ///     Creates the list form for a specific meta class.
    /// </summary>
    /// <param name="metaClass">Metaclass to be handled</param>
    /// <param name="configuration">Configuration of the metaclass. </param>
    /// <returns></returns>
    IElement? CreateTableFormForMetaClass(IElement metaClass, FormFactoryConfiguration configuration);

    IElement? CreateTableFormForProperty(IObject? element, string propertyName, IElement? propertyType,
        FormFactoryConfiguration configuration);


    public IElement? CreateTableFormForPropertyValues(IObject element, string propertyName,
        FormFactoryConfiguration configuration)
    {
        return CreateTableFormForProperty(element, propertyName, null, configuration);
    }
}