using System;
using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;

namespace DatenMeister.Provider.ManagementProviders
{
    /// <summary>
    /// Gets the workspace view
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ViewDefinitions
    {
        /// <summary>
        /// Stores the view logic
        /// </summary>
        private readonly ViewLogic _viewLogic;

        /// <summary>
        /// Stores the name of the package
        /// </summary>
        public const string PackageName = "Management";

        public const string NewWorkspaceForm = "NewWorkspaceForm";

        public const string NewXmiDetailForm = "NewXmiDetailForm";

        public const string WorkspaceListView = "WorkspaceListView";

        public const string ExtentListView = "ExtentListView";

        public const string PathNewXmiDetailForm = PackageName + "::" + NewXmiDetailForm;

        public const string PathWorkspaceListView = PackageName + "::" + WorkspaceListView;

        public const string PathExtentListView = PackageName + "::" + ExtentListView;

        public const string PathNewWorkspaceForm = PackageName + "::" + NewWorkspaceForm;

        /// <summary>
        /// Initializes a new instance of the ViewDefinitions class
        /// </summary>
        /// <param name="viewLogic">View logic being used to find View Extent</param>
        public ViewDefinitions(ViewLogic viewLogic)
        {
            _viewLogic = viewLogic;
        }

        /// <summary>
        /// Gets the detail form to create a new workspace
        /// </summary>
        /// <returns></returns>
        public IElement GetNewWorkspaceDetail()
        {
            var form = new Form(NewWorkspaceForm);
            form.fields.Add(
                new TextFieldData("id", "Name"));
            form.fields.Add(
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

            form.fields.Add(
                new TextFieldData("id", "Name")
                {
                    isReadOnly = true
                });
            form.fields.Add(
                new TextFieldData("annotation", "Annotation"));
            form.fields.Add(
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

            form.fields.Add(
                new TextFieldData("uri", "URI"));
            form.fields.Add(
                new TextFieldData("count", "# of items")
                {
                    isReadOnly = true
                });
            form.fields.Add(
                new TextFieldData("type", "Provider-Type")
                {
                    isReadOnly = true
                });

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

            form.fields.Add(
                new TextFieldData("uri", "URI")
                {
                    defaultValue = "dm:///"
                });
            form.fields.Add(
                new TextFieldData("filepath", "Path to Xmi File")
                {
                    defaultValue =
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/"
                });

            return DotNetSetter.Convert(_viewLogic.GetViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Adds the views to the view logic
        /// </summary>
        public void AddToViewDefinition()
        {
            var viewExtent = _viewLogic.GetViewExtent();

            // Creates the package containing the views
            var factory = new MofFactory(viewExtent);
            var managementPackage = factory.create(null);
            managementPackage.set("name", PackageName);

            var items = new List<IElement>
            {
                GetWorkspaceListForm(),
                GetExtentListForm(),
                GetNewXmiExtentDetailForm(),
                GetNewWorkspaceDetail()
            };
            managementPackage.set(_UML._CommonStructure._Namespace.member, items);

            viewExtent.elements().add(managementPackage);
        }
    }
}