using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.TreeView;

/// <summary>
/// Stores the treeview item parameter
/// </summary>
public class TreeViewItemParameter
{
    /// <summary>
    /// Initializes a new instnace of the TreeViewItemParameter class
    /// </summary>
    /// <param name="element">Element being shown</param>
    /// <param name="parentElement">The parent element storing the element</param>
    /// <param name="parentProperty">The property by which the element is access</param>
    public TreeViewItemParameter(IObject? element, IObject? parentElement = null, string? parentProperty = null)
    {
        Element = element;
        ParentElement = parentElement;
        ParentProperty = parentProperty;
    }

    /// <summary>
    /// Gets or sets the element
    /// </summary>
    public IObject? Element { get; set; }
        
    /// <summary>
    /// Gets or sets the parent element of the element.
    /// This element is used to get access to the Element via the reflective collection
    /// </summary>
    public IObject? ParentElement { get; set; }
        
    /// <summary>
    /// Gets the parant property to access the element via the paraent element
    /// </summary>
    public string? ParentProperty { get; set; }
}