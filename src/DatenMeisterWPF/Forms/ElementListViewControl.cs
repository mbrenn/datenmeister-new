using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms
{
    public class ElementListViewControl : ListViewControl
    {
        private IElement _originalformDefinition;
        private IReflectiveSequence _sequence;

        /// <summary>
        /// Shows the enumeration of the reflection. It also creates a view if required.
        /// </summary>
        /// <param name="scope">Scope to be used to retrieve additional objects</param>
        /// <param name="sequence">Sequence to be shown</param>
        /// <param name="formDefinition">Form to be shown</param>
        public new void SetContent(IReflectiveSequence sequence, IElement formDefinition)
        {
            _originalformDefinition = formDefinition;
            _sequence = sequence;

            RefreshViewDefinition();

            base.SetContent(sequence, FormDefinition);
        }

        /// <summary>
        /// Performs an automatic creation of the view if no view definition was given.
        /// </summary>
        public override void RefreshViewDefinition()
        {
            if (_originalformDefinition == null)
            {
                var viewFinder = App.Scope.Resolve<IViewFinder>();
                FormDefinition = viewFinder.CreateView(_sequence);
            }
            else
            {
                FormDefinition = _originalformDefinition;
            }
        }
    }
}