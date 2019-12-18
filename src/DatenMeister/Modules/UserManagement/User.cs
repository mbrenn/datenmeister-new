namespace DatenMeister.Modules.UserManagement
{
    /// <summary>
    /// Defines the model for the user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Defines the name of the user
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the encrypted password of the user
        /// </summary>
        public string password
        {
            get;
            set;
        }
    }
}