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

/// <summary>
/// Implements the logic for the data views
/// </summary>
/// <param name="workspaceLogic">The workspace logic</param>
/// <param name="scopeStorage">The scope storage</param>
public class DataViewLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    /// <summary>
    /// Defines the path to the packages of the data views
    /// </summary>
    public const string PackagePathTypesDataView = "DatenMeister::DataViews";

    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(DataViewLogic));

    /// <summary>
    /// Gets the list of data view elements
    /// </summary>
    /// <returns>The collection of data view elements</returns>
    public IEnumerable<IElement> GetDataViewElements()
    {
        var metaClass = _DataViews.TheOne.__DataView;
        var cache = scopeStorage.Get<DataViewCache>();

        lock (cache)
        {
            if (cache.IsDirty)
            {
                Logger.Info("Cache is built up");
                cache.CachedDataViews.Clear();
                var managementWorkspace = workspaceLogic.GetManagementWorkspace();
                foreach (var dataView in managementWorkspace.extent.OfType<IUriExtent>()
                             .Where(extent => extent.contextURI() != WorkspaceNames.UriExtentWorkspaces)
                             .SelectMany(extent =>
                                 extent.elements().GetAllDescendantsIncludingThemselves().WhenMetaClassIs(metaClass)
                                     .Cast<IElement>()))
                {
                    cache.CachedDataViews.Add(dataView);
                }
            }

            cache.IsDirty = false;
            return cache.CachedDataViews.ToArray();
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