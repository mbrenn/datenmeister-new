using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Gets the workspace view
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WorkspaceViewDefinition
    {
        private readonly ViewLogic _viewLogic;

        public WorkspaceViewDefinition(ViewLogic viewLogic)
        {
            _viewLogic = viewLogic;
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