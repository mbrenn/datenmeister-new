using System.Collections.Generic;
using Autofac.Features.ResolveAnything;
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
        private readonly ViewLogic _viewLogic;

        /// <summary>
        /// Initializes a new instance of the ViewDefinitions class
        /// </summary>
        /// <param name="viewLogic">View logic being used to find View Extent</param>
        public ViewDefinitions(ViewLogic viewLogic)
        {
            _viewLogic = viewLogic;
        }

        /// <summary>
        /// Creates a form object
        /// </summary>
        /// <returns>The created form</returns>
        private IElement GetWorkspaceListForm()
        {
            // Finds the forms
            var form = new Form("WorkspaceListView")
            {
                inhibitNewItems = true
            };

            form.fields.Add(
                new TextFieldData("id", "Name"));
            form.fields.Add(
                new TextFieldData("annotation", "Annotation"));
            form.fields.Add(
                new TextFieldData("extents", "Extents")
                {
                    isEnumeration = true
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
            var form = new Form("ExtentListView")
            {
                inhibitNewItems = true
            };

            form.fields.Add(
                new TextFieldData("uri", "URI"));
            form.fields.Add(
                new TextFieldData("count", "# of items"));
            form.fields.Add(
                new TextFieldData("type", "Provider-Type"));

            return DotNetSetter.Convert(_viewLogic.GetViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Gets the form data for the user where he can create a new Xmi extent
        /// </summary>
        /// <returns>The created form element</returns>
        private IElement GetNewXmiExtentDetailForm()
        {
            // Finds the forms
            var form = new Form("NewXmiDetailForm")
            {
                inhibitNewItems = true
            };

            form.fields.Add(
                new TextFieldData("uri", "URI"));
            form.fields.Add(
                new TextFieldData("filepath", "Path to Xmi File"));

            return DotNetSetter.Convert(_viewLogic.GetViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Stores the name of the package
        /// </summary>
        public const string PackageName = "Management";

        public const string NewXmiDetailFormName = "NewXmiDetailForm";

        public const string PathNewXmiDetailForm = PackageName + "::" + NewXmiDetailFormName;

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
                GetNewXmiExtentDetailForm()
            };
            managementPackage.set(_UML._CommonStructure._Namespace.member, items);

            viewExtent.elements().add(managementPackage);
        }
    }
}