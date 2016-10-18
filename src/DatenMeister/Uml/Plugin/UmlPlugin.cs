 using DatenMeister.Core.EMOF.Interface.Identifiers;
 using DatenMeister.Core.EMOF.Interface.Reflection;
 using DatenMeister.Core.Plugins;
 using DatenMeister.Models.Forms;
 using DatenMeister.Modules.ViewFinder;
 using DatenMeister.Provider.DotNet;
 using DatenMeister.Runtime;
 using DatenMeister.Runtime.Workspaces;
 using DatenMeister.Uml.Helper;

namespace DatenMeister.Uml.Plugin
{
    // ReSharper disable once UnusedMember.Global
    public class UmlPlugin : IDatenMeisterPlugin
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly NamedElementMethods _namesElementMethods;
        private readonly IDotNetTypeLookup _typeLookup;

        /// <summary>
        /// Defines the view logic
        /// </summary>
        private readonly ViewLogic _viewLogic;

        public UmlPlugin(IWorkspaceLogic workspaceLogic, NamedElementMethods namesElementMethods, IDotNetTypeLookup typeLookup, ViewLogic viewLogic)
        {
            _workspaceLogic = workspaceLogic;
            _namesElementMethods = namesElementMethods;
            _typeLookup = typeLookup;
            _viewLogic = viewLogic;
        }

        public void Start()
        {
            var umlWorkspace = _workspaceLogic.GetById(WorkspaceNames.NameUml);
            var umlUriExtent = umlWorkspace.FindExtent(WorkspaceNames.UriUml);
            InitViews(umlUriExtent);

            var mofWorkspace = _workspaceLogic.GetById(WorkspaceNames.NameMof);
            umlUriExtent = mofWorkspace.FindExtent(WorkspaceNames.UriUml);
            InitViews(umlUriExtent);
        }

        /// <summary>
        /// Initializes the views for the given extent
        /// </summary>
        /// <param name="extent"></param>
        private void InitViews(IUriExtent extent)
        {
            var umlClass = _namesElementMethods.GetByFullName(extent, "UML::StructuredClassifiers::Class");
            var defaultView = new DefaultViewForMetaclass
            {
                metaclass = umlClass.GetUri(),
                viewType = ViewType.Detail
            };

            var form = new Form {name = "Class"};
            form.fields.Add(
                new TextFieldData
                {
                    name = "name",
                    title = "Classname"
                });

            defaultView.view = form;
            var element = _typeLookup.CreateDotNetElement(defaultView);
            _viewLogic.Add(element);
        }
    }
}