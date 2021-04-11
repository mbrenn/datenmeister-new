using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.Provider.ManagementProviders.Workspaces
{
    /// <summary>
    /// Contains all workspaces in an extent like structure
    /// </summary>
    public class ExtentOfWorkspaceProvider : IProvider
    {
        /// <summary>
        /// Gets the uri of the extent which contains the workspaces
        /// </summary>
        public const string WorkspaceTypeUri = WorkspaceNames.UriExtentInternalTypes;

        /// <summary>
        /// Initializes a new instance of the ExtentOfWorkspaces
        /// </summary>
        /// <param name="scope">The dependency inject</param>
        public ExtentOfWorkspaceProvider(IDatenMeisterScope scope)
        {
            WorkspaceLogic = scope.WorkspaceLogic;
            ExtentManager = scope.Resolve<ExtentManager>();
        }

        public ExtentManager ExtentManager { get; set; }

        /// <summary>
        /// Gets the workspace logic for the elements
        /// </summary>
        public IWorkspaceLogic WorkspaceLogic { get; }

        public IProviderObject CreateElement(string? metaClassUri)
        {
            throw new InvalidOperationException("The creation of new elements is not supported via MOF." +
                                                "Use WorkspaceLogic to create new extents");
        }
        public void AddElement(IProviderObject? valueAsObject, int index = -1)
        {
            throw new NotImplementedException();
        }

        public bool DeleteElement(string id) =>
            throw new NotImplementedException();

        public void DeleteAllElements()
        {
            throw new NotImplementedException();
        }

        public IProviderObject? Get(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return null;
            }

            var result = WorkspaceLogic.GetWorkspace(id);
            if (result == null)
            {
                return null;
            }

            return new WorkspaceObject(this, result);
        }

        public IEnumerable<IProviderObject> GetRootObjects()
        {
            var workspaces = WorkspaceLogic.Workspaces;
            return workspaces.Select(x => new WorkspaceObject(this, x));
        }

        /// <summary>
        /// Stores the capabilities of the provider
        /// </summary>
        /// <returns></returns>
        private readonly ProviderCapability _providerCapability = new ProviderCapability
        {
            IsTemporaryStorage = true,
            CanCreateElements = false
        };

        /// <summary>
        /// Gets the capabilities of the provider
        /// </summary>
        /// <returns></returns>
        public ProviderCapability GetCapabilities() => _providerCapability;
    }
}