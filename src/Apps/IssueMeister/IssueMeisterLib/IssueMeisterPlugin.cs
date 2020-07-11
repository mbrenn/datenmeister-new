using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents.Configuration;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

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
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly PackageMethods _packageMethods;
        private readonly FormsPlugin _formsPlugin;
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly ExtentSettings _extentSettings;

        /// <summary>
        /// Initializes a new instance of the IssueMeisterPlugin
        /// </summary>
        /// <param name="workspaceLogic">Defines the workspacelogic</param>
        /// <param name="packageMethods"></param>
        /// <param name="formsPlugin">Sets the form logic</param>
        /// <param name="localTypeSupport">Sets the local type support</param>
        /// <param name="extentSettings">The settings for the extent</param>
        public IssueMeisterPlugin(IWorkspaceLogic workspaceLogic, PackageMethods packageMethods, FormsPlugin formsPlugin,
            LocalTypeSupport localTypeSupport, ExtentSettings extentSettings)
        {
            _workspaceLogic = workspaceLogic;
            _packageMethods = packageMethods;
            _formsPlugin = formsPlugin;
            _localTypeSupport = localTypeSupport;
            _extentSettings = extentSettings;
        }

        public void Start(PluginLoadingPosition position)
        {
            // Import 
            _packageMethods.ImportByManifest(
                typeof(IssueMeisterPlugin),
                "IssueMeisterLib.Xmi.IssueMeister.Forms.xml",
                PackageName,
                _formsPlugin.GetInternalFormExtent(),
                TargetPackageName);
            
            // Import 
            _packageMethods.ImportByManifest(
                typeof(IssueMeisterPlugin),
                "IssueMeisterLib.Xmi.IssueMeister.Types.xml",
                PackageName,
                _localTypeSupport.InternalTypes,
                TargetPackageName);

            var extentSetting =
                new ExtentTypeSetting(ExtentTypeName);
            extentSetting.rootElementMetaClasses.Add(
                _localTypeSupport.InternalTypes.element("#IssueMeister.Issue"));
            _extentSettings.extentTypeSettings.Add(extentSetting);
        }
    }
}