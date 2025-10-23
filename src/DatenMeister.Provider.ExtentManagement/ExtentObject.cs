using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.AutoEnumerate;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Mapping;
using DatenMeister.Core.Provider.Proxies;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Provider.ExtentManagement;

public class ExtentObject : MappingProviderObject<Tuple<IUriExtent?, ExtentStorageData.LoadedExtentInformation?>>
{
    static ExtentObject()
    {
        MetaclassUriPath = _Management.TheOne.__Extent.Uri;
    }

    public ExtentObject(
        ExtentOfWorkspaceProvider provider,
        IWorkspace parentWorkspace,
        IUriExtent? uriExtent,
        ExtentStorageData.LoadedExtentInformation? loadedExtentInformation)
        : base(
            new Tuple<IUriExtent?, ExtentStorageData.LoadedExtentInformation?>(uriExtent, loadedExtentInformation),
            provider,
            string.Empty /*will be set below*/,
            MetaclassUriPath)
    {
        Id = ExtentManagementHelper.GetIdOfExtent(parentWorkspace, uriExtent);

        LoadedExtentInformation = loadedExtentInformation;

        AddMapping(
            _Management._Extent.workspaceId,
            _ => parentWorkspace.id,
            (_, _) => throw new InvalidOperationException("Setting of workspaceId is not supported"));

        AddMapping(
            _Management._Extent.uri,
            _ => uriExtent?.contextURI()
                 ?? loadedExtentInformation?.Configuration.getOrDefault<string>(_ExtentLoaderConfigs
                     ._ExtentLoaderConfig.extentUri) ?? "Invalid Uri",
            (_, v) =>
            {
                if (v == null) throw new InvalidOperationException("value is null");

                // First, update the extent itself
                if (uriExtent is MofUriExtent mofUriExtent) mofUriExtent.UriOfExtent = v.ToString() ?? "No string";

                // But we also need to update the extentmanager's configuration
                if (loadedExtentInformation != null)
                    loadedExtentInformation.Configuration.set(
                        _ExtentLoaderConfigs._ExtentLoaderConfig.extentUri,
                        v.ToString());
            });

        AddMapping(
            _Management._Extent.count,
            _ => uriExtent?.elements().size() ?? 0,
            (_, _) => throw new InvalidOperationException("count cannot be set"));

        AddMapping(
            _Management._Extent.totalCount,
            _ => (uriExtent as MofUriExtent)?.ItemCount ?? 0,
            (_, _) => throw new InvalidOperationException("totalCount cannot be set"));

        AddMapping(
            _Management._Extent.type,
            _ => (uriExtent as MofUriExtent)?.Provider.GetType().Name,
            (_, _) => throw new InvalidOperationException("type cannot be set"));

        AddMapping(
            _Management._Extent.name,
            _ =>
            {
                var result = (uriExtent as MofExtent)?.GetConfiguration().Name;
                if (string.IsNullOrEmpty(result)) result = uriExtent?.contextURI();

                return result;
            },
            (_, v) =>
            {
                if (uriExtent is MofExtent mofExtent)
                    mofExtent.GetConfiguration().Name = v?.ToString() ?? string.Empty;
            });

        AddMapping(
            _Management._Extent.extentType,
            _ => (uriExtent as MofExtent)?.GetConfiguration().ExtentType,
            (_, v) =>
            {
                if (uriExtent is MofExtent mofExtent)
                    mofExtent.GetConfiguration().ExtentType = v?.ToString() ?? string.Empty;
            });

        AddMapping(
            _Management._Extent.autoEnumerateType,
            _ => (uriExtent as MofExtent)?.GetConfiguration().AutoEnumerateType,
            (_, v) =>
            {
                if (uriExtent is MofExtent mofExtent && v != null)
                    if (Enum.TryParse<AutoEnumerateType>(v.ToString(), out var result))
                        mofExtent.GetConfiguration().AutoEnumerateType = result;
            });

        AddMapping(
            _Management._Extent.isModified,
            _ => (uriExtent as MofExtent)?.IsModified == true,
            (_, _) => throw new InvalidOperationException("isModified cannot be set"));

        AddMapping(
            _Management._Extent.alternativeUris,
            _ => (uriExtent as MofUriExtent)?.AlternativeUris,
            (_, _) => throw new InvalidOperationException("alternativeUris cannot be set"));

        AddMapping(
            _Management._Extent.state,
            _ => loadedExtentInformation?.LoadingState ?? ExtentLoadingState.Unknown,
            (_, _) => throw new InvalidOperationException("state cannot be set"));

        AddMapping(
            _Management._Extent.failMessage,
            _ => loadedExtentInformation?.FailLoadingMessage ?? string.Empty,
            (_, _) => throw new InvalidOperationException("state cannot be set"));

        AddMapping(
            _Management._Extent.properties,
            _ =>
            {
                var found = (loadedExtentInformation?.Extent as MofExtent)?.GetMetaObject()
                            ?? (uriExtent as MofExtent)?.GetMetaObject();
                return found == null
                    ? null
                    : new ProxyIdProviderObject(
                        found.ProviderObject,
                        ExtentManagementHelper.GetIdOfExtentsProperties(parentWorkspace, uriExtent));
            },
            (_, _) => throw new InvalidOperationException("properties cannot be set"));

        /*AddMapping(
            _Management._Extent.loadingConfiguration,
            _ => loadedExtentInformation?.Configuration,
            (_, _) => throw new InvalidOperationException("loadingConfiguration cannot be set"));*/

        AddContainerMapping(
            _ => new WorkspaceObject(provider, parentWorkspace),
            (_, _) => { }
        );
    }

    /// <summary>
    ///     /Gets the information
    /// </summary>
    public ExtentStorageData.LoadedExtentInformation? LoadedExtentInformation { get; }

    /// <summary>
    ///     Stores the uri to the metaclass
    /// </summary>
    public static string MetaclassUriPath { get; }
}