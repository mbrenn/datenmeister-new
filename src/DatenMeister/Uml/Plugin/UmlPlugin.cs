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

        private readonly IWorkspaceLogic _workspaceLogic;

        public UmlPlugin(ViewLogic viewLogic, IWorkspaceLogic workspaceLogic)
        {
            _viewLogic = viewLogic;
            _workspaceLogic = workspaceLogic;
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
            var umlData = _workspaceLogic.GetUmlData();

            // Creates the forms
            var umlExtentForm = new ListForm(
                "Extent View for Classes",
                new MetaClassElementFieldData("Metaclass"),
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Class"),
                new SubElementFieldData(_UML._StructuredClassifiers._Class.ownedAttribute, "Properties")
                {
                    defaultTypesForNewElements = new[]
                    {
                        umlData.Classification.__Property
                    }
                });
            umlExtentForm.defaultTypesForNewElements = new[]
            {
                umlData.Packages.__Package,
                umlData.StructuredClassifiers.__Class
            };
            _viewLogic.Add(umlExtentForm);

            // Creates the forms
            var umlPropertyForm = new Form(
                "Property",
                new MetaClassElementFieldData("Metaclass"),
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Property"),
                new ReferenceFieldData(_UML._CommonStructure._TypedElement.type, "Type of Property"));

            _viewLogic.Add(umlPropertyForm);

            // Creates the forms
            var umlClassForm = new ListForm(
                "Extent View for Classes",
                new MetaClassElementFieldData("Metaclass"),
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Class"),
                new SubElementFieldData(_UML._StructuredClassifiers._Class.ownedAttribute, "Properties")
                {
                    form = umlPropertyForm,
                    defaultTypesForNewElements = new[]
                    {
                        umlData.Classification.__Property
                    }
                });
            umlClassForm.defaultTypesForNewElements = new[]
            {
                umlData.Packages.__Package,
                umlData.StructuredClassifiers.__Class
            };
            _viewLogic.Add(umlClassForm);

            var umlPackageForm = new Form(
                "Package",
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Package"),
                new SubElementFieldData(_UML._Packages._Package.packagedElement, "Packaged Elements")
                {
                    defaultTypesForNewElements = new[]
                    {
                        umlData.Packages.__Package,
                        umlData.StructuredClassifiers.__Class
                    }
                });
            _viewLogic.Add(umlPackageForm);

            // Creates the default mapping
            var classView = new DefaultDetailViewForMetaclass(
                WorkspaceNames.UriUml + "#Class",
                ViewType.Detail,
                umlClassForm);
            _viewLogic.Add(classView);

            var packageView = new DefaultDetailViewForMetaclass(
                WorkspaceNames.UriUml + "#Package",
                ViewType.Detail,
                umlPackageForm);
            _viewLogic.Add(packageView);

            var propertyView = new DefaultDetailViewForMetaclass(
                WorkspaceNames.UriUml + "#Property",
                ViewType.Detail,
                umlPropertyForm);
            _viewLogic.Add(propertyView);

            var classExtentView = new DefaultViewForExtentType(
                "Uml.Classes",
                umlExtentForm);
            _viewLogic.Add(classExtentView);
        }
    }
}