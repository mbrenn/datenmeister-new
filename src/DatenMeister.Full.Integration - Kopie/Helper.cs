using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DatenMeister.Full.Integration
{
    public static class Helper
    {
        public static void LoadAllAssembliesInDirectory()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var files = Directory.GetFiles(directoryName)
                .Where (x=>Path.GetExtension(x).ToLower() == ".dll");
            foreach (var file in files)
            {
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(file).ToLower();
                if (AppDomain.CurrentDomain.GetAssemblies().All(
                    x => x.GetName().Name.ToLower() != filenameWithoutExtension))
                {
                    Debug.WriteLine($"Loading by file: {file}");
                    Assembly.LoadFile(Path.Combine(directoryName, file));
                }
            }
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
                    Debug.WriteLine($"Loading: {name}");
                    LoadReferencedAssembly(Assembly.Load(name));
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