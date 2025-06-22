using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public class RowFormFactoryParameter
{
    public IObject? Element { get; set; }
    
    public IElement? MetaClass { get; set; }
}

public interface INewRowFormFactory
{
    public void CreateRowForm(RowFormFactoryParameter parameter, NewFormCreationContext context, FormCreationResult result);
}