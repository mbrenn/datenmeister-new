#nullable enable

using System;
using System.Threading.Tasks;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;

namespace DatenMeister.Forms
{
    /// <summary>
    /// Defines the access to the view logic and abstracts the access to the view extent
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FormsPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(FormsPlugin));

        private readonly ExtentCreator _extentCreator;
        private readonly ExtentSettings _extentSettings;
        private readonly IntegrationSettings _integrationSettings;
        private readonly IScopeStorage _scopeStorage;

        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Initializes a new instance of the FormLogic class
        /// </summary>
        /// <param name="workspaceLogic">The workspace being used</param>
        /// <param name="extentCreator">The support class to create extents</param>
        /// <param name="scopeStorage">The settings that had been used for integration</param>
        public FormsPlugin(IWorkspaceLogic workspaceLogic,
            ExtentCreator extentCreator,
            IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _extentCreator = extentCreator;
            _scopeStorage = scopeStorage;
            _integrationSettings = scopeStorage.Get<IntegrationSettings>();
            _extentSettings = scopeStorage.Get<ExtentSettings>();
        }

        /// <summary>
        /// Initializes a new instance of the FormLogic class
        /// </summary>
        /// <param name="workspaceLogic">The workspace being used</param>
        /// <param name="scopeStorage">The settings that had been used for integration</param>
        public FormsPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
            : this(workspaceLogic, new ExtentCreator(workspaceLogic, scopeStorage), scopeStorage)
        {
        }

        /// <summary>
        /// Gets the workspace logic of the view logic
        /// </summary>
        public IWorkspaceLogic WorkspaceLogic => _workspaceLogic;

        /// <summary>
        /// Integrates the the view logic into the workspace.
        /// </summary>
        public async Task Start(PluginLoadingPosition position)
        {
            var mgmtWorkspace = _workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceManagement)
                                ?? throw new InvalidOperationException("Management Workspace is not found");

            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    // Creates the internal views for the DatenMeister
                    var dotNetUriExtent =
                        new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentInternalForm, _scopeStorage);
                    dotNetUriExtent.GetConfiguration().ExtentType = FormMethods.FormExtentType;
                    _workspaceLogic.AddExtent(mgmtWorkspace, dotNetUriExtent);
                    _extentSettings.extentTypeSettings.Add(
                        new ExtentType(FormMethods.FormExtentType));
                    break;

                case PluginLoadingPosition.AfterLoadingOfExtents:
                    var extent = await _extentCreator.GetOrCreateXmiExtentInInternalDatabase(
                        WorkspaceNames.WorkspaceManagement,
                        WorkspaceNames.UriExtentUserForm,
                        "DatenMeister.Forms_User",
                        FormMethods.FormExtentType,
                        _integrationSettings.InitializeDefaultExtents
                            ? ExtentCreationFlags.CreateOnly
                            : ExtentCreationFlags.LoadOrCreate
                    );

                    if (extent == null)
                        throw new InvalidOperationException("Extent for users is not found");

                    extent.GetConfiguration()
                        .AddDefaultTypes(new[]
                            {_DatenMeister.TheOne.Forms.__Form,
                                _DatenMeister.TheOne.Forms.__CollectionForm,
                                _DatenMeister.TheOne.Forms.__ObjectForm,
                                _DatenMeister.TheOne.Forms.__TableForm,
                                _DatenMeister.TheOne.Forms.__RowForm,
                                _DatenMeister.TheOne.Forms.__FormAssociation});

                    // Tests the existence of the test form and other forms
                    var testForm = _workspaceLogic.FindElement(WorkspaceNames.WorkspaceManagement, Uris.TestFormUri);
                    if (testForm == null)
                    {
                        Logger.Error("The Testform with uri " + Uris.TestFormUri + " is not found");
                    }

                    break;
            }
        }
    }
}