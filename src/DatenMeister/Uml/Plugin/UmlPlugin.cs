using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
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
            var umlClassForm = new ListForm(
                "Class",
                new MetaClassElementFieldData("Metaclass"),
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Class"),
                new SubElementFieldData(_UML._StructuredClassifiers._Class.ownedAttribute, "Properties")
                {
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
            
            // Creates the forms
            var umlPropertyForm = new Form(
                "Property",
                new MetaClassElementFieldData("Metaclass"),
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Property"),
                new ReferenceFieldData(_UML._CommonStructure._TypedElement.type, "Type of Property"));

            _viewLogic.Add(umlPropertyForm);

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

            var propertyView = new DefaultViewForMetaclass(
                WorkspaceNames.UriUml + "#Property",
                ViewType.Detail,
                umlPropertyForm);
            _viewLogic.Add(propertyView);

            var classExtentView = new DefaultViewForExtentType(
                "Uml.Classes",
                umlClassForm);
            _viewLogic.Add(classExtentView);
        }
    }
}