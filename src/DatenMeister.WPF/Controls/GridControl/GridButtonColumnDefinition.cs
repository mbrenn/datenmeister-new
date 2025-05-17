using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Controls.GridControl
{
    /// <summary>
    /// Gets the column definition for buttons
    /// </summary>
    public class GridButtonColumnDefinition : GridColumnDefinition
    {
        /// <summary>
        /// Defines the action that shall be performed when the user has clicked on the button
        /// </summary>
        public Action<INavigationGuest, IObject>? OnPressed;
    }
}