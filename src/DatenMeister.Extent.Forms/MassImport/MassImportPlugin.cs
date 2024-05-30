using DatenMeister.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Extent.Forms.MassImport
{
    [PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
    internal class MassImportPlugin : IDatenMeisterPlugin
    {
        public Task Start(PluginLoadingPosition position)
        {
            return Task.CompletedTask;
        }
    }
}
