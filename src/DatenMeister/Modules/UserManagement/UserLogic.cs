using System.Diagnostics;
using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Provider.XMI.ExtentStorage;
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
        /// <summary>
        /// Defines the default extent for the users
        /// </summary>
        public const string ExtentUri = "datenmeister:///users";

        /// <summary>
        /// Initializes the user logic and creates and loads the default extent
        /// </summary>
        /// <param name="scope"></param>
        public static void Initialize(ILifetimeScope scope)
        {
            var types = LocalTypeSupport.AddLocalType(
                scope,
                new[] {typeof(User), typeof(UserManagementSettings)}
            );

            var settingsMetaClass = LocalTypeSupport.GetMetaClassFor(scope, typeof(UserManagementSettings));

            var extent = ExtentCreator.GetOrCreateXmiExtentInInternalDatabase(
                scope,
                WorkspaceNames.NameManagement,
                ExtentUri,
                "DatenMeister.Users");

            var settings = extent.elements().WhenMetaClassIs(settingsMetaClass).FirstOrDefault() as IElement;
            if (settings == null)
            {
                // Create new settings
                var factory = new MofFactory(extent);
                var element = factory.create(types[1]);
                element.set(nameof(UserManagementSettings.Salt), RandomFunctions.GetRandomAlphanumericString(16));
                extent.elements().add(element);
                Debug.WriteLine("UserLogic - Created Salt...");
            }
            else
            {
                Debug.WriteLine("UserLogic - Salt is existing");
            }
        }
    }
}