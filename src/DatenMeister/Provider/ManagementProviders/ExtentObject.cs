using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime;
using Workspace = DatenMeister.Runtime.Workspaces.Workspace;

namespace DatenMeister.Provider.ManagementProviders
{
    public class ExtentObject : MappingProviderObject<IUriExtent>
    {
        static ExtentObject()
        {
            MetaclassUriPath = ((MofObjectShadow) _ManagementProvider.TheOne.__Extent).Uri;
        }

        public ExtentObject(IProvider provider,
            Workspace parentWorkspace,
            IUriExtent uriExtent) : base(uriExtent, provider, uriExtent.contextURI(), MetaclassUriPath)
        {
            AddMapping(
                "uri",
                e => e.contextURI(),
                (e, v) => throw new InvalidOperationException("uri cannot be set"));

            AddMapping(
                "count",
                e => e.elements().size(),
                (e, v) => throw new InvalidOperationException("count cannot be set"));

            AddMapping(
                "type",
                e => (e as MofExtent)?.Provider.GetType().Name,
                (e, v) => throw new InvalidOperationException("type cannot be set"));

            AddMapping(
                "extentType",
                e => (e as MofExtent)?.GetExtentType(),
                (e, v) => (e as MofExtent)?.SetExtentType(v?.ToString() ?? string.Empty));

            AddMapping(
                "isModified",
                e => (e as MofExtent)?.IsModified == true,
                (e, v) => throw new InvalidOperationException("isModified cannot be set"));

            AddMapping(
                "alternativeUris",
                e => (e as MofUriExtent)?.AlternativeUris,
                (e, v) => throw new InvalidOperationException("alternativeUris cannot be set"));

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