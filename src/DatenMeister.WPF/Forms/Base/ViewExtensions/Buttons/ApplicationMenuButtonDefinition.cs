#nullable enable

using System;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons
{
    /// <summary>
    /// This definition is shown generically to the complete application and does not have a reference to
    /// a shown extent or item
    /// </summary>
    public class ApplicationMenuButtonDefinition : NavigationButtonDefinition
    {
        ///<summary>
        /// Gets the action being executed when the user clicked upon the button
        /// </summary>
        public Action OnPressed { get; }

        public ApplicationMenuButtonDefinition(
            string name,
            Action onPressed,
            string? imageName,
            string categoryName,
            int priority = 0) : base(name, NavigationScope.Application, imageName, categoryName, priority)
        {
            OnPressed = onPressed;
        }
    }
}