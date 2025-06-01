using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.ListViews;

/// <summary>
/// Defines a view definition which allows the user to create a new instance
/// of the given type by clicking upon the connected UI element. 
/// </summary>
public class NewInstanceViewExtension(IElement metaClass) : ViewExtension
{
    public IElement MetaClass { get; set; } = metaClass;
}