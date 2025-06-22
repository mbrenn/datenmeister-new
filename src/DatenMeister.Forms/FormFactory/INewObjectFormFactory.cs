using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public class ObjectFormFactoryParameter
{
    public IObject? Element { get; set; }
    
    public IElement? MetaClass { get; set; }
}

public interface INewObjectFormFactory
{
    /// <summary>
    /// Gets the extent form for a certain item
    /// </summary>
    /// <param name="element">Element to which the form is requested</param>
    /// <param name="context">Configuration to be used</param>
    /// <param name="result">The result to which the element will be added</param>
    /// <returns>The instance of the extent form</returns>
    void CreateObjectFormForItem(IObject element, NewFormCreationContext context, FormCreationResult result);
    
    void CreateObjectFormForMetaClass(IElement? metaClass, NewFormCreationContext context, FormCreationResult result);
}
