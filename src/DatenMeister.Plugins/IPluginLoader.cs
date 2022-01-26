using System;
using System.Collections.Generic;

namespace DatenMeister.Plugins
{
    public interface IPluginLoader
    {
        /// <summary>
        ///     Loads all assemblies from the specific folder into the current context
        /// </summary>
        /// <param name="path">Path to directory whose assemblies are loaded</param>
        void LoadAssembliesFromFolder(string path);

        /// <summary>
        ///     Gets the plugins
        /// </summary>
        /// <returns>List of plugins</returns>
        List<Type> GetPluginTypes();
    }
}