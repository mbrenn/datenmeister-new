using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Integration;

namespace DatenMeister.Runtime.Plugins
{
    public class PluginManager
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(PluginManager));

        /// <summary>
        /// Gets or sets va value indicating whtether at least one exception occured during the loading.
        /// </summary>
        public bool NoExceptionDuringLoading { get; set; }

        public static void LoadAllAssembliesFromCurrentDirectory()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LoadAssembliesFromFolder(directoryName);
        }

        /// <summary>
        /// Loads all referenced assemblies
        /// </summary>
        public static void LoadAllReferencedAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !IsDotNetLibrary(x.GetName())))
            {
                LoadReferencedAssembly(assembly);
            }
        }

        /// <summary>
        /// Loads all referenced assembly of the given assembly and all subassemblies
        /// </summary>
        /// <param name="assembly"></param>
        private static void LoadReferencedAssembly(Assembly assembly)
        {
            // All assemblies, which do not start with Microsoft or System.
            // We will not find any extent or something like that within these assemblies.
            foreach (var name in assembly.GetReferencedAssemblies()
                .Where(x => !IsDotNetLibrary(x))
                .Where(x => AppDomain.CurrentDomain.GetAssemblies().All(a =>
                    !string.Equals(a.GetName().Name, x.Name, StringComparison.CurrentCultureIgnoreCase))))
            {
                var innerAssembly = Assembly.Load(name);
                Logger.Info($"Loaded (2): {innerAssembly}");
                LoadReferencedAssembly(innerAssembly);
            }
        }

        public static void LoadAssembliesFromFolder(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(Environment.CurrentDirectory, path);
            }

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path)
                    .Where(x => Path.GetExtension(x).ToLower() == ".dll");
                foreach (var file in files)
                {
                    var filenameWithoutExtension = Path.GetFileNameWithoutExtension(file).ToLower();
                    if (IsDotNetLibrary(filenameWithoutExtension))
                    {
                        // Skip .Net Library
                        continue;
                    }

                    if (AppDomain.CurrentDomain.GetAssemblies().All(
                        x => x.GetName().Name.ToLower() != filenameWithoutExtension))
                    {
                        try
                        {
                            var assembly = Assembly.LoadFile(Path.Combine(path, file));
                            Logger.Info($"Loaded (1): {assembly.GetName().Name}, {assembly.GetName().Version}");
                        }
                        catch (Exception e)
                        {
                            Logger.Error($"Loading of assembly {file} failed: {e}");
                        }
                    }
                }
            }
            else
            {
                Logger.Warn($"Directory does not exist: {path}");
            }
        }

        /// <summary>
        /// Gets all types of the loaded assemblies, having a certain attribute type
        /// </summary>
        /// <param name="attributeType">Attribute Type being queried</param>
        /// <returns>Enumeration of Types containing the attribute</returns>
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
                        if (found == null)
                        {
                            continue;
                        }

                        types.Add(new KeyValuePair<Type, T>(type, found));
                    }
                }
                catch (Exception e)
                {
                    Logger.Warn($"Error occured during Enumeration of Types in Assembly: {assembly.FullName}: {e.Message}");
                }

                foreach (var type in types)
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Starts the plugins in all loaded assemblies by calling each class which has the implementation
        /// of the IDatenMeisterPlugin-Interface
        /// </summary>
        /// <param name="kernel">Dependency Kernel to be used</param>
        /// <param name="loadingPosition">Defines the plugin position currently used</param>
        /// <returns>true, if all plugins have been started without exception</returns>
        public bool StartPlugins(ILifetimeScope kernel, PluginLoadingPosition loadingPosition)
        {
            var pluginList = new List<IDatenMeisterPlugin>();

            NoExceptionDuringLoading = true;

            LoadAssembliesFromFolder(Path.GetDirectoryName(typeof(DatenMeisterScope).Assembly.Location));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    // Go through all types and check, if the type has implemented the interface for the pluging
                    pluginList.AddRange(
                        assembly.GetTypes()
                            .Where(type => type.GetInterfaces().Any(x => x == typeof(IDatenMeisterPlugin)))
                            .Where(type => GetPluginEntry(type).HasFlag(loadingPosition))
                            .Select(type => (IDatenMeisterPlugin) kernel.Resolve(type)));
                }
                catch (ReflectionTypeLoadException e)
                {
                    Logger.Error($"PluginLoader: Exception during assembly loading of {assembly.FullName} [{assembly.Location}]: {e.Message}");
                }
            }

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
                    {
                        if (pluginList.Any(x => x.GetType() == dependencyType))
                        {
                            // The current plugin is dependent upon the current dependency
                            dependent = true;
                            break;
                        }
                    }

                    if (dependent)
                    {
                        continue;
                    }

                    //
                    // Now, start the plugin
                    pluginList.Remove(plugin);

                    Logger.Info($"Starting plugin [{loadingPosition}]: {plugin.GetType().FullName}");
                    if (Debugger.IsAttached)
                    {
                        // When a debugger is attached, we are directly interested to figure out that an exception was thrown
                        plugin.Start(loadingPosition);
                    }
                    else
                    {
                        try
                        {
                            plugin.Start(loadingPosition);
                        }
                        catch (Exception exc)
                        {
                            NoExceptionDuringLoading = false;
                            Logger.Error($"Failed plugin: {exc}");

                            if (Debugger.IsAttached)
                            {
                                throw;
                            }
                        }
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
        /// Gets the enumeration of Plugin Dependency Attributes upon the given plugin instance
        /// </summary>
        /// <param name="plugin">Instance which is queried</param>
        /// <returns>The dependencies</returns>
        private IEnumerable<PluginDependencyAttribute> GetPluginDependencyAttribute(IDatenMeisterPlugin plugin)
        {
            foreach (var attribute in plugin.GetType().GetCustomAttributes(typeof(PluginDependencyAttribute), false))
            {
                yield return (PluginDependencyAttribute) attribute;
            }
        }

        /// <summary>
        /// Gets the enumeration of Plugin Dependency Attributes upon the given plugin instance
        /// </summary>
        /// <param name="type">Instance which is queried</param>
        /// <returns>The dependencies</returns>
        private PluginLoadingAttribute? GetPluginLoadingAttribute(Type type)
        {
            foreach (var attribute in type.GetCustomAttributes(typeof(PluginLoadingAttribute), false))
            {
                return (PluginLoadingAttribute) attribute;
            }

            return null;
        }

        /// <summary>
        /// Gets the enumeration of Plugin Dependency Attributes upon the given plugin instance
        /// </summary>
        /// <param name="plugin">Instance which is queried</param>
        /// <returns>The dependencies</returns>
        private IEnumerable<Type> GetPluginDependencies(IDatenMeisterPlugin plugin) =>
            GetPluginDependencyAttribute(plugin).Select(x => x.DependentType);

        private PluginLoadingPosition GetPluginEntry(Type plugin)
        {
            var attribute = GetPluginLoadingAttribute(plugin);
            return attribute?.PluginLoadingPosition ?? PluginLoadingPosition.AfterLoadingOfExtents;
        }

        /// <summary>
        /// Gets true, if the given library is a dotnet library which
        /// starts with System, Microsoft or mscorlib.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly</param>
        /// <returns></returns>
        private static bool IsDotNetLibrary(AssemblyName assemblyName) =>
            assemblyName.FullName.StartsWith("microsoft", StringComparison.InvariantCultureIgnoreCase) ||
            assemblyName.FullName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase) ||
            assemblyName.FullName.StartsWith("system", StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets true, if the given library is a dotnet library which
        /// starts with System, Microsoft or mscorlib.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly</param>
        /// <returns>true, if the given library is a .Net Library</returns>
        private static bool IsDotNetLibrary(string assemblyName) =>
            assemblyName.StartsWith("microsoft", StringComparison.InvariantCultureIgnoreCase) ||
            assemblyName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase) ||
            assemblyName.StartsWith("system", StringComparison.InvariantCultureIgnoreCase);
    }
}