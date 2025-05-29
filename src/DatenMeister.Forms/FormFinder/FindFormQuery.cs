using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Forms.FormFinder;

public record FindFormQuery
{
    public _DatenMeister._Forms.___FormType FormType { get; set; }

    public IElement? metaClass { get; set; }

    public IEnumerable<string> extentTypes { get; set; } = new List<string>();

    public IObject? parentMetaClass { get; set; }

    public string parentProperty { get; set; } = string.Empty;

    public string workspaceId { get; set; } = string.Empty;

    public string extentUri { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the information whether the Form Finding shall be with active debugger break,
    /// in case the FormAssociation also requests a debugging. 
    /// </summary>
    public bool debugActive { get; set; }

    /// <summary>
    /// Gets or sets the id of the view mode being used for the query
    /// </summary>
    public string viewModeId { get; set; } = string.Empty;

    public FindFormQuery()
    {
#if DEBUG
        debugActive = true;
#else
            debugActive = false;
#endif
    }

    public override string ToString()
    {
        var result = $"{FormType}";
        if (metaClass != null)
        {
            result += $", metaClass: {NamedElementMethods.GetName(metaClass)}";
        }

        if (extentTypes.Any())
        {
            result += $", extentTypes: {string.Join(", ", extentTypes)}";
        }
            
        if (parentMetaClass != null)
        {
            result += $", parentMetaClass: {NamedElementMethods.GetName(parentMetaClass)}";
        }

        if (!string.IsNullOrEmpty(parentProperty))
        {
            result += $", parentProperty: {parentProperty}";
        }

        if (!string.IsNullOrEmpty(viewModeId))
        {
            result += $", viewModeId: {viewModeId}";
        }


        if (!string.IsNullOrEmpty(workspaceId))
        {
            result += $", workspaceId: {workspaceId}";
        }


        if (!string.IsNullOrEmpty(extentUri))
        {
            result += $", extentUri: {extentUri}";
        }

        return result;
    }
}