using System.Collections.Generic;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
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
        public ItemListViewControl Control
        {
            get => _content.InnerContent.Content as ItemListViewControl;
            set => _content.InnerContent.Content = value;
        }

        public IEnumerable<ViewExtension> ViewExtensions { get; set; }

        public void EvaluateViewExtensions(ItemExplorerControl control)
        {
            var helper = new MenuHelper(_content.Menu, NavigationScope.Collection | NavigationScope.Item)
            {
                ShowApplicationItems = false,
                // ReSharper disable once SuspiciousTypeConversion.Global
                Item = control is IItemNavigationGuest itemNavigationGuest ? itemNavigationGuest.Item : null,
                Collection = control is ICollectionNavigationGuest collectionNavigationGuest
                    ? collectionNavigationGuest.Collection
                    : null,
                Extent = control is IExtentNavigationGuest extentNavigationGuest ? extentNavigationGuest.Extent : null
            };
            helper.EvaluateExtensions(ViewExtensions);
        }
    }
}