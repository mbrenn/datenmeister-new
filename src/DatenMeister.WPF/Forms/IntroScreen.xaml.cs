using System.Windows.Controls;
using BurnSystems.Logging;
using BurnSystems.WPF;

namespace DatenMeister.WPF.Forms
{
    /// <summary>
    /// Interaktionslogik für IntroScreen.xaml
    /// </summary>
    public partial class IntroScreen : UserControl
    {
        public IntroScreen()
        {
            InitializeComponent();
            TheLog.AddProvider(new TextBlockLogProvider(LoggingText));
        }
    }
}
