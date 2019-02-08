using System.Reflection;
using System.Xml.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Core.Plugins;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Uml.Plugin
{
    // ReSharper disable once UnusedMember.Global
    public class UmlPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Defines the view logic
        /// </summary>
        private readonly ViewLogic _viewLogic;

        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly PackageMethods _packageMethods;
        private readonly NamedElementMethods _namedElementMethods;

        public const string PackageName = "Uml";

        public UmlPlugin(ViewLogic viewLogic, IWorkspaceLogic workspaceLogic, PackageMethods packageMethods, NamedElementMethods namedElementMethods)
        {
            _viewLogic = viewLogic;
            _workspaceLogic = workspaceLogic;
            _packageMethods = packageMethods;
            _namedElementMethods = namedElementMethods;
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
            var targetPackage = _packageMethods.GetOrCreatePackageStructure(viewExtent.elements(), PackageName);

            using (var stream = typeof(ManagementViewPlugin).GetTypeInfo().Assembly.GetManifestResourceStream(
                "DatenMeister.XmiFiles.Views.UML.xmi"))
            {
                var document = XDocument.Load(stream);
                var pseudoProvider = new XmiProvider(document);
                var pseudoExtent = new MofUriExtent(pseudoProvider)
                {
                    Workspace = viewExtent.GetWorkspace()
                };

                var sourcePackage = _packageMethods.GetOrCreatePackageStructure(
                    pseudoExtent.elements(),
                    PackageName);
                PackageMethods.ImportPackage(sourcePackage, targetPackage);
            }
        }
    }
}