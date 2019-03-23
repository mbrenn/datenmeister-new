using System;
using DatenMeister.Core.Plugins;
using DatenMeister.Modules.DataViews.Model;
using DatenMeister.Modules.TypeSupport;

namespace DatenMeister.Modules.DataViews
{
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class DataViewPlugin : IDatenMeisterPlugin
    {
        private readonly LocalTypeSupport _localTypeSupport;

        public static Type[] GetTypes()
        {
            return new[]
            {
                typeof(DataView),
                typeof(ViewNode)
            };
        }

        public DataViewPlugin(LocalTypeSupport localTypeSupport)
        {
            _localTypeSupport = localTypeSupport;
        }


        /// <summary>
        /// Starts the plugin
        /// </summary>
        /// <param name="position"></param>
        public void Start(PluginLoadingPosition position)
        {
            _localTypeSupport.AddInternalTypes(GetTypes(), DataViewLogic.PackagePathTypesDataView);
        }
    }
}