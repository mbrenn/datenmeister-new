using DatenMeister.Core;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Plugins;
using DatenMeister.Types;
using System.Threading.Tasks;

namespace IssueMeisterLib
{
    public class IssueMeisterPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Stores the name of the package
        /// </summary>
        public const string PackageName = "IssueMeister";
        public const string TargetPackageName = "Apps::IssueMeister";
        public const string ExtentTypeName = "IssueMeister";
        private readonly FormMethods _formsPlugin;
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly ExtentSettings _extentSettings;

        /// <summary>
        /// Initializes a new instance of the IssueMeisterPlugin
        /// </summary>
        /// <param name="formsPlugin">Sets the form logic</param>
        /// <param name="localTypeSupport">Sets the local type support</param>
        /// <param name="scopeStorage">The settings for the extent</param>
        public IssueMeisterPlugin(
            FormMethods formsPlugin,
            LocalTypeSupport localTypeSupport, 
            IScopeStorage scopeStorage)
        {
            _formsPlugin = formsPlugin;
            _localTypeSupport = localTypeSupport;
            _extentSettings = scopeStorage.Get<ExtentSettings>();
        }

        public Task Start(PluginLoadingPosition position)
        {
            // Import 
            PackageMethods.ImportByManifest(
                typeof(IssueMeisterPlugin),
                "IssueMeisterLib.Xmi.IssueMeister.Forms.xml",
                PackageName,
                _formsPlugin.GetInternalFormExtent(),
                TargetPackageName);
            
            // Import 
            PackageMethods.ImportByManifest(
                typeof(IssueMeisterPlugin),
                "IssueMeisterLib.Xmi.IssueMeister.Types.xml",
                PackageName,
                _localTypeSupport.InternalTypes,
                TargetPackageName);

            var extentSetting =
                new ExtentType(ExtentTypeName);
            extentSetting.rootElementMetaClasses.Add(
                _localTypeSupport.InternalTypes.element("#IssueMeister.Issue"));
            _extentSettings.extentTypeSettings.Add(extentSetting);

            return Task.CompletedTask;
        }
    }
}