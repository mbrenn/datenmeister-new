using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;

namespace DatenMeister.Core.Plugins
{
    public class PluginManager
    {
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
                .Where(x => !IsDotNetLibrary(x)))
            {
                if (AppDomain.CurrentDomain.GetAssemblies().All(a => !string.Equals(a.GetName().Name, name.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var innerAssembly = Assembly.Load(name);
                    Debug.WriteLine($"Loaded (2): {innerAssembly}");
                    LoadReferencedAssembly(innerAssembly);
                }
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
                    if (AppDomain.CurrentDomain.GetAssemblies().All(
                        x => x.GetName().Name.ToLower() != filenameWithoutExtension))
                    {
                        try
                        {
                            var assembly = Assembly.LoadFile(Path.Combine(path, file));
                            Debug.WriteLine($"Loaded (1): {assembly.GetName().Name}, {assembly.GetName().Version}");
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine($"Loading of assembly {file} failed: {e}");
                        }
                    }
                }
            }
            else
            {
                Debug.WriteLine($"Directory does not exist: {path}");
            }
        }

        /// <summary>
        /// Starts the plugins in all loaded assemblies by calling each class which has the implementation
        /// of the IDatenMeisterPlugin-Interface
        /// </summary>
        /// <param name="kernel">Dependency Kernel to be used</param>
        /// <returns>true, if all plugins have been started without exception</returns>
        public bool StartPlugins(ILifetimeScope kernel)
        {
            var pluginList = new List<IDatenMeisterPlugin>();

            NoExceptionDuringLoading = true;

            LoadAssembliesFromFolder(".");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Go through all types and check, if the type has implemented the interface for the pluging 
                foreach (var type in assembly.GetTypes())
                {
                    // Checks, if one of the class implements the IDatenMeisterPlugin 
                    if (type.GetInterfaces().Any(x => x == typeof(IDatenMeisterPlugin)))
                    {
                        pluginList.Add((IDatenMeisterPlugin) kernel.Resolve(type));
                    }
                }
            }

            var lastCount = pluginList.Count;
            while (pluginList.Count != 0)
            {
                // Go through the list and check which plugins can be loaded in current round
                foreach (var plugin in pluginList.ToList().OrderBy(x=>x.GetType().FullName))
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

                    Debug.WriteLine($"Starting plugin: {plugin.GetType().FullName}");
                    if (Debugger.IsAttached)
                    {
                        // When a debugger is attached, we are directly interested to figure out that an exception was thrown
                        plugin.Start();
                    }
                    else
                    {
                        try
                        {
                            plugin.Start();
                        }
                        catch (Exception exc)
                        {

                            NoExceptionDuringLoading = false;
                            Debug.WriteLine($"Failed plugin: {exc}");

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
        /// <param name="plugin">Instance which is queried</param>
        /// <returns>The dependencies</returns>
        private IEnumerable<Type> GetPluginDependencies(IDatenMeisterPlugin plugin) =>
            GetPluginDependencyAttribute(plugin).Select(x => x.DependentType);

        /// <summary> 
        /// Gets true, if the given library is a dotnet library which  
        /// starts with System, Microsoft or mscorlib.   
        /// </summary> 
        /// <param name="assemblyName">Name of the assembly</param> 
        /// <returns></returns> 
        private static bool IsDotNetLibrary(AssemblyName assemblyName)
        {
            return assemblyName.FullName.StartsWith("Microsoft") ||
                   assemblyName.FullName.StartsWith("mscorlib") ||
                   assemblyName.FullName.StartsWith("System");
        }
    }
}