using DatenMeister.Core.Plugins;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Modules.TypeSupport;

namespace DatenMeister.Modules.FastViewFilter
{
    /// <summary>
    /// Implements the plugin adding the fast views
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class FastViewFilterPlugin : IDatenMeisterPlugin
    {

        /// <summary>
        /// Stores the local type support
        /// </summary>
        private readonly LocalTypeSupport _localTypeSupport;

        /// <summary>
        /// Initializes a new instance of the FastViewFilterPlugin
        /// </summary>
        /// <param name="localTypeSupport">Local Type support to be added</param>
        public FastViewFilterPlugin(LocalTypeSupport localTypeSupport)
        {
            _localTypeSupport = localTypeSupport;
        }

        public void Start(PluginLoadingPosition position)
        {
            _localTypeSupport.AddInternalTypes(FastViewFilters.Types, FastViewFilterLogic.PackagePathTypesFastViewFilters);
        }
    }
}