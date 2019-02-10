namespace DatenMeister.Modules.UserManagement
{
    /// <summary>
    /// Gets or sets the settings of the usermanagement
    /// </summary>
    public class UserManagementSettings
    {
        /// <summary>
        /// Gets or sets the salt to store the hashes for the password of the user
        /// </summary>
        public string Salt { get; set; }
    }
}