namespace DatenMeister.Core.Runtime.Workspaces;

public static class WorkspaceNames
{
    public const string WorkspaceManagement = "Management";
    public const string WorkspaceData = "Data";
    public const string WorkspaceTypes = "Types";
    public const string WorkspaceUml = "UML";
    public const string WorkspaceMof = "MOF";
    public const string WorkspaceViews = "Views";

    public const string UriExtentUml = "dm:///_internal/model/uml";
    public const string UriExtentMof = "dm:///_internal/model/mof";
    public const string UriExtentPrimitiveTypes = "dm:///_internal/model/primitivetypes";


    public const string UriExtentInternalTypes = "dm:///_internal/types/internal";
    public const string UriExtentUserTypes = "dm:///_internal/types/user";


    /// <summary>
    /// Defines the uri of the view to the view extents
    /// </summary>
    public const string UriExtentInternalForm = "dm:///_internal/forms/internal";

    /// <summary>
    /// Defines the uri of the user views
    /// </summary>
    public const string UriExtentUserForm = "dm:///_internal/forms/user";

    /// <summary>
    /// Gets the uri of the extent which contains the workspaces
    /// </summary>
    public const string UriExtentWorkspaces = "dm:///_internal/workspaces";

    /// <summary> 
    /// Gets the uri of the extent which contains the workspaces
    /// </summary>
    public const string UriExtentSettings = "dm:///_internal/settings";


    public const string StandardUmlNamespace = "http://www.omg.org/spec/UML/20131001";
    public const string StandardUmlNamespaceAlternative = "http://www.omg.org/spec/UML/20131001/UML.xmi";


    public const string StandardPrimitiveTypeNamespace = "http://www.omg.org/spec/PrimitiveTypes/20131001";

    public const string StandardPrimitiveTypeNamespaceAlternative =
        "http://www.omg.org/spec/UML/20131001/PrimitiveTypes.xmi";
}