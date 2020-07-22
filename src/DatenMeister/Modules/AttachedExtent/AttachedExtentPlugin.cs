using System.Linq;
using DatenMeister.Models.AttachedExtent;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Runtime;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.AttachedExtent
{
    [PluginLoading(PluginLoadingPosition.AfterInitialization | PluginLoadingPosition.AfterLoadingOfExtents)]
    public class AttachedExtentPlugin : IDatenMeisterPlugin
    {
        private readonly PackageMethods _packageMethods;
        private readonly FormsPlugin _formsPlugin;
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly ExtentSettings _extentSettings;

        public AttachedExtentPlugin(
            LocalTypeSupport localTypeSupport, IScopeStorage scopeStorage, PackageMethods packageMethods,
            FormsPlugin formsPlugin)
        {
            _localTypeSupport = localTypeSupport;
            _packageMethods = packageMethods;
            _formsPlugin = formsPlugin;
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
                        name = AttachedExtentHandler.AttachedExtentProperty,
                        title = "Attached Extent",
                        metaClass =
                            (from x in createdTypes
                                let hasId = (IHasId) x
                                where hasId.Id?.Contains(typeof(AttachedExtentConfiguration).FullName) == true
                                select x)
                            .First()
                    });
            }
            else if ((position & PluginLoadingPosition.AfterLoadingOfExtents) != 0)
            {
                _packageMethods.ImportByManifest(
                    typeof(AttachedExtentPlugin),
                    "DatenMeister.XmiFiles.Modules.AttachedExtent.xmi",
                    "Forms",
                    _formsPlugin.GetInternalFormExtent(),
                    "Reports");
            }
        }
    }
}