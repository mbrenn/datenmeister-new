using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Gets the workspace view
    /// </summary>
    public class WorkspaceView
    {
        private ViewLogic _viewLogic;
        private readonly WorkspaceUriResolver _uriResolver;

        public WorkspaceView(ViewLogic viewLogic, WorkspaceUriResolver uriResolver)
        {
            _viewLogic = viewLogic;
            _uriResolver = uriResolver;
        }

        /// <summary>
        /// Creates a form object
        /// </summary>
        /// <returns>The created form</returns>
        public IElement CreateForm()
        {
            // Finds the forms
            var formType =
                _uriResolver.ResolveById(
                    MofUriExtent.GetIdOfUri("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.Form"));
            var textFieldDataType = _uriResolver.ResolveById(
                MofUriExtent.GetIdOfUri(
                    "dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.TextFieldData"));

            var factory = new MofFactory(_viewLogic.GetViewExtent());
            var form = factory.create(formType);

            var nameField = factory.create(textFieldDataType);
            nameField.set(_FormAndFields._FieldData.name, "Name");
            nameField.set(_FormAndFields._FieldData.title, "id");

            // Sets the fields of form
            var fields = new[] {nameField};
            form.set(_FormAndFields._Form.fields, fields);

            return form;
        }
    }
}