using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für DetailFormWindow.xaml
    /// </summary>
    public partial class DetailFormWindow : Window
    {
        public DetailFormWindow()
        {
            InitializeComponent();
        }
        
        public void UpdateContent(IDatenMeisterScope scope, IElement element, IElement formDefinition)
        {
            DetailFormControl.UpdateContent(scope, element, formDefinition);
        }
    }
}
