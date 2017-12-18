using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Provider.ManagementProviders
{
    /// <summary>
    /// Gets the workspace view
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ManagementViewDefinitions
    {
        /// <summary>
        /// Stores the view logic
        /// </summary>
        private readonly ViewLogic _viewLogic;

        /// <summary>
        /// Stores the view logic
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Stores the name of the package
        /// </summary>
        public const string PackageName = "ManagementProvider";

        public const string NewWorkspaceForm = "NewWorkspaceForm";

        public const string NewXmiDetailForm = "NewXmiDetailForm";

        public const string WorkspaceListView = "WorkspaceListView";

        public const string ExtentListView = "ExtentListView";

        public const string FindTypeForm = "FindTypeForm";

        public const string PathNewXmiDetailForm = PackageName + "::" + NewXmiDetailForm;

        public const string PathFindTypeForm = PackageName + "::" + FindTypeForm;

        public const string PathWorkspaceListView = PackageName + "::" + WorkspaceListView;

        public const string PathExtentListView = PackageName + "::" + ExtentListView;

        public const string PathNewWorkspaceForm = PackageName + "::" + NewWorkspaceForm;

        /// <summary>
        /// Initializes a new instance of the ManagementViewDefinitions class
        /// </summary>
        /// <param name="viewLogic">View logic being used to find View Extent</param>
        /// <param name="workspaceLogic">Logic of the workspace</param>
        public ManagementViewDefinitions(ViewLogic viewLogic, IWorkspaceLogic workspaceLogic)
        {
            _viewLogic = viewLogic;
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Gets the detail form to create a new workspace
        /// </summary>
        /// <returns></returns>
        public IElement GetNewWorkspaceDetail()
        {
            var form = new Form(NewWorkspaceForm);
            form.AddFields(
                new TextFieldData("id", "Name"),
                new TextFieldData("annotation", "Annotation"));

            return DotNetSetter.Convert(_viewLogic.GetViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Creates a form object
        /// </summary>
        /// <returns>The created form</returns>
        private IElement GetWorkspaceListForm()
        {
            // Finds the forms
            var form = new Form(WorkspaceListView)
            {
                inhibitNewItems = true
            };

            form.AddFields(
                new TextFieldData("id", "Name")
                {
                    isReadOnly = true
                },
                new TextFieldData("annotation", "Annotation"),
                new TextFieldData("extents", "Extents")
                {
                    isEnumeration = true,
                    isReadOnly = true
                });

            return DotNetSetter.Convert(_viewLogic.GetViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Gets the view for the extent
        /// </summary>
        /// <returns>The created form</returns>
        private IElement GetExtentListForm()
        {
            // Finds the forms
            var form = new Form(ExtentListView)
            {
                inhibitNewItems = true
            };

            form.AddFields(
                new TextFieldData("uri", "URI"),
                new TextFieldData("count", "# of items")
                {
                    isReadOnly = true
                },
                new TextFieldData("type", "Provider-Type")
                {
                    isReadOnly = true
                },
                new TextFieldData("extentType", "Extent Type"));

            return DotNetSetter.Convert(_viewLogic.GetViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Gets the form data for the user where he can create a new Xmi extent
        /// </summary>
        /// <returns>The created form element</returns>
        private IElement GetNewXmiExtentDetailForm()
        {
            // Finds the forms
            var form = new Form(NewXmiDetailForm)
            {
                inhibitNewItems = true
            };

            form.AddFields(
                new TextFieldData("uri", "URI")
                {
                    defaultValue = "dm:///"
                },
                new TextFieldData("filepath", "Path to Xmi File")
                {
                    defaultValue =
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/"
                });

            return DotNetSetter.Convert(_viewLogic.GetViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Creates a form to create a new type. 
        /// </summary>
        /// <param name="types">Types to be added to the form</param>
        /// <returns>The created type</returns>
        public IElement GetFindTypeForm(IEnumerable<IElement> types = null)
        {
            var form = new Form(FindTypeForm)
            {
                inhibitNewItems = true,
                fixView = true,
                hideMetaClass = true,
                minimizeDesign = true,
                defaultHeight = 300,
                defaultWidth = 500
            };

            var typeField = new DropDownFieldData("selectedType", "Type");
            if (types != null)
            {
                foreach (var type in types)
                {
                    typeField.AddValue(type, NamedElementMethods.GetFullName(type));
                }
            }

            typeField.values = typeField.values.OrderBy(x => x.name).ToList();
            form.AddFields(typeField);

            var createdForm = DotNetSetter.Convert(_viewLogic.GetViewExtent(), form) as IElement;
            return createdForm;
        }

        /// <summary>
        /// Adds the views to the view logic
        /// </summary>
        public void AddToViewDefinition()
        {
            var viewExtent = _viewLogic.GetViewExtent();
            var factory = viewExtent.GetFactory();
            var formAndFields = _workspaceLogic.GetTypesWorkspace().Get<_FormAndFields>();

            // Creates the package containing the views
            var managementPackage = factory.create(null);
            managementPackage.set("name", PackageName);

            var workspaceForm = GetWorkspaceListForm();
            var extentForm = GetExtentListForm();
            var items = new List<IElement>
            {
                workspaceForm,
                extentForm,
                GetNewXmiExtentDetailForm(),
                GetNewWorkspaceDetail(),
                GetFindTypeForm()
            };

            var workspaceFormDefaultView = factory.create(formAndFields.__DefaultViewForMetaclass);
            workspaceFormDefaultView.SetProperties(
                new Dictionary<string, object>
                {
                    [_FormAndFields._DefaultViewForMetaclass.metaclass] = WorkspaceObject.MetaclassUriPath,
                    [_FormAndFields._DefaultViewForMetaclass.view] = workspaceForm,
                    [_FormAndFields._DefaultViewForMetaclass.viewType] = ViewType.Detail
                }
            );
            items.Add(workspaceFormDefaultView);

            var extentFormDefaultView = factory.create(formAndFields.__DefaultViewForMetaclass);
            extentFormDefaultView.SetProperties(
                new Dictionary<string, object>
                {
                    [_FormAndFields._DefaultViewForMetaclass.metaclass] = ExtentObject.MetaclassUriPath,
                    [_FormAndFields._DefaultViewForMetaclass.view] = extentForm,
                    [_FormAndFields._DefaultViewForMetaclass.viewType] = ViewType.Detail
                }
            );
            items.Add(extentFormDefaultView);

            managementPackage.set(_UML._CommonStructure._Namespace.member, items);
            viewExtent.elements().add(managementPackage);
        }
    }
}