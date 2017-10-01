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
        private readonly ViewLogic _viewLogic;
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
            var form = new Form();
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
    }
}