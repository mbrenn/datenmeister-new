using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms
{
    public class ElementListViewControl : ListViewControl
    {
        /// <summary>
        /// Shows the enumeration of the reflection
        /// </summary>
        /// <param name="scope">Scope to be used to retrieve additional objects</param>
        /// <param name="sequence">Sequence to be shown</param>
        /// <param name="formDefinition">Form to be shown</param>
        public void Show(IDatenMeisterScope scope, IReflectiveSequence sequence, IElement formDefinition)
        {
            if (formDefinition == null)
            {
                var viewFinder = scope.Resolve<IViewFinder>();
                formDefinition = viewFinder.CreateView(sequence);
            }

            FormDefinition = formDefinition;
            Items = sequence.Cast<IObject>();

            UpdateContent();
        }
    }
}