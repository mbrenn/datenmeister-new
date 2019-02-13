using System.Windows.Controls;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    /// The tab instance containing the information about the control and other additional information
    /// </summary>
    public class ItemExplorerTab : TabItem
    {
        public ItemExplorerTab(ViewDefinition viewDefinition)
        {
            ViewDefinition = viewDefinition;
        }

        /// <summary>
        /// Gets or sets the view definition being used for the tab
        /// </summary>
        public ViewDefinition ViewDefinition { get; set; }

        /// <summary>
        /// Gets or sets the control
        /// </summary>
        public ItemListViewControl Control
        {
            get => Content as ItemListViewControl;
            set => Content = value;
        }
    }
}