using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public class CollectionFormFactoryParameter
{
    public IReflectiveCollection? Collection { get; set; }
    
    public IElement? MetaClass { get; set; }
}

public interface INewCollectionFormFactory
{
    /// <summary>
    /// Creates a new collectionform out of the given collection. 
    /// </summary>
    /// <param name="parameter">Parameter whose data is used to create the form</param>
    /// <param name="context">The context being used</param>
    /// <param name="result">The structure in which the result will be stored</param>
    void CreateCollectionFormForCollection(
        CollectionFormFactoryParameter parameter,
        NewFormCreationContext context,
        FormCreationResult result);
}