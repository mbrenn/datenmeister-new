using System.Collections.Generic;

namespace DatenMeister.Web.Modules
{
    public interface IClientModulePlugin
    {
        /// <summary>
        /// Stores the list of script paths
        /// </summary>
        List<string> ScriptPaths { get; }

        /// <summary>
        /// Adds a script path
        /// </summary>
        /// <param name="path">Path to be added</param>
        void AddScript(string path);
    }
}