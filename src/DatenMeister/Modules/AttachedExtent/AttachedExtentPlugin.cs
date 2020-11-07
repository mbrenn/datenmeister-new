using DatenMeister.Runtime.Plugins;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Runtime.Extents.Configuration;


namespace DatenMeister.Modules.AttachedExtent
{
    [PluginLoading(PluginLoadingPosition.AfterInitialization | PluginLoadingPosition.AfterLoadingOfExtents)]
    public class AttachedExtentPlugin : IDatenMeisterPlugin
    {
        private readonly ExtentSettings _extentSettings;

        public AttachedExtentPlugin(IScopeStorage scopeStorage)
        {
            _extentSettings = scopeStorage.Get<ExtentSettings>();
        }

        public void Start(PluginLoadingPosition position)
        {
            if ((position & PluginLoadingPosition.AfterInitialization) != 0)
            {
                _extentSettings.propertyDefinitions.Add(
                    new ExtentPropertyDefinition
                    {
                        name = AttachedExtentHandler.AttachedExtentProperty,
                        title = "Attached Extent",
                        metaClass = _DatenMeister.TheOne.AttachedExtent.__AttachedExtentConfiguration
                    });
            }
            else if ((position & PluginLoadingPosition.AfterLoadingOfExtents) != 0)
            {
            }
        }
    }
}