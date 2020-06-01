using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using BurnSystems.Logging;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.NetCore.Modules.PluginLoader
{
    /// <summary>
    /// Defines the plugin-loader for .Net Core
    /// </summary>
    public class DotNetCorePluginLoader : IPluginLoader
    {
        public List<Type> GetPluginTypes()
        {
            var pluginList = new List<Type>();

            foreach (var assembly in AssemblyLoadContext.Default.Assemblies)
            {
                try
                {
                    // Go through all types and check, if the type has implemented the interface for the pluging
                    pluginList.AddRange(
                        assembly.GetTypes()
                            .Where(type => type.GetInterfaces().Any(x => x == typeof(IDatenMeisterPlugin))));
                }
                catch (ReflectionTypeLoadException e)
                {
                    Logger.Error(
                        $"PluginLoader: Exception during assembly loading of {assembly.FullName} [{assembly.Location}]: {e.Message}");
                }
            }

            return pluginList;
        }
        
        /// <summary>
        /// The class logger being used
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(DotNetCorePluginLoader));

        /// <summary>
        /// Loads all assemblies from the specific folder into the current context
        /// </summary>
        /// <param name="path">Path to directory whose assemblies are loaded</param>
        public void LoadAssembliesFromFolder(string path)
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
                    if (DefaultPluginLoader.IsDotNetLibrary(filenameWithoutExtension))
                    {
                        // Skip .Net Library
                        continue;
                    }

                    if (AssemblyLoadContext.Default.Assemblies.All(
                        x => x.GetName()!.Name!.ToLower() != filenameWithoutExtension))
                    {
                        try
                        {
                            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(path, file));
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
    }
}