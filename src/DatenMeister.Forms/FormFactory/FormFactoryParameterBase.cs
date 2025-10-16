using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Forms.FormFactory;

public abstract record FormFactoryParameterBase
{
    /// <summary>
    /// Defines the Extenttypes to which the current request belongs to 
    /// </summary>
    public IEnumerable<string>? ExtentTypes { get; set; }
    
    /// <summary>
    /// Defines the extent to which the current of a form belongs to 
    /// </summary>
    public IUriExtent? Extent { get; set; } 
    
    /// <summary>
    /// Defines the metaclass to which the current request of a form belongs to 
    /// </summary>
    public IElement? MetaClass { get; set; }

    /// <summary>
    /// Sets the properties for Extent and ExtentTypes by the given extent.
    /// If extent is empty, the call will just be ignored
    /// </summary>
    /// <param name="extent">Extent to be set</param>
    public void SetByExtent(IUriExtent? extent)
    {
        if (extent == null)
            return;
        
        Extent = extent;
        ExtentTypes = extent.GetConfiguration().ExtentTypes;
    }
}