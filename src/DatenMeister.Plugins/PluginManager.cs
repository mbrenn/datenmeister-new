using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Autofac;
using BurnSystems.Logging;

namespace DatenMeister.Plugins
{
    public class PluginManager
    {
        private static readonly ClassLogger Logger = new(typeof(PluginManager));

        /// <summary>
        ///     Stores the dictionary of instantiated plugins
        /// </summary>
        private readonly Dictionary<Type, IDatenMeisterPlugin> _instantiatedPlugins = new();

        private List<Type>? _pluginTypes;

        /// <summary>
        ///     Gets or sets va value indicating whether at least one exception occured during the loading.
        /// </summary>
        public bool NoExceptionDuringLoading { get; set; }

        /// <summary>
        ///     Gets all types of the loaded assemblies, having a certain attribute type
        /// </summary>
        /// <returns>Enumeration of key value pairs in which the type is associated with the assembly</returns>
        public static IEnumerable<KeyValuePair<Type, T>> GetTypesOfAssemblies<T>() where T : Attribute
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = new List<KeyValuePair<Type, T>>();
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        var found = type.GetCustomAttributes(typeof(T)).FirstOrDefault() as T;
                        if (found == null) continue;

                        types.Add(new KeyValuePair<Type, T>(type, found));
                    }
                }
                catch (Exception e)
                {
                    Logger.Warn(
                        $"Error occured during Enumeration of Types in Assembly: {assembly.FullName}: {e.Message}");
                }

                foreach (var type in types) yield return type;
            }
        }

        /// <summary>
        ///     Starts the plugins in all loaded assemblies by calling each class which has the implementation
        ///     of the IDatenMeisterPlugin-Interface
        /// </summary>
        /// <param name="kernel">The dependency injector</param>
        /// <param name="pluginLoader">The plugin loader being used</param>
        /// <param name="loadingPosition">Defines the plugin position currently used</param>
        /// <returns>true, if all plugins have been started without exception</returns>
        public bool StartPlugins(
            ILifetimeScope kernel,
            IPluginLoader pluginLoader,
            PluginLoadingPosition loadingPosition)
        {
            Logger.Debug("Starting Plugins" + loadingPosition);
            _pluginTypes ??= pluginLoader.GetPluginTypes();
            var pluginList = _pluginTypes
                .Where(type => GetPluginLoadingPosition(type).HasFlag(loadingPosition))
                .Select(type =>
                {
                    if (_instantiatedPlugins.TryGetValue(type, out var plugin)) return plugin;

                    var result = (IDatenMeisterPlugin) kernel.Resolve(type);
                    _instantiatedPlugins[type] = result;
                    return result;
                })
                .ToList();

            NoExceptionDuringLoading = true;

            var lastCount = pluginList.Count;
            while (pluginList.Count != 0)
            {
                // Go through the list and check which plugins can be loaded in current round
                foreach (var plugin in pluginList.ToList().OrderBy(x => x.GetType().FullName))
                {
                    //
                    // Checks whether the current plugin is dependent upon another non-loaded plugin
                    var dependent = false;
                    foreach (var dependencyType in GetPluginDependencies(plugin))
                        if (pluginList.Any(x => x.GetType() == dependencyType))
                        {
                            // The current plugin is dependent upon the current dependency
                            dependent = true;
                            break;
                        }

                    if (dependent) continue;

                    //
                    // Now, start the plugin
                    pluginList.Remove(plugin);

                    Logger.Info($"Starting plugin [{loadingPosition}]: {plugin.GetType().FullName}");
                    if (Debugger.IsAttached)
                        // When a debugger is attached, we are directly interested to figure out that an exception was thrown
                        plugin.Start(loadingPosition);
                    else
                        try
                        {
                            plugin.Start(loadingPosition);
                        }
                        catch (Exception exc)
                        {
                            NoExceptionDuringLoading = false;
                            Logger.Error($"Failed plugin: {exc}");

                            if (Debugger.IsAttached) throw;
                        }
                }

                // Checks whether we have an endless loop in the plugin system
                var currentCount = pluginList.Count;
                if (currentCount == lastCount)
                {
                    Logger.Fatal("Endless Loop in Plugin System");
                    throw new InvalidOperationException("Endless Loop in Plugin System");
                }
            }

            return NoExceptionDuringLoading;
        }

        /// <summary>
        ///     Gets the enumeration of Plugin Dependency Attributes upon the given plugin instance
        /// </summary>
        /// <param name="plugin">Instance which is queried</param>
        /// <returns>The dependencies</returns>
        private IEnumerable<PluginDependencyAttribute> GetPluginDependencyAttribute(IDatenMeisterPlugin plugin)
        {
            foreach (var attribute in plugin.GetType().GetCustomAttributes(typeof(PluginDependencyAttribute), false))
                yield return (PluginDependencyAttribute) attribute;
        }

        /// <summary>
        ///     Gets the enumeration of Plugin Dependency Attributes upon the given plugin instance
        /// </summary>
        /// <param name="type">Instance which is queried</param>
        /// <returns>The dependencies</returns>
        private PluginLoadingAttribute? GetPluginLoadingAttribute(Type type)
        {
            foreach (var attribute in type.GetCustomAttributes(typeof(PluginLoadingAttribute), false))
                return (PluginLoadingAttribute) attribute;

            return null;
        }

        /// <summary>
        ///     Gets the enumeration of Plugin Dependency Attributes upon the given plugin instance
        /// </summary>
        /// <param name="plugin">Instance which is queried</param>
        /// <returns>The dependencies</returns>
        private IEnumerable<Type> GetPluginDependencies(IDatenMeisterPlugin plugin)
        {
            return GetPluginDependencyAttribute(plugin).Select(x => x.DependentType);
        }

        private PluginLoadingPosition GetPluginLoadingPosition(Type plugin)
        {
            var attribute = GetPluginLoadingAttribute(plugin);
            return attribute?.PluginLoadingPosition ?? PluginLoadingPosition.AfterLoadingOfExtents;
        }
    }
}