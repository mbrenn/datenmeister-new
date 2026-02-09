using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.CollectionForms;

/// <summary>
/// The factory which creates an empty collection form. 
/// </summary>
public class EmptyCollectionFormFactory() : FormFactoryBase, ICollectionFormFactory
{
    public void CreateCollectionForm(
        CollectionFormFactoryParameter parameter, 
        FormCreationContext context,
        FormCreationResultOneForm result)
    {
        if (result.Form == null)
        {
            result.Form = context.Global.FactoryForForms.create(_Forms.TheOne.__CollectionForm);
            result.IsManaged = true;
            result.AddToFormCreationProtocol(
                "[EmptyCollectionFormFactory] Empty object Collection-Form created");
        }
    }
}