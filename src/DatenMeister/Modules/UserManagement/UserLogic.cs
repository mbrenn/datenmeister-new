using Autofac;
using DatenMeister.Modules.TypeSupport;
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
        public void Initialize(ILifetimeScope scope)
        {
            LocalTypeSupport.AddLocalType(scope, typeof(User));
        }
    }
}