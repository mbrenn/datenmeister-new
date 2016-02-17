using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DatenMeister.Full.Integration
{
    public class Helper
    {
        /// <summary>
        /// Loads all referenced assemblies
        /// </summary>
        public static void LoadAllReferenceAssemblies()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
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
            foreach (AssemblyName name in assembly.GetReferencedAssemblies()
                .Where(x => !(
                    x.FullName.StartsWith("Microsoft") ||
                    x.FullName.StartsWith("mscorlib") ||
                    x.FullName.StartsWith("System"))))
            {
                if (AppDomain.CurrentDomain.GetAssemblies().All(a => a.FullName != name.FullName))
                {
                    Debug.WriteLine($"Loading: " + name);
                    LoadReferencedAssembly(Assembly.Load(name));
                }
            }
        }
    }
}