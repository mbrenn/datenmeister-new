using System.Collections.Generic;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base.ViewExtensions;

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
        }

        /// <summary>
        /// Gets or sets the view definition being used for the tab
        /// </summary>
        public IElement Form { get; set; }

        /// <summary>
        /// Gets or sets the control
        /// </summary>
        public ItemListViewControl Control
        {
            get => Content as ItemListViewControl;
            set => Content = value;
        }

        public IEnumerable<ViewExtension> ViewExtensions { get; set; }
    }
}