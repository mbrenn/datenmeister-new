using System;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions
{
    /// <summary>
    /// This definition is allocated to the current extent to which the item belongs to.
    /// This is shown within the context of the extent
    /// </summary>
    public class CollectionMenuButtonDefinition : NavigationButtonDefinition
    {
        ///<summary>
        /// Gets the action being executed when the user clicked upon the button
        /// </summary>
        public Action<IReflectiveCollection> OnPressed { get; }

        public CollectionMenuButtonDefinition(
            string name,
            Action<IReflectiveCollection> onPressed,
            string imageName,
            string categoryName,
            int priority = 0)
            : base(name, NavigationScope.Collection, imageName, categoryName, priority)
        {
            OnPressed = onPressed;
        }
    }
}