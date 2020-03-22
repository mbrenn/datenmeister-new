using DatenMeister.Modules.Forms.FormFinder;
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
        public const string PackageName = "PackageName";
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly PackageMethods _packageMethods;
        private readonly FormLogic _formLogic;

        /// <summary>
        /// Initializes a new instance of the IssueMeisterPlugin
        /// </summary>
        /// <param name="workspaceLogic">Defines the workspacelogic</param>
        /// <param name="packageMethods"></param>
        public IssueMeisterPlugin(IWorkspaceLogic workspaceLogic, PackageMethods packageMethods, FormLogic formLogic)
        {
            _workspaceLogic = workspaceLogic;
            _packageMethods = packageMethods;
            _formLogic = formLogic;
        }

        public void Start(PluginLoadingPosition position)
        {
            // Import 

            _packageMethods.ImportByManifest(
                typeof(IssueMeisterPlugin),
                "IssueMeisterLib.Xmi.IssueMeister.xml",
                PackageName,
                _formLogic.GetInternalFormExtent(),
                PackageName);
        }
    }
}