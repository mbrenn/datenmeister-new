using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.AutoEnumerate;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Helper;
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
        MetaclassUriPath = ((MofObjectShadow) _DatenMeister.TheOne.Management.__Extent).Uri;
    }

    public ExtentObject(
        ExtentOfWorkspaceProvider provider,
        Workspace parentWorkspace,
        IUriExtent? uriExtent,
        ExtentStorageData.LoadedExtentInformation? loadedExtentInformation)
        : base(
            new Tuple<IUriExtent?, ExtentStorageData.LoadedExtentInformation?>(uriExtent, loadedExtentInformation),
            provider,
            string.Empty /*will be set below*/,
            MetaclassUriPath)
    {
        Id = ExtentManagementUrlHelper.GetIdOfExtent(parentWorkspace, uriExtent);

        LoadedExtentInformation = loadedExtentInformation;

        AddMapping(
            _DatenMeister._Management._Extent.workspaceId,
            e => parentWorkspace.id,
            (e, v) => throw new InvalidOperationException("Setting of workspaces is not supported"));

        AddMapping(
            _DatenMeister._Management._Extent.uri,
            e => uriExtent?.contextURI()
                 ?? loadedExtentInformation?.Configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs
                     ._ExtentLoaderConfig.extentUri) ?? "Invalid Uri",
            (e, v) =>
            {
                if (v == null) throw new InvalidOperationException("value is null");

                // First, update the extent itself
                if (uriExtent is MofUriExtent mofUriExtent) mofUriExtent.UriOfExtent = v.ToString() ?? "No string";

                // But we also need to update the extentmanager's configuration
                if (loadedExtentInformation != null)
                    loadedExtentInformation.Configuration.set(
                        _DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig.extentUri,
                        v.ToString());
            });

        AddMapping(
            _DatenMeister._Management._Extent.count,
            e => uriExtent?.elements().size() ?? 0,
            (e, v) => throw new InvalidOperationException("count cannot be set"));

        AddMapping(
            _DatenMeister._Management._Extent.totalCount,
            e => (uriExtent as MofUriExtent)?.ItemCount ?? 0,
            (e, v) => throw new InvalidOperationException("totalCount cannot be set"));

        AddMapping(
            _DatenMeister._Management._Extent.type,
            e => (uriExtent as MofUriExtent)?.Provider.GetType().Name,
            (e, v) => throw new InvalidOperationException("type cannot be set"));

        AddMapping(
            _DatenMeister._Management._Extent.name,
            e =>
            {
                var result = (uriExtent as MofExtent)?.GetConfiguration().Name;
                if (string.IsNullOrEmpty(result)) result = uriExtent?.contextURI();

                return result;
            },
            (e, v) =>
            {
                if (uriExtent is MofExtent mofExtent) mofExtent.GetConfiguration().Name = v?.ToString() ?? string.Empty;
            });

        AddMapping(
            _DatenMeister._Management._Extent.extentType,
            e => (uriExtent as MofExtent)?.GetConfiguration().ExtentType,
            (e, v) =>
            {
                if (uriExtent is MofExtent mofExtent)
                    mofExtent.GetConfiguration().ExtentType = v?.ToString() ?? string.Empty;
            });

        AddMapping(
            _DatenMeister._Management._Extent.autoEnumerateType,
            e => (uriExtent as MofExtent)?.GetConfiguration().AutoEnumerateType,
            (e, v) =>
            {
                if (uriExtent is MofExtent mofExtent && v != null)
                    if (Enum.TryParse<AutoEnumerateType>(v.ToString(), out var result))
                        mofExtent.GetConfiguration().AutoEnumerateType = result;
            });

        AddMapping(
            _DatenMeister._Management._Extent.isModified,
            e => (uriExtent as MofExtent)?.IsModified == true,
            (e, v) => throw new InvalidOperationException("isModified cannot be set"));

        AddMapping(
            _DatenMeister._Management._Extent.alternativeUris,
            e => (uriExtent as MofUriExtent)?.AlternativeUris,
            (e, v) => throw new InvalidOperationException("alternativeUris cannot be set"));

        AddMapping(
            _DatenMeister._Management._Extent.state,
            e => loadedExtentInformation?.LoadingState ?? ExtentLoadingState.Unknown,
            (e, v) => throw new InvalidOperationException("state cannot be set"));

        AddMapping(
            _DatenMeister._Management._Extent.failMessage,
            e => loadedExtentInformation?.FailLoadingMessage ?? string.Empty,
            (e, v) => throw new InvalidOperationException("state cannot be set"));

        AddMapping(
            _DatenMeister._Management._Extent.properties,
            e =>
            {
                var found = (loadedExtentInformation?.Extent as MofExtent)?.GetMetaObject()
                            ?? (uriExtent as MofExtent)?.GetMetaObject();
                return found == null
                    ? null
                    : new ProxyIdProviderObject(found.ProviderObject,
                        ExtentManagementUrlHelper.GetIdOfExtentsProperties(parentWorkspace, uriExtent));
            },
            (e, v) => throw new InvalidOperationException("properties cannot be set"));

        AddContainerMapping(
            x => new WorkspaceObject(provider, parentWorkspace),
            (_, value) => { }
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