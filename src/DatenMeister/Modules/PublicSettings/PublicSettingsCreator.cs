using System;
using System.IO;
using System.Reflection;
using BurnSystems;

namespace DatenMeister.Modules.PublicSettings
{
    /// <summary>
    /// Creates a Public Settings file in the folder in which the
    /// file would be read upon next start
    /// </summary>
    public class PublicSettingsCreator
    {
        /// <summary>
        /// Adds an example file
        /// </summary>
        public void CreateExampleFile()
        {
            var readFile = ResourceHelper.LoadStringFromAssembly(
                typeof(PublicSettingsCreator),
                "DatenMeister.Modules.PublicSettings.publicSettings.example");

            var filePath = GetPublicSettingsPath();

            // Avoid overwriting
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, readFile);
            }
        }
        
        /// <summary>
        /// Gets the path to the public settings file
        /// </summary>
        /// <returns>The path to the file</returns>
        public static string GetPublicSettingsPath()
        {
            var path = Assembly.GetEntryAssembly()?.Location ??
                       throw new InvalidOperationException("No entry assembly");
            var filePath = Path.Combine(
                Path.GetDirectoryName(path) ??
                throw new InvalidOperationException("No directory"),
                PublicSettingHandler.XmiFileName);
            return filePath;
        }

        /// <summary>
        /// Checks whether the file is existing
        /// </summary>
        /// <returns></returns>
        public bool DoesFileExist()
        {
            return File.Exists(GetPublicSettingsPath());
        }
    }
}