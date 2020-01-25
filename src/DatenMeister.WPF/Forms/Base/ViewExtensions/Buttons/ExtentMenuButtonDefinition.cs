#nullable enable

using System;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons
{
    /// <summary>
    /// This definition is allocated to the current extent to which the item belongs to.
    /// This is shown within the context of the extent
    /// </summary>
    public class ExtentMenuButtonDefinition : NavigationButtonDefinition
    {
        ///<summary>
        /// Gets the action being executed when the user clicked upon the button
        /// </summary>
        public Action<IExtent> OnPressed { get; }

        public ExtentMenuButtonDefinition(
            string name,
            Action<IExtent> onPressed,
            string? imageName,
            string categoryName,
            int priority = 0) : base(name, NavigationScope.Extent, imageName, categoryName, priority)
        {
            OnPressed = onPressed;
        }
    }
}