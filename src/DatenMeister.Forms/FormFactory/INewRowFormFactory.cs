using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public class RowFormFactoryParameter : FormFactoryParameterBase
{
    public IObject? Element { get; set; }
}

public interface INewRowFormFactory
{
    public void CreateRowForm(RowFormFactoryParameter parameter, NewFormCreationContext context, FormCreationResult result);
}