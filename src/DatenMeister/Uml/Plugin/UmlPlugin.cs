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

        private readonly NamedElementMethods _namedElementMethods;
        private readonly IDotNetTypeLookup _typeLookup;

        /// <summary>
        /// Defines the view logic
        /// </summary>
        private readonly ViewLogic _viewLogic;

        public UmlPlugin(IWorkspaceLogic workspaceLogic, NamedElementMethods namedElementMethods, IDotNetTypeLookup typeLookup, ViewLogic viewLogic)
        {
            _workspaceLogic = workspaceLogic;
            _namedElementMethods = namedElementMethods;
            _typeLookup = typeLookup;
            _viewLogic = viewLogic;
        }

        public void Start()
        {
            InitViews();
        }

        /// <summary>
        /// Initializes the views for the given extent
        /// </summary>
        private void InitViews()
        {
            var defaultView = new DefaultViewForMetaclass
            {
                metaclass = "UML::StructuredClassifiers::Class",
                viewType = ViewType.Detail
            };

            var form = new Form {name = "Class"};
            form.fields.Add(
                new TextFieldData
                {
                    name = "name",
                    title = "Classname"
                });
            form.fields.Add(
                new SubElementFieldData("ownedAttribute", "Properties"));
            form.fields.Add(
                new SubElementFieldData("attribute", "Properties"));

            defaultView.view = form;
            var element = _typeLookup.CreateDotNetElement(defaultView);

            _viewLogic.Add(element);

            defaultView.viewType = ViewType.List;
            _viewLogic.Add(element);
        }
    }
}