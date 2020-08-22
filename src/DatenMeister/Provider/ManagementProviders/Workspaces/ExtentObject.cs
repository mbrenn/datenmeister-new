using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Models.ManagementProviders;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using Workspace = DatenMeister.Runtime.Workspaces.Workspace;

namespace DatenMeister.Provider.ManagementProviders.Workspaces
{
    public class ExtentObject : MappingProviderObject<ExtentStorageData.LoadedExtentInformation>
    {
        static ExtentObject()
        {
            MetaclassUriPath = ((MofObjectShadow) _ManagementProvider.TheOne.__Extent).Uri;
        }

        public ExtentObject(
            ExtentOfWorkspaceProvider provider,
            Workspace parentWorkspace,
            IUriExtent? uriExtent,
            ExtentStorageData.LoadedExtentInformation loadedExtentInformation) : base(loadedExtentInformation, provider, loadedExtentInformation.Configuration.extentUri, MetaclassUriPath)
        {
            AddMapping(
                _ManagementProvider._Extent.uri,
                e => loadedExtentInformation.Configuration.extentUri,
                (e, v) =>
                {
                    if (v == null)
                    {
                        throw new InvalidOperationException("value is null");
                    }

                    // First, update the extent itself
                    if (uriExtent is MofUriExtent mofUriExtent)
                    {
                        (mofUriExtent).UriOfExtent = v.ToString();
                    }

                    // But we also need to update the extentmanager's configuration
                    loadedExtentInformation.Configuration.extentUri = v.ToString();
                });

            AddMapping(
                _ManagementProvider._Extent.count,
                e => uriExtent?.elements().size() ?? 0,
                (e, v) => throw new InvalidOperationException("count cannot be set"));

            AddMapping(
                _ManagementProvider._Extent.totalCount,
                e => (uriExtent as MofUriExtent)?.ItemCount ?? 0,
                (e, v) => throw new InvalidOperationException("totalCount cannot be set"));

            AddMapping(
                _ManagementProvider._Extent.type,
                e => (uriExtent as MofUriExtent)?.Provider.GetType().Name,
                (e, v) => throw new InvalidOperationException("type cannot be set"));

            AddMapping(
                _ManagementProvider._Extent.extentType,
                e => (uriExtent as MofExtent)?.GetConfiguration().ExtentType,
                (e, v) =>
                {
                    if (uriExtent is MofExtent mofExtent)
                    {
                        mofExtent.GetConfiguration().ExtentType = v?.ToString() ?? string.Empty;
                    }
                });

            AddMapping(
                _ManagementProvider._Extent.isModified,
                e => (uriExtent as MofExtent)?.IsModified == true,
                (e, v) => throw new InvalidOperationException("isModified cannot be set"));

            AddMapping(
                _ManagementProvider._Extent.alternativeUris,
                e => (uriExtent as MofUriExtent)?.AlternativeUris,
                (e, v) => throw new InvalidOperationException("alternativeUris cannot be set"));

            AddMapping(
                _ManagementProvider._Extent.state,
                e => loadedExtentInformation.LoadingState,
                (e, v) => throw new InvalidOperationException("state cannot be set"));


            AddMapping(
                _ManagementProvider._Extent.failMessage,
                e => loadedExtentInformation.FailLoadingMessage,
                (e, v) => throw new InvalidOperationException("state cannot be set"));

            AddContainerMapping(
                (x) => new WorkspaceObject(provider, parentWorkspace),
                (_, value) => { }
            );
        }

        /// <summary>
        /// Stores the uri to the metaclass
        /// </summary>
        public static string MetaclassUriPath { get; }
    }
}