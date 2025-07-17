using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public record RowFormFactoryParameter : FormFactoryParameterBase
{
    public IObject? Element { get; set; }
}

public interface IRowFormFactory
{
    public void CreateRowForm(RowFormFactoryParameter parameter, FormCreationContext context, FormCreationResultMultipleForms result);
}