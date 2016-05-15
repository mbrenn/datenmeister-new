﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using DatenMeister.Plugins;
using Ninject;

namespace DatenMeister.Integration
{
    public static class Helper
    {
        public static void LoadAllAssembliesFromCurrentDirectory()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LoadAssembliesFromFolder(directoryName);
        }

        /// <summary>
        /// Loads all referenced assemblies
        /// </summary>
        public static void LoadAllReferenceAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                .Where(x=>!IsDotNetLibrary(x.GetName())))
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
                if (AppDomain.CurrentDomain.GetAssemblies().All(a => a.FullName != name.FullName))
                {
                    Debug.WriteLine($"Loading Assembly: {name}");
                    LoadReferencedAssembly(Assembly.Load(name));
                }
            }
        }

        public static void LoadAssembliesFromFolder(string path)
        {
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
                        Debug.WriteLine($"Loading by file: {file}");
                        var assembly = Assembly.LoadFile(Path.Combine(path, file));
                        Debug.WriteLine($"Loaded  by file: {assembly.FullName}");
                    }
                }
            }
            else
            {
                 Debug.WriteLine($"Directory does not exist: {path}");
            }
        }

        public static void StartPlugins(StandardKernel kernel)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Go through all types and check, if the type has implemented the interface for the pluging
                foreach (var type in assembly.GetTypes())
                {
                    // Checks, if one of the class implements the IDatenMeisterPlugin
                    if (type.GetInterfaces().Any(x => x == typeof(IDatenMeisterPlugin)))
                    {
                        Debug.WriteLine($"Starting plugin: {type}");
                        ((IDatenMeisterPlugin) kernel.Get(type)).Start();
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