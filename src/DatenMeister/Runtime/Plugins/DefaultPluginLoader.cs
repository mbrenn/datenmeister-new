using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BurnSystems.Logging;

namespace DatenMeister.Runtime.Plugins
{
    /// <summary>
    /// Loads the plugins
    /// </summary>
    public class DefaultPluginLoader : IPluginLoader
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(DefaultPluginLoader));

        /// <summary>
        /// Gets the list of loaded 
        /// </summary>
        /// <returns>List of types which elong to a plugin</returns>
        public List<Type> GetPluginTypes()
        {
            var pluginList = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
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
                    if (IsDotNetLibrary(filenameWithoutExtension))
                    {
                        // Skip .Net Library
                        continue;
                    }

                    if (AppDomain.CurrentDomain.GetAssemblies().All(
                        x => x.GetName().Name?.ToLower() != filenameWithoutExtension))
                    {
                        try
                        {
                            //var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(path, file));
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
        /// Loads all assemblies from the current directories 
        /// </summary>
        public void LoadAllAssembliesFromCurrentDirectory()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                ?? throw new InvalidOperationException("directoryName is not set");
            LoadAssembliesFromFolder(directoryName);
        }

        /// <summary>
        /// Loads all referenced assemblies from the assemblies being currently loaded. 
        /// </summary>
        public void LoadAllReferencedAssemblies()
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
        /// <param name="assembly">Assembly whose references shall be loaded</param>
        private void LoadReferencedAssembly(Assembly assembly)
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
        

        /// <summary>
        /// Gets true, if the given library is a dotnet library which
        /// starts with System, Microsoft or mscorlib.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly</param>
        /// <returns></returns>
        public static bool IsDotNetLibrary(AssemblyName assemblyName) =>
            assemblyName.FullName.StartsWith("microsoft", StringComparison.InvariantCultureIgnoreCase) ||
            assemblyName.FullName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase) ||
            assemblyName.FullName.StartsWith("system", StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets true, if the given library is a dotnet library which
        /// starts with System, Microsoft or mscorlib.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly</param>
        /// <returns>true, if the given library is a .Net Library</returns>
        public static bool IsDotNetLibrary(string assemblyName) =>
            assemblyName.StartsWith("microsoft", StringComparison.InvariantCultureIgnoreCase) ||
            assemblyName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase) ||
            assemblyName.StartsWith("system", StringComparison.InvariantCultureIgnoreCase);
    }
}