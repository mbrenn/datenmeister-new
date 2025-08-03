using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Forms.FormFinder;

public record FindFormQuery
{
    public _Forms.___FormType FormType { get; init; }

    public IElement? MetaClass { get; init; }

    public IEnumerable<string> ExtentTypes { get; init; } = new List<string>();

    public IObject? ParentMetaClass { get; init; }

    public string ParentProperty { get; init; } = string.Empty;

    public string WorkspaceId { get; init; } = string.Empty;

    public string ExtentUri { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the information whether the Form Finding shall be with active debugger break,
    /// in case the FormAssociation also requests a debugging. 
    /// </summary>
    public bool DebugActive { get; init; }

    /// <summary>
    /// Gets or sets the id of the view mode being used for the query
    /// </summary>
    public string ViewModeId { get; init; } = string.Empty;

    public FindFormQuery()
    {
#if DEBUG
        DebugActive = true;
#else
        DebugActive = false;
#endif
    }

    public override string ToString()
    {
        var result = $"{FormType}";
        if (MetaClass != null)
        {
            result += $", metaClass: {NamedElementMethods.GetName(MetaClass)}";
        }

        if (ExtentTypes.Any())
        {
            result += $", extentTypes: {string.Join(", ", ExtentTypes)}";
        }
            
        if (ParentMetaClass != null)
        {
            result += $", parentMetaClass: {NamedElementMethods.GetName(ParentMetaClass)}";
        }

        if (!string.IsNullOrEmpty(ParentProperty))
        {
            result += $", parentProperty: {ParentProperty}";
        }

        if (!string.IsNullOrEmpty(ViewModeId))
        {
            result += $", viewModeId: {ViewModeId}";
        }


        if (!string.IsNullOrEmpty(WorkspaceId))
        {
            result += $", workspaceId: {WorkspaceId}";
        }

        if (!string.IsNullOrEmpty(ExtentUri))
        {
            result += $", extentUri: {ExtentUri}";
        }

        return result;
    }
}