using System.Collections.Generic;
using DatenMeister.Modules.Forms.FormModifications;

namespace DatenMeister.Modules.Forms
{
    public class FormsPluginState
    {
        /// <summary>
        /// Lists all form modificationPlugins
        /// </summary>
        public List<IFormModificationPlugin> FormModificationPlugins { get; } = new List<IFormModificationPlugin>();
    }
}
