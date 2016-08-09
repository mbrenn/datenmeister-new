using System.Collections.Generic;
using DatenMeister.Web.Models.Modules;

namespace DatenMeister.Web.Modules
{
    public class ClientModulePlugin : IClientModulePlugin
    {
        /// <summary>
        /// Stores the list of script paths
        /// </summary>
        public List<string> ScriptPaths { get; } = new List<string>();

        /// <summary>
        /// Adds a script path
        /// </summary>
        /// <param name="path">Path to be added</param>
        public void AddScript(string path)
        {
            if (!ScriptPaths.Contains(path))
            {
                ScriptPaths.Add(path);
            }
        }
    }
}