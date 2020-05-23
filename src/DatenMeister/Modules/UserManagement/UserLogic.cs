using System;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Functions.Algorithm;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.UserManagement
{
    /// <summary>
    /// Implements the logic for the user
    /// </summary>
    public class UserLogic
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(UserLogic));

        /// <summary>
        /// Defines the default extent for the users
        /// </summary>
        public const string ExtentUri = "datenmeister:///users";

        private const string ExtentName = "DatenMeister.Users";

        private readonly LocalTypeSupport _localTypeSupport;

        private readonly ExtentCreator _extentCreator;

        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IntegrationSettings _integrationSettings;

        public UserLogic(LocalTypeSupport localTypeSupport, ExtentCreator extentCreator, IWorkspaceLogic workspaceLogic, IntegrationSettings integrationSettings)
        {
            _localTypeSupport = localTypeSupport;
            _extentCreator = extentCreator;
            _workspaceLogic = workspaceLogic;
            _integrationSettings = integrationSettings;
        }

        /// <summary>
        /// Initializes the user logic and creates and loads the default extent
        /// </summary>
        public void Initialize()
        {
            var types = _localTypeSupport.AddInternalTypes(
                new[] {typeof(User), typeof(UserManagementSettings)},
                "DatenMeister::UserManagement"
            );

            var settingsMetaClass = _localTypeSupport.GetMetaClassFor(typeof(UserManagementSettings));

            var extent = _extentCreator.GetOrCreateXmiExtentInInternalDatabase(
                WorkspaceNames.WorkspaceManagement,
                ExtentUri,
                ExtentName,
                "",
                _integrationSettings.InitializeDefaultExtents ? ExtentCreationFlags.CreateOnly : ExtentCreationFlags.LoadOrCreate);
            if (extent == null)
            {
                throw new InvalidOperationException("Extent could not be created");
            }

            if (!(extent.elements().WhenMetaClassIs(settingsMetaClass).FirstOrDefault() is IElement))
            {
                // Create new settings
                var factory = new MofFactory(extent);
                var element = factory.create(types[1]);
                element.set(nameof(UserManagementSettings.salt), RandomFunctions.GetRandomAlphanumericString(16));
                extent.elements().add(element);
                Logger.Info("UserLogic - Created Salt...");
            }
            else
            {
                Logger.Debug("UserLogic - Salt is existing");
            }
        }

        /// <summary>
        /// Clears the database and all user
        /// </summary>
        public void ClearDatabase()
        {
            GetUserDatabase().elements().clear();
        }

        public (string password, string salt) HashPassword(string password)
        {
            var userDatabase = GetUserDatabase();
            var salt = RandomFunctions.GetRandomAlphanumericString(16);
            var settingsMetaClass = _localTypeSupport.GetMetaClassFor(typeof(UserManagementSettings));

            if (!(userDatabase.elements().WhenMetaClassIs(settingsMetaClass).FirstOrDefault() is IElement settings))
            {
                throw new InvalidOperationException("UserManagementSettings is not found");
            }
            
            var globalSalt = settings.getOrDefault<string>(nameof(UserManagementSettings.salt));

            var totalPassword = salt + password + globalSalt;

            return (totalPassword, salt);
        }

        private IUriExtent GetUserDatabase()
        {
            var userDatabase = _workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceManagement)?.FindExtent(ExtentName);
            if (userDatabase == null)
            {
                throw new InvalidOperationException("User Database was not found");
            }
            
            return userDatabase;
        }

        public void AddUser(string username, string password)
        {
        }

        public bool VerifyUser(string username, string password) =>
            true;

        public void ChangePassword(string username, string password)
        {
        }
    }
}