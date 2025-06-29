using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public class RowFormFactoryParameter : FormFactoryParameterBase
{
    public IObject? Element { get; set; }
}

public interface IRowFormFactory
{
    public void CreateRowForm(RowFormFactoryParameter parameter, FormCreationContext context, FormCreationResult result);
}