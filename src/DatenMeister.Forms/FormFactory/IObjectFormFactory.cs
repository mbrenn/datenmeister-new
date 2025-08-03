using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public record ObjectFormFactoryParameter : FormFactoryParameterBase
{
    public IObject? Element { get; set; }
}

public interface IObjectFormFactory
{
    /// <summary>
    /// Gets the extent form for a certain item
    /// </summary>
    /// <param name="parameter">Parameter to be evaluated</param>
    /// <param name="context">Configuration to be used</param>
    /// <param name="result">The result to which the element will be added</param>
    /// <returns>The instance of the extent form</returns>
    void CreateObjectForm(
        ObjectFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm result);
}
