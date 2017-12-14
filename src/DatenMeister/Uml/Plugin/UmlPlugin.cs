using DatenMeister.Core;
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
            // Creates the forms
            var umlClassForm = new Form(
                "Class",
                new MetaClassElementFieldData("Metaclass"),
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Classname"),
                new SubElementFieldData(_UML._StructuredClassifiers._Class.ownedAttribute, "Properties"));
            _viewLogic.Add(umlClassForm);

            var umlPackageForm = new Form(
                "Package",
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Classname"),
                new SubElementFieldData(_UML._CommonStructure._Namespace.member, "Member"));
            _viewLogic.Add(umlPackageForm);

            // Creates the default mapping
            var classView = new DefaultViewForMetaclass(
                WorkspaceNames.UriUml + "#Class",
                ViewType.Detail,
                umlClassForm);
            _viewLogic.Add(classView);

            var packageView = new DefaultViewForMetaclass(
                WorkspaceNames.UriUml + "#Package",
                ViewType.Detail,
                umlPackageForm);

            _viewLogic.Add(packageView);

            var classExtentView = new DefaultViewForExtentType(
                "Uml.Classes",
                umlClassForm);

            _viewLogic.Add(classExtentView);
        }
    }
}