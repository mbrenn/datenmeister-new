namespace DatenMeister.WPF.Modules.ViewExtensions.Definition
{
    /// <summary>
    /// Defines an extension for the view, like row buttons, Navigation buttons and
    /// additional buttons. Depending on the control being used, the values will be evaluated
    /// </summary>
    public class ViewExtension
    {
        /// <summary>
        /// Gets or sets a tag that can be used to identify the purpose of the viewextension
        /// </summary>
        public object? Tag
        {
            get;
            set;
        }
    }
}