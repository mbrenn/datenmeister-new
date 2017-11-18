using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DatenMeisterWPF.Helper
{
    /// <summary>
    /// Extents the combobox by setting the width of the combobox to the largest element. 
    /// I have no clue, why this function is not included into WPF itself
    /// </summary>
    public class ComboBoxEx : ComboBox
    {
        /// <summary>
        /// Stores the index of the selected template
        /// </summary>
        private int _selected;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _selected = SelectedIndex;
            SelectedIndex = -1;
            Loaded += ComboBoxEx_Loaded;
        }

        private void ComboBoxEx_Loaded(object sender, RoutedEventArgs e)
        {
            if (Items.Count > 0)
            {
                var popup = GetTemplateChild("PART_Popup") as Popup;
                var content = popup?.Child as FrameworkElement;
                content?.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                MinHeight = (content?.DesiredSize.Height ?? 0) / Items.Count;
                MinWidth = (content?.DesiredSize.Width) ?? 0;
                SelectedIndex = _selected;
            }
        }
    }
}