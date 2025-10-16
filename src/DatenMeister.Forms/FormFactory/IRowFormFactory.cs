using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Forms.FormFactory;

public record RowFormFactoryParameter : FormFactoryParameterBase
{
    public IObject? Element { get; set; }
}

public interface IRowFormFactory : IFormFactoryBase
{
    public void CreateRowForm(RowFormFactoryParameter parameter, FormCreationContext context, FormCreationResultMultipleForms result);
}