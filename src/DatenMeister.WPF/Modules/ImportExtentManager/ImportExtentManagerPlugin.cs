using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Uml.Helper;
using DatenMeister.Uml.Plugin;

namespace DatenMeister.WPF.Modules.ImportExtentManager
{
    public class ImportExtentManagerPlugin : IDatenMeisterPlugin
    {
        public const string PackageName = "ImportExtentManager";

        private readonly PackageMethods _packageMethods;

        public ImportExtentManagerPlugin(PackageMethods packageMethods)
        {
            _packageMethods = packageMethods;
        }

        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new ImportExtentViewExtensions(this));
        }

        public void PerformImport(IExtent sourceExtent, IReflectiveCollection items)
        {
            var copier = new ExtentCopier(new MofFactory(items));
            copier.Copy(sourceExtent.elements(), items, CopyOptions.None);
        }
    }
}