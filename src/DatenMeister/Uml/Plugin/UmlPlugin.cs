using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
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
            var umlPackage = _packageMethods.GetOrCreatePackageStructure(viewExtent.elements(), PackageName);

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

            _packageMethods.AddObjectToPackage(umlPackage, DotNetSetter.Convert(viewExtent, umlExtentForm) as IElement);

            // Creates the forms
            var umlPropertyForm = new Form(
                "Detail - Property",
                new MetaClassElementFieldData("Metaclass"),
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Property"),
                new ReferenceFieldData(_UML._CommonStructure._TypedElement.type, "Type of Property"));

            _packageMethods.AddObjectToPackage(umlPackage, DotNetSetter.Convert(viewExtent, umlPropertyForm) as IElement);

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

            _packageMethods.AddObjectToPackage(umlPackage, DotNetSetter.Convert(viewExtent, umlClassForm) as IElement);

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

            _packageMethods.AddObjectToPackage(umlPackage, DotNetSetter.Convert(viewExtent, umlPackageForm) as IElement);

            // Creates the default mapping
            var classView = new ViewAssociation(
                ViewType.Detail,
                umlClassForm)
            {
                metaclassName = WorkspaceNames.UriUmlExtent + "#Class",
            };

            _packageMethods.AddObjectToPackage(umlPackage, DotNetSetter.Convert(viewExtent, classView) as IElement);

            var packageView = new ViewAssociation(
                ViewType.Detail,
                umlPackageForm)
            {
                metaclassName = WorkspaceNames.UriUmlExtent + "#Package"
            };

            _packageMethods.AddObjectToPackage(umlPackage, DotNetSetter.Convert(viewExtent, packageView) as IElement);

            var propertyView = new ViewAssociation(
                ViewType.Detail,
                umlPropertyForm)
            {
                metaclassName = WorkspaceNames.UriUmlExtent + "#Property"
            };
            _packageMethods.AddObjectToPackage(umlPackage, DotNetSetter.Convert(viewExtent, propertyView) as IElement);

            var classExtentView = new ViewAssociation(
                ViewType.Detail,
                umlExtentForm)
            {
                extentType = "Uml.Classes",
            };
            _packageMethods.AddObjectToPackage(umlPackage, DotNetSetter.Convert(viewExtent, classExtentView) as IElement);

        }
    }
}