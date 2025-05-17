using DatenMeister.Core;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Extent.Manager.Extents.Configuration;
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
        private readonly IScopeStorage scopeStorage;

        public UmlPlugin(IScopeStorage scopeStorage)
        {
            this.scopeStorage = scopeStorage;
        }

        public Task Start(PluginLoadingPosition position)
        {
            var extentSettings = scopeStorage.Get<ExtentSettings>();
            extentSettings.extentTypeSettings.Add(
                new ExtentType(ExtentType)
                {
                    rootElementMetaClasses =
                    {
                        _UML.TheOne.Packages.__Package,
                        _UML.TheOne.StructuredClassifiers.__Class,
                        _UML.TheOne.SimpleClassifiers.__Enumeration
                    }
                });
            return Task.CompletedTask;
        }
    }
}