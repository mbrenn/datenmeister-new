namespace DatenMeister.Users.UserManagement;

/// <summary>
/// Defines the model for the user
/// </summary>
public class User
{
    /// <summary>
    /// Defines the name of the user
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Defines the encrypted password of the user
    /// </summary>
    public string? Password { get; set; }
}