namespace DatenMeister.Plugins
{
    /// <summary>
    ///     Defines the settings for the default plugin loader
    /// </summary>
    public class DefaultPluginSettings
    {
        /// <summary>
        ///     Stores the name of assemblies to be skipped
        /// </summary>
        public List<string> AssemblyFilesToBeSkipped { get; set; } = new();
    }
}