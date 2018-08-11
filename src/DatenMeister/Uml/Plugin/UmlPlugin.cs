using DatenMeister.Core;
using DatenMeister.Core.Plugins;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
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

        public const string PackageName = "Uml";

        public UmlPlugin(ViewLogic viewLogic, IWorkspaceLogic workspaceLogic, PackageMethods packageMethods)
        {
            _viewLogic = viewLogic;
            _workspaceLogic = workspaceLogic;
            _packageMethods = packageMethods;
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
            
            var viewExtent = _viewLogic.GetInternalViewExtent();
            _packageMethods.GetOrCreatePackageStructure(viewExtent.elements(), PackageName);
            

            // Creates the forms
            var umlExtentForm = new ListForm(
                "List - Classes",
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

            _viewLogic.Add(ViewLocationType.Internal, umlExtentForm);

            // Creates the forms
            var umlPropertyForm = new Form(
                "Detail - Property",
                new MetaClassElementFieldData("Metaclass"),
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Property"),
                new ReferenceFieldData(_UML._CommonStructure._TypedElement.type, "Type of Property"));

            _viewLogic.Add(ViewLocationType.Internal, umlPropertyForm);

            // Creates the forms
            var umlClassForm = new ListForm(
                "List - Classes",
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
            _viewLogic.Add(ViewLocationType.Internal, umlClassForm);

            var umlPackageForm = new Form(
                "Detail - Package",
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Package"),
                new SubElementFieldData(_UML._Packages._Package.packagedElement, "Packaged Elements")
                {
                    defaultTypesForNewElements = new[]
                    {
                        umlData.Packages.__Package,
                        umlData.StructuredClassifiers.__Class
                    }
                });
            _viewLogic.Add(ViewLocationType.Internal, umlPackageForm);

            // Creates the default mapping
            var classView = new ViewAssociation(
                ViewType.Detail,
                umlClassForm)
            {
                metaclassName = WorkspaceNames.UriUmlExtent + "#Class",
            };
            _viewLogic.Add(ViewLocationType.Internal, classView);

            var packageView = new ViewAssociation(
                ViewType.Detail,
                umlPackageForm)
            {
                metaclassName = WorkspaceNames.UriUmlExtent + "#Package"
            };
            _viewLogic.Add(ViewLocationType.Internal, packageView);

            var propertyView = new ViewAssociation(
                ViewType.Detail,
                umlPropertyForm)
            {
                metaclassName = WorkspaceNames.UriUmlExtent + "#Property"
            };
            _viewLogic.Add(ViewLocationType.Internal, propertyView);

            var classExtentView = new ViewAssociation(
                ViewType.Detail,
                umlExtentForm)
            {
                extentType = "Uml.Classes",
            };

            _viewLogic.Add(ViewLocationType.Internal, classExtentView);
        }
    }
}