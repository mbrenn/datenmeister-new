using System.Collections.Generic;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms
{
    public class FormsPluginState
    {
        /// <summary>
        /// Lists all form modificationPlugins
        /// </summary>
        public List<IFormModificationPlugin> FormModificationPlugins { get; } = new List<IFormModificationPlugin>();
    }
}
