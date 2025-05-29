namespace DatenMeister.Core;

public class IntegrationSettings
{
    public IntegrationSettings()
    {
        DatabasePath = DefaultDatabasePath;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the complete MOF and UML integration shall
    /// be performed or if a slim integration without having the complete meta model is sufficient.
    /// The slim integration does not load the Xmi files
    /// </summary>
    public bool PerformSlimIntegration { get; set; }

    /// <summary>
    /// Gets or sets the path to the xml files. It may be null, if the xmis shall be loaded
    /// from the embedded resources 
    /// </summary>
    public string? PathToXmiFiles { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the data environment including all the metamodels shall be established
    /// </summary>
    public bool EstablishDataEnvironment { get; set; } = true;

    /// <summary>
    /// True, if the default extents like users, user types, user views and other automatically generated
    /// extents shall be created as empty
    /// </summary>
    public bool InitializeDefaultExtents { get; set; }

    /// <summary>
    /// Gets or sets the name of the database path in which the databases are stored per default
    /// </summary>
    public string DatabasePath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the loading of an extent may fail without interrupting the complete initialization of the DatenMeister
    /// </summary>
    public bool AllowNoFailOfLoading { get; set; }

    /// <summary>
    /// Gets or sets the title of the mainwindow 
    /// </summary>
    public string? WindowTitle { get; set; }

    /// <summary>
    /// Gets or sets the flag indicating whether the locking is activated
    /// </summary>
    public bool IsLockingActivated { get; set; }

    /// <summary>
    /// Gets or sets the value that the DatenMeister is started in 'read-only' mode
    /// </summary>
    public bool IsReadOnly { get; set; }

    /// <summary>
    ///     Gets or sets the additional settings.
    ///     This parameter is included into the integration settings since the scope storage
    ///     is just created during the integration and cannot be filled by parameters which
    ///     are used by the integration before calling the integration
    /// </summary>
    public IScopeStorage AdditionalSettings { get; } = new ScopeStorage();

    /// <summary>
    /// Gets the default database path
    /// </summary>
    public static string DefaultDatabasePath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "datenmeister/data");

    /// <summary>
    /// Normalizes the directory path by using the integration settings.
    /// The normalization is done by using the following steps
    ///
    /// 1) First, the environmental settings are included
    /// 2) Second, the relative path is moved to absolute path
    /// </summary>
    /// <param name="directoryPath">Directory path to be normalized</param>
    /// <returns>The normalized directory path</returns>
    public string NormalizeDirectoryPath(string directoryPath)
    {
        directoryPath = Environment.ExpandEnvironmentVariables(directoryPath);

        if (!Path.IsPathRooted(directoryPath))
        {
            directoryPath = Path.Combine(DatabasePath, directoryPath);
        }

        return directoryPath;
    }
}