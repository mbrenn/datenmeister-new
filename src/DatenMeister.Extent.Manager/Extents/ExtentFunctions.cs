using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Extent.Manager.Extents;

/// <summary>
/// Helper function for extents
/// </summary>
public class ExtentFunctions
{
    /// <summary>
    /// Stores the workspace logic
    /// </summary>
    private readonly IWorkspaceLogic _workspaceLogic;

    /// <summary>
    /// Initializes a new instance of the ExtentFunctions class.
    /// </summary>
    /// <param name="workspaceLogic"></param>
    public ExtentFunctions(IWorkspaceLogic workspaceLogic)
    {
        _workspaceLogic = workspaceLogic;
    }

    /// <summary>
    /// Gets an enumeration of creatable types for a given extent.
    /// It navigates to the meta extent and looks for all classes
    /// </summary>
    /// <param name="collection">The reflectivecollection into which a new instance shall be created</param>
    /// <returns>Enumeration of types</returns>
    public CreatableTypeResult GetCreatableTypes(IReflectiveCollection collection) =>
        GetCreatableTypes(((IHasExtent) collection).Extent ?? throw new InvalidOperationException("No extent"));

    /// <summary>
    /// Gets an enumeration of creatable types for a given extent.
    /// It navigates to the meta extent and looks for all classes
    /// </summary>
    /// <param name="element">The reflectivecollection into which a new instance shall be created</param>
    /// <returns>Enumeration of types</returns>
    public CreatableTypeResult GetCreatableTypes(IElement element) =>
        GetCreatableTypes(((IHasExtent) element).Extent ?? throw new InvalidOperationException("No extent"));

    /// <summary>
    /// Gets an enumeration of creatable types for a given extent.
    /// It navigates to the meta extent and looks for all classes
    /// </summary>
    /// <param name="extent">The extent into which a new instance shall be created</param>
    /// <returns>Enumeration of types</returns>
    public CreatableTypeResult GetCreatableTypes(IExtent extent)
    {
        var dataLayer = _workspaceLogic.GetWorkspaceOfExtent(extent);
        if (dataLayer == null)
        {
            throw new InvalidOperationException("Datalayer is not found");
        }
            
        var typeLayer = dataLayer.MetaWorkspaces.FirstOrDefault() 
                        ?? _workspaceLogic.GetTypesWorkspace();

        var classType = _UML.TheOne.StructuredClassifiers.__Class;

        if (classType == null)
        {
            // We did not find the corresponding class type
            return new CreatableTypeResult
            {
                MetaLayer = typeLayer,
                CreatableTypes = []
            };
        }

        return new CreatableTypeResult
        {
            MetaLayer = typeLayer,
            CreatableTypes = _workspaceLogic.GetExtentsForWorkspace(typeLayer)
                .SelectMany(x => x.elements().GetAllDescendants().WhenMetaClassIs(classType))
                .Cast<IElement>()
                .ToList()
        };
    }

    public class CreatableTypeResult
    {
        public Workspace? MetaLayer { get; set; }
        public IList<IElement>? CreatableTypes { get; set; }
    }
}