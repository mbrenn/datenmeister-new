using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

[Obsolete]
public interface IObjectFormFactory
{
    /// <summary>
    /// Gets the extent form for a certain item
    /// </summary>
    /// <param name="element">Element to which the form is requested</param>
    /// <param name="context">Configuration to be used</param>
    /// <returns>The instance of the extent form</returns>
    IElement? CreateObjectFormForItem(IObject element, FormFactoryContext context);
}