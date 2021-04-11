using DatenMeister.Core;
using DatenMeister.ExtentManager.Extents.Configuration;
using DatenMeister.Plugins;

namespace DatenMeister.Types.Plugin
{
    // ReSharper disable once UnusedMember.Global
    public class UmlPlugin : IDatenMeisterPlugin
    {

        public const string PackageName = "Uml";

        /// <summary>
        /// Stores the name of the extent type
        /// </summary>
        public const string ExtentType = "Uml.Classes";

        public UmlPlugin(IScopeStorage scopeStorage)
        {
            var extentSettings = scopeStorage.Get<ExtentSettings>();
            extentSettings.extentTypeSettings.Add(
                new ExtentType(ExtentType));
        }

        public void Start(PluginLoadingPosition position)
        {
          
        }
    }
}