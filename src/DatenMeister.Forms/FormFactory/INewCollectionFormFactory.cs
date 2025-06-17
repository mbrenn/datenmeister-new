using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public interface INewCollectionFormFactory
{
    /// <summary>
    /// Creates a new collectionform out of the given collection. 
    /// </summary>
    /// <param name="collection">Collection whose data is used to create the form</param>
    /// <param name="context">The context being used</param>
    /// <param name="result">The structure in which the result will be stored</param>
    void CreateCollectionFormForCollection(
        IReflectiveCollection collection,
        NewFormCreationContext context,
        FormCreationResult result);
    
    /// <summary>
    /// Creates a new collectionform out of the given collection. 
    /// </summary>
    /// <param name="metaClass">MetaClass be</param>
    /// <param name="context">The context being used</param>
    /// <param name="result">The structure in which the result will be stored</param>
    void CreateCollectionFormForMetaClass(
        IElement metaClass,
        NewFormCreationContext context,
        FormCreationResult result);
}