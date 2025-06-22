using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public class RowFormFactoryParameter
{
    public IObject? Element { get; set; }
    
    public IElement? MetaClass { get; set; }
}

public interface INewRowFormFactory
{
    public void CreateRowFormForItem(IObject element, NewFormCreationContext context, FormCreationResult result);
    
    public void CreateRowFormForMetaClass(IElement metaClass, NewFormCreationContext context, FormCreationResult result);   
}