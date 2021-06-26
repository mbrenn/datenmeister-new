using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Forms.Helper
{
    /// <summary>
    /// Defines the parameter for the ActionButtonToFormadder
    /// </summary>
    public  class ActionButtonAdderParameter
    {
        /// <summary>
        /// The action button will only be added when the form was created for the given metaclass.
        /// May be null, if the form shall be added for every metaclass
        /// </summary>
        public IElement? MetaClass { get; set; }

        /// <summary>
        /// The view mode to which the action button shall be added
        /// </summary>
        public string ViewMode { get; set; } = ViewModes.Default;

        /// <summary>
        /// Gets or sets the form type to be used for the extension
        /// </summary>
        public _DatenMeister._Forms.___FormType? FormType { get; set; } = null;

        /// <summary>
        /// The action name being used. 
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// The title being used. 
        /// </summary>
        public string Title { get; set; }

        public ActionButtonAdderParameter(string actionName, string title)
        {
            ActionName = actionName;
            Title = title;
        }
    }
}