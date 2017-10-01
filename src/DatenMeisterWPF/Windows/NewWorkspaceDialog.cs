using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;

namespace DatenMeisterWPF.Windows
{
    public class NewWorkspaceDialog : DetailFormWindow
    {
        public void SetContent(IDatenMeisterScope scope)
        {
            var viewLogic = scope.Resolve<ViewLogic>();
            var viewExtent = viewLogic.GetViewExtent();
            var form = new Form();
            form.fields.Add(
                new TextFieldData("id", "Name"));
            form.fields.Add(
                new TextFieldData("annotation", "Annotation"));

            var formElement = DotNetSetter.Convert(viewExtent, form) as IElement;
            UpdateContent(scope, null, formElement);
        }
    }
}