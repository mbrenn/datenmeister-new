using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Forms.FormFactory;

public class CollectionFormFactoryParameter : FormFactoryParameterBase
{
    public IReflectiveCollection? Collection { get; set; }
}

public interface ICollectionFormFactory
{
    /// <summary>
    /// Creates a new collectionform out of the given collection. 
    /// </summary>
    /// <param name="parameter">Parameter whose data is used to create the form</param>
    /// <param name="context">The context being used</param>
    /// <param name="result">The structure in which the result will be stored</param>
    void CreateCollectionForm(
        CollectionFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResult result);
}