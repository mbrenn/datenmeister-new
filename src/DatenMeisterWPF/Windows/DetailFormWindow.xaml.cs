using System.Windows;
using System.Windows.Controls.Ribbon;

namespace DatenMeisterWPF.Windows
{
    public interface IHasRibbon
    {
        /// <summary>
        /// Gets the ribbon
        /// </summary>
        /// <returns></returns>
        Ribbon GetRibbon();
    }

    /// <summary>
    /// Interaktionslogik für DetailFormWindow.xaml
    /// </summary>
    public partial class DetailFormWindow : Window, IHasRibbon
    {
        /// <summary>
        /// Gets the ribbon
        /// </summary>
        /// <returns></returns>
        public Ribbon GetRibbon() => MainRibbon;

        public DetailFormWindow()
        {
            InitializeComponent();
        }
    }
}
