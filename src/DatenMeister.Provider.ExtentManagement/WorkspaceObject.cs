using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Mapping;
using DatenMeister.Core.Runtime.Workspaces;
using static DatenMeister.Core.Models._ExtentLoaderConfigs;

namespace DatenMeister.Provider.ExtentManagement;

public class WorkspaceObject : MappingProviderObject<Workspace>
{
    static WorkspaceObject()
    {
        MetaclassUriPath = _Management.TheOne.__Workspace.Uri;
    }

    /// <summary>
    ///     Initializes a new instance of the WorkspaceObject
    /// </summary>
    /// <param name="workspace">Workspace to be set</param>
    /// <param name="provider">The provider being set</param>
    public WorkspaceObject(ExtentOfWorkspaceProvider provider, Workspace workspace) : base(workspace, provider,
        workspace.id, MetaclassUriPath)
    {
        Id = ExtentManagementHelper.GetIdOfWorkspace(workspace);

        AddMapping(
            _Management._Workspace.id,
            w => w.id,
            (_, _) => throw new InvalidOperationException("Id cannot be set"));

        AddMapping(
            _Management._Workspace.annotation,
            w => w.annotation,
            (w, v) => w.annotation = v?.ToString() ?? string.Empty);

        AddMapping(
            _Management._Workspace.extents,
            w => 
            {
                var result = new List<ExtentObject?>();

                // Gets all the extents which are actively loaded
                result.AddRange(
                    w.extent.Select(x =>
                    {
                        if (x is not IUriExtent asUriExtent) return null;

                        var loadedExtentInformation =
                            provider.ExtentManager.GetLoadedExtentInformation(asUriExtent);
                        if (loadedExtentInformation != null)
                            return new ExtentObject(provider, workspace, asUriExtent, loadedExtentInformation);

                        return new ExtentObject(provider, workspace, asUriExtent, null);
                    }).Where(x => x != null));

                var copyResult = result.ToList();

                // Gets all the extent which are registered but are not actively loaded.
                // These one might be in error, unloaded state or other states
                foreach (var loadedExtent in
                         provider.ExtentManager.GetLoadedExtentInformationForWorkspace(w.id))
                {
                    if (copyResult.Any(x =>
                            x?.LoadedExtentInformation?.Configuration.getOrDefault<string>(_ExtentLoaderConfig
                                .extentUri) ==
                            loadedExtent.Configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri)))
                        continue;

                    result.Add(new ExtentObject(
                        provider, workspace, null, loadedExtent));
                }

                var resultAsCollection = new TemporaryReflectiveCollection(result);
                resultAsCollection.OnDelete += (x, y) =>
                {
                    var extentObject = y.DeleteObject as ExtentObject;
                    if (extentObject?.LoadedExtentInformation != null)
                    {
                        provider.ExtentManager.RemoveExtent(extentObject.LoadedExtentInformation).GetAwaiter()
                            .GetResult();
                    }
                };

                return resultAsCollection;
            },
            (_, _) => throw new InvalidOperationException("Extent cannot be set"));
    }

    /// <summary>
    ///     Stores the uri to the metaclass
    /// </summary>
    public static string MetaclassUriPath { get; }
}