using System.Linq;
using DatenMeister.Models.AttachedExtent;
using DatenMeister.Models.ManagementProvider;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Runtime;

namespace DatenMeister.Modules.AttachedExtent
{
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class AttachedExtentPlugin : IDatenMeisterPlugin
    {
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly ExtentSettings _extentSettings;

        public AttachedExtentPlugin(LocalTypeSupport localTypeSupport, IScopeStorage scopeStorage)
        {
            _localTypeSupport = localTypeSupport;
            _extentSettings = scopeStorage.Get<ExtentSettings>();
        }

        public void Start(PluginLoadingPosition position)
        {
            if ((position & PluginLoadingPosition.AfterInitialization) != 0)
            {
                var createdTypes = _localTypeSupport.AddInternalTypes("DatenMeister::AttachedExtent", AttachedExtentTypes.GetTypes());
                _extentSettings.propertyDefinitions.Add(
                    new ExtentPropertyDefinition
                    {
                        name = "DatenMeister.AttachedProperty",
                        title = "Attached Property",
                        metaClass =
                            (from x in createdTypes
                                let hasId = (IHasId) x
                                where hasId.Id?.Contains(typeof(AttachedExtentConfiguration).FullName) == true
                                select x)
                            .First()
                    });
            }
        }
    }
}