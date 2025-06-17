using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

[Obsolete]
public interface ICollectionFormFactory
{
    /// <summary>
    /// Gets the extent form for a certain item's metaclass.
    /// This method can be used when the object to which a form shall be provided is not available 
    /// </summary>
    /// <param name="metaClass">MetaClass to which the form shall be provided</param>
    /// <param name="context">Configuration to be used</param>
    /// <returns>The instance of the extent form</returns>
    IElement? CreateCollectionFormForMetaClass(IElement metaClass, FormFactoryContext context);

    IElement? CreateCollectionFormForCollection(
        IExtent extent,
        IReflectiveCollection collection,
        FormFactoryContext context);
}