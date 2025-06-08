using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public interface IRowFormFactory
{
    IElement? CreateRowFormForItem(IObject element, FormFactoryConfiguration configuration);

    /// <summary>
    /// Creates the detail form by a certain metaclass
    /// </summary>
    /// <param name="metaClass">Metaclass to be used</param>
    /// <param name="creationMode">Creation Mode to be used</param>
    /// <returns></returns>
    IElement? CreateRowFormByMetaClass(IElement metaClass, FormFactoryConfiguration? creationMode);
}