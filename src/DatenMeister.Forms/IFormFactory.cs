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

    /// <summary>
    /// Gets the extent form for a certain item's metaclass.
    /// This method can be used when the object to which a form shall be provided is not available 
    /// </summary>
    /// <param name="metaClass">MetaClass to which the form shall be provided</param>
    /// <param name="configuration">Configuration to be used</param>
    /// <returns>The instance of the extent form</returns>
    IElement? CreateCollectionFormForMetaClass(IElement metaClass, FormFactoryConfiguration configuration);

    IElement? CreateTableFormForCollection(IReflectiveCollection collection, FormFactoryConfiguration configuration);

    /// <summary>
    ///     Creates the list form for a specific meta class.
    /// </summary>
    /// <param name="metaClass">Metaclass to be handled</param>
    /// <param name="configuration">Configuration of the metaclass. </param>
    /// <returns></returns>
    IElement? CreateTableFormForMetaClass(IElement metaClass, FormFactoryConfiguration configuration);

    IElement? CreateCollectionFormForExtent(IExtent extent, IReflectiveCollection collection, FormFactoryConfiguration configuration);

    IElement? CreateTableFormForProperty(IObject? element, string propertyName, IElement? propertyType,
        FormFactoryConfiguration configuration);


    public IElement? CreateTableFormForPropertyValues(IObject element, string propertyName,
        FormFactoryConfiguration configuration)
    {
        return CreateTableFormForProperty(element, propertyName, null, configuration);
    }
}