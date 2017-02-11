using DatenMeister.Core.Plugins;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Uml.Plugin
{
    // ReSharper disable once UnusedMember.Global
    public class UmlPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Defines the view logic
        /// </summary>
        private readonly ViewLogic _viewLogic;

        public UmlPlugin(ViewLogic viewLogic)
        {
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
            var classView = new DefaultViewForMetaclass(
                WorkspaceNames.UriUml + "#Class",
                ViewType.Detail,
                new Form(
                    "Class",
                    new TextFieldData("name", "Classname"),
                    new SubElementFieldData("ownedAttribute", "Properties"),
                    new SubElementFieldData("attribute", "Properties")));
            
            AddForView(classView);

            var packageView = new DefaultViewForMetaclass(
                WorkspaceNames.UriUml + "#Package",
                ViewType.Detail,
                new Form(
                    "Package",
                    new TextFieldData("name", "Classname")));

            AddForView(packageView);

            var classExtentView = new DefaultViewForExtentType(
                "Uml.Classes",
                new Form(
                    "Class",
                    new TextFieldData("name", "Classname"),
                    new SubElementFieldData("ownedAttribute", "Properties"),
                    new SubElementFieldData("attribute", "Properties")));

            AddForView(classExtentView);
        }

        /// <summary>
        /// Adds a default view for as a detail and list item
        /// </summary>
        /// <param name="classView">Defaultview to be added</param>
        private void AddForView(DefaultViewForMetaclass classView)
        {
            _viewLogic.Add(classView);
            /*
            var element = _typeLookup.CreateDotNetElement(classView);
            _viewLogic.Add(element);
            */
        }

        /// <summary>
        /// Adds a default view for as a detail and list item
        /// </summary>
        /// <param name="classView">Defaultview to be added</param>
        private void AddForView(DefaultViewForExtentType classView)
        {
            _viewLogic.Add(classView);
            /*
            var element = _typeLookup.CreateDotNetElement(classView);
            _viewLogic.Add(element);
            */
        }
    }
}