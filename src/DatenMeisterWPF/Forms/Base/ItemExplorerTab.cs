using System.Windows.Controls;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// The tab instance containing the information about the control and other additional information
    /// </summary>
    public class ItemExplorerTab : TabItem
    {
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