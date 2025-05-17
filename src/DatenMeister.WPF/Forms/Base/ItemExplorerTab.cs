using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    /// The tab instance containing the information about the control and other additional information
    /// </summary>
    public class ItemExplorerTab : TabItem
    {
        public ItemExplorerTab(IElement form)
        {
            Form = form;
            _content = new ExplorerTabContent();
            Content = _content;
        }

        /// <summary>
        /// Gets or sets the view definition being used for the tab
        /// </summary>
        public IElement Form { get; set; }

        /// <summary>
        /// Stores the content
        /// </summary>
        private readonly ExplorerTabContent _content;

        /// <summary>
        /// Gets or sets the control
        /// </summary>
        public UserControl Control
        {
            get => _content.InnerContent.Content as UserControl ??
                   throw new InvalidOperationException("UserControl == null");
            set => _content.InnerContent.Content = value;
        }

        /// <summary>
        /// Gets the enclosed control as INavigationGuest
        /// </summary>
        public INavigationGuest ControlAsNavigationGuest => (INavigationGuest) Control;

        public void EvaluateViewExtensions(IEnumerable<ViewExtension> viewExtensions)
        {
            var helper = new MenuHelper(
                _content.Menu,
                NavigationScope.Collection | NavigationScope.Item)
            {
                ShowApplicationItems = false,
                // ReSharper disable once SuspiciousTypeConversion.Global
                Item = Control is IItemNavigationGuest itemNavigationGuest ? itemNavigationGuest.Item : null,
                Collection = Control is ICollectionNavigationGuest collectionNavigationGuest
                    ? collectionNavigationGuest.Collection
                    : null,
                Extent = Control is IExtentNavigationGuest extentNavigationGuest ? extentNavigationGuest.Extent : null
            };

            helper.EvaluateExtensions(viewExtensions);
        }
    }
}