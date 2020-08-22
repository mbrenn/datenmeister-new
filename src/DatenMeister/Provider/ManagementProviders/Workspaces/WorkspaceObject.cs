using System;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Models.ManagementProviders;
using Workspace = DatenMeister.Runtime.Workspaces.Workspace;

namespace DatenMeister.Provider.ManagementProviders.Workspaces
{
    public class WorkspaceObject : MappingProviderObject<Workspace>
    {
        static WorkspaceObject()
        {
            MetaclassUriPath = ((MofObjectShadow) _ManagementProvider.TheOne.__Workspace).Uri;
        }

        /// <summary>
        /// Initializes a new instance of the WorkspaceObject
        /// </summary>
        /// <param name="workspace">Workspace to be set</param>
        /// <param name="provider">The provider being set</param>
        public WorkspaceObject(ExtentOfWorkspaceProvider provider, Workspace workspace) : base(workspace, provider,
            workspace.id, MetaclassUriPath)
        {
            AddMapping(
                _ManagementProvider._Workspace.id,
                w => w.id,
                (w, v) => throw new InvalidOperationException("Id cannot be set"));

            AddMapping(
                _ManagementProvider._Workspace.annotation,
                w => w.annotation,
                (w, v) => w.annotation = v?.ToString() ?? string.Empty);

            AddMapping(
                _ManagementProvider._Workspace.extents,
                w => w.extent.Select(x =>
                {
                    var loadedExtentInformation = provider.ExtentManager.GetLoadedExtentInformation((IUriExtent) x);
                    if (loadedExtentInformation != null)
                    {
                        return new ExtentObject(provider, workspace, (IUriExtent) x, loadedExtentInformation);
                    }

                    return null;
                }).Where(x => x != null),
                (w, v) => throw new InvalidOperationException("Extent cannot be set"));
        }

        /// <summary>
        /// Stores the uri to the metaclass
        /// </summary>
        public static string MetaclassUriPath { get; }
    }
}