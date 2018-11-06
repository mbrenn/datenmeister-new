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
            var umlListClassForm = new ListForm(
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
            umlListClassForm.defaultTypesForNewElements = new[]
            {
                umlData.Packages.__Package,
                umlData.StructuredClassifiers.__Class
            };

            PackageMethods.AddObjectToPackage(umlPackage,
                DotNetConverter.ConvertToMofObject(viewExtent, umlListClassForm, "umlListClass") as IElement);

            // Creates the forms
            var umlDetailPropertyForm = new Form(
                "Detail - Property",
                new MetaClassElementFieldData("Metaclass"),
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Property"),
                new ReferenceFieldData(_UML._CommonStructure._TypedElement.type, "Type of Property"));

            PackageMethods.AddObjectToPackage(umlPackage,
                DotNetConverter.ConvertToMofObject(viewExtent, umlDetailPropertyForm, "umlDetailProperty") as IElement);

            // Form for packages
            var umlListPackageForm = new Form(
                "List - Package",
                new TextFieldData(_UML._CommonStructure._NamedElement.name, "Name of Package"),
                new SubElementFieldData(_UML._Packages._Package.packagedElement, "Packaged Elements")
                {
                    defaultTypesForNewElements = new[]
                    {
                        umlData.Packages.__Package,
                        umlData.StructuredClassifiers.__Class
                    }
                });

            PackageMethods.AddObjectToPackage(umlPackage,
                DotNetConverter.ConvertToMofObject(viewExtent, umlListPackageForm, "umlListPackage") as IElement);

            // Form for packages
            var umlDetailPackageForm = new Form(
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

            PackageMethods.AddObjectToPackage(umlPackage,
                DotNetConverter.ConvertToMofObject(viewExtent, umlDetailPackageForm, "umlDetailPackage") as IElement);

            // Creates the default mappings
            var classView = new ViewAssociation(ViewType.Detail, umlListClassForm)
            {
                metaclassName = WorkspaceNames.UriUmlExtent + "#Class",
            };
            PackageMethods.AddObjectToPackage(umlPackage, DotNetConverter.ConvertToMofObject(viewExtent, classView) as IElement);

            var packageView = new ViewAssociation(ViewType.Detail, umlDetailPackageForm)
            {
                metaclassName = WorkspaceNames.UriUmlExtent + "#Package"
            };
            PackageMethods.AddObjectToPackage(umlPackage, DotNetConverter.ConvertToMofObject(viewExtent, packageView) as IElement);

            var propertyView = new ViewAssociation(ViewType.Detail, umlDetailPropertyForm)
            {
                metaclassName = WorkspaceNames.UriUmlExtent + "#Property"
            };
            PackageMethods.AddObjectToPackage(umlPackage, DotNetConverter.ConvertToMofObject(viewExtent, propertyView) as IElement);

            var classExtentView = new ViewAssociation(ViewType.List, umlListClassForm)
            {
                extentType = "Uml.Classes",
            };
            PackageMethods.AddObjectToPackage(umlPackage,
                DotNetConverter.ConvertToMofObject(viewExtent, classExtentView) as IElement);



            var packageListView = new ViewAssociation(ViewType.List, umlDetailPackageForm)
            {
                //metaclass = umlData.Packages.__Package,
                metaclassName = NamedElementMethods.GetFullName(umlData.Packages.__Package)
            };
            PackageMethods.AddObjectToPackage(umlPackage, DotNetConverter.ConvertToMofObject(viewExtent, packageListView) as IElement);

            var classListView = new ViewAssociation(ViewType.List, umlListClassForm)
            {
                //metaclass = umlData.StructuredClassifiers.__Class,
                metaclassName = NamedElementMethods.GetFullName(umlData.StructuredClassifiers.__Class)
            };
            PackageMethods.AddObjectToPackage(umlPackage, DotNetConverter.ConvertToMofObject(viewExtent, classListView) as IElement);

            var propertyListView = new ViewAssociation(ViewType.List, umlDetailPropertyForm)
            {
                //metaclass = umlData.Classification.__Property,
                metaclassName = NamedElementMethods.GetFullName(umlData.Classification.__Property)
            };
            PackageMethods.AddObjectToPackage(umlPackage, DotNetConverter.ConvertToMofObject(viewExtent, propertyListView) as IElement);
        }
    }
}