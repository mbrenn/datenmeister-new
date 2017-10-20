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
        private IElement _originalformDefinition;
        private IReflectiveSequence _sequence;
        private IDatenMeisterScope _scope;

        /// <summary>
        /// Shows the enumeration of the reflection. It also creates a view if required.
        /// </summary>
        /// <param name="scope">Scope to be used to retrieve additional objects</param>
        /// <param name="sequence">Sequence to be shown</param>
        /// <param name="formDefinition">Form to be shown</param>
        public new void SetContent(IDatenMeisterScope scope, IReflectiveSequence sequence, IElement formDefinition)
        {
            _originalformDefinition = formDefinition;
            _sequence = sequence;
            _scope = scope;

            RefreshViewDefinition();

            base.SetContent(scope, sequence, FormDefinition);
        }

        /// <summary>
        /// Performs an automatic creation of the view if no view definition was given.
        /// </summary>
        public override void RefreshViewDefinition()
        {
            if (_originalformDefinition == null)
            {
                var viewFinder = _scope.Resolve<IViewFinder>();
                FormDefinition = viewFinder.CreateView(_sequence);
            }
            else
            {
                FormDefinition = _originalformDefinition;
            }
        }
    }
}