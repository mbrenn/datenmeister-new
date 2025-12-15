using BurnSystems.Logging;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView;

public class DataViewLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    /// <summary>
    /// Defines the path to the packages of the dataviews
    /// </summary>
    public const string PackagePathTypesDataView = "DatenMeister::DataViews";

    /// <summary>
    ///     Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(DataViewLogic));

    public IEnumerable<IElement> GetDataViewElements()
    {
        var metaClass = _DataViews.TheOne.__DataView;

        var managementWorkspace = workspaceLogic.GetManagementWorkspace();
        foreach (var dataView in managementWorkspace.extent.OfType<IUriExtent>()
                     .Where(extent => extent.contextURI() != WorkspaceNames.UriExtentWorkspaces)
                     .SelectMany(extent =>
                         extent.elements().GetAllDescendantsIncludingThemselves().WhenMetaClassIs(metaClass).Cast<IElement>()))
        {
            yield return dataView;
        }
    }

    /// <summary>
    /// Parses the given view node and return the values of the viewnode as a reflective sequence
    /// </summary>
    /// <param name="viewNode">View Node to be parsed</param>
    /// <returns>The reflective Sequence</returns>
    public IReflectiveCollection GetElementsForViewNode(IElement viewNode)
    {
        var evaluation = new DataViewEvaluation(workspaceLogic, scopeStorage);
        return evaluation.GetElementsForViewNode(viewNode);
    }
}