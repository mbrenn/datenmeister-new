using System.Diagnostics;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.TypeSupport;
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
                WorkspaceNames.NameManagement,
                ExtentUri,
                ExtentName,
                null,
                _integrationSettings.InitializeDefaultExtents ? ExtentCreationFlags.CreateOnly : ExtentCreationFlags.LoadOrCreate);

            if (!(extent.elements().WhenMetaClassIs(settingsMetaClass).FirstOrDefault() is IElement settings))
            {
                // Create new settings
                var factory = new MofFactory(extent);
                var element = factory.create(types[1]);
                element.set(nameof(UserManagementSettings.Salt), RandomFunctions.GetRandomAlphanumericString(16));
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
            GetsUserDatabase().elements().clear();
        }

        public (string password, string salt) HashPassword(string password)
        {
            var userDatabase = GetsUserDatabase();
            var salt = RandomFunctions.GetRandomAlphanumericString(16);
            var settingsMetaClass = _localTypeSupport.GetMetaClassFor(typeof(UserManagementSettings));
            var settings = userDatabase.elements().WhenMetaClassIs(settingsMetaClass).FirstOrDefault() as IElement;

            Debug.Assert(settings != null, "settings != null");
            var globalSalt = settings.get(nameof(UserManagementSettings.Salt)).ToString();

            var totalPassword = salt + password + globalSalt;

            return (totalPassword, salt);
        }

        private IUriExtent GetsUserDatabase()
        {
            var userDatabase = _workspaceLogic.GetWorkspace(WorkspaceNames.NameManagement).FindExtent(ExtentName);
            return userDatabase;
        }

        public void AddUser(string username, string password)
        {
            
        }

        public bool VerifyUser(string username, string password)
        {
            return true;
        }

        public void ChangePassword(string username, string password)
        {
        }
    }
}