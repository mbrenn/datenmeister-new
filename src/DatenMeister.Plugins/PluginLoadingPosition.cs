using System;

namespace DatenMeister.Plugins
{
    /// <summary>
    ///     Defines the loadingPosition point of the plugin. It may be at start after having defined all objects and
    ///     resolvement or it may be at the end of DatenMeisterPreparation
    /// </summary>
    [Flags]
    public enum PluginLoadingPosition
    {
        /// <summary>
        ///     Before the MOF and XMI are boot strapped. This position is used to add new extent types
        /// </summary>
        BeforeBootstrapping = 1,

        /// <summary>
        ///     After MOF and XMI are boot strapped. This position is used to define new objects
        /// </summary>
        AfterBootstrapping = 2,

        /// <summary>
        ///     Called, after the initialization of all the internal information but before the extents
        ///     are explicitly loaded
        /// </summary>
        AfterInitialization = 4,

        /// <summary>
        ///     After initialization of the AfterBootstrapping has occured. After that event, some internal objects are created
        ///     upon some DatenMeister internal states
        /// </summary>
        AfterLoadingOfExtents = 8,
        
        /// <summary>
        /// When there is a shutdown request, the plugins can stop there work and background tasks
        /// </summary>
        AfterShutdownStarted = 16
    }
}