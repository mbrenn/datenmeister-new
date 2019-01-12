using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Plugins;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.Uml.Plugin;

namespace DatenMeister.Provider.ManagementProviders
{
    [PluginDependency(typeof(UmlPlugin))]
    public class ManagementViewPlugin : IDatenMeisterPlugin
    {
        private readonly ViewLogic _viewLogic;
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly PackageMethods _packageMethods;


        /// <summary>
        /// Initializes a new instance of the ManagementViewDefinitions class
        /// </summary>
        /// <param name="viewLogic">View logic being used to find View Extent</param>
        /// <param name="workspaceLogic">Logic of the workspace</param>
        public ManagementViewPlugin(ViewLogic viewLogic, IWorkspaceLogic workspaceLogic, PackageMethods packageMethods)
        {
            _viewLogic = viewLogic;
            _workspaceLogic = workspaceLogic;
            _packageMethods = packageMethods;
        }

        public void Start(PluginLoadingPosition position)
        {
            AddToViewDefinition();
        }

        /// <summary>
        /// Adds the views to the view logic
        /// </summary>
        public void AddToViewDefinition()
        {
            var viewExtent = _viewLogic.GetInternalViewExtent();

            // Creates the package for "ManagementProvider" containing the views
            var targetPackage = _packageMethods.GetOrCreatePackageStructure(viewExtent.elements(), ManagementViewDefinitions.PackageName);

            using (var stream = typeof(ManagementViewPlugin).GetTypeInfo().Assembly.GetManifestResourceStream(
                "DatenMeister.XmiFiles.Views.WorkspacesAndExtents.xmi"))
            {
                var document = XDocument.Load(stream);
                var pseudoProvider = new XmiProvider(document);
                var pseudoExtent = new MofExtent(pseudoProvider)
                {
                    Workspace = viewExtent.GetWorkspace()
                };

                var sourcePackage = _packageMethods.GetOrCreatePackageStructure(
                    pseudoExtent.elements(),
                    ManagementViewDefinitions.PackageName);
                PackageMethods.ImportPackage(sourcePackage, targetPackage);
            }
        }
    }
}