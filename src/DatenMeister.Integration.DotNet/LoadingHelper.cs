using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using DatenMeister.Core.Plugins;

namespace DatenMeister.Integration.DotNet
{
    public static class LoadingHelper
    {
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

        public static void StartPlugins(ILifetimeScope kernel)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Go through all types and check, if the type has implemented the interface for the pluging 
                foreach (var type in assembly.GetTypes())
                {
                    // Checks, if one of the class implements the IDatenMeisterPlugin 
                    if (type.GetInterfaces().Any(x => x == typeof(IDatenMeisterPlugin)))
                    {
                        try
                        {
                            Debug.WriteLine($"Starting plugin: {type.FullName}");
                            ((IDatenMeisterPlugin) kernel.Resolve(type)).Start();
                        }
                        catch (Exception exc)
                        {
                            Debug.WriteLine($"Failed plugin: {exc}");
                        }
                    }
                }
            }
        }

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