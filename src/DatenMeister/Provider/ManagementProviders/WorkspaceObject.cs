using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.HelpingExtents
{
    public class WorkspaceObject : MappingProviderObject<Workspace>, IProviderObject
    {
        /// <summary>
        /// Initializes a new instance of the WorkspaceObject
        /// </summary>
        /// <param name="workspace">Workspace to be set</param>
        /// <param name="provider">The provider being set</param>
        public WorkspaceObject(IProvider provider, Workspace workspace) : base (workspace, provider, workspace.id)
        {
            AddMapping(
                "id",
                w => w.id,
                (w, v) => throw new InvalidOperationException("Id cannot be set"));

            AddMapping(
                "annotation",
                w => w.annotation,
                (w, v) => w.annotation = v.ToString());

            AddMapping(
                "extents",
                w => w.extent.Select(x => new ExtentObject(provider, (IUriExtent) x)),
                (w, v) => throw new InvalidOperationException("Extent cannot be set"));
        }

    }
}