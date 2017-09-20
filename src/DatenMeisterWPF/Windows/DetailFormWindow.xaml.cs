using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;

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
            if (formDefinition == null)
            {
                var viewFinder = scope.Resolve<IViewFinder>();
                formDefinition = viewFinder.FindView(element, null);
            }

            DetailFormControl.UpdateContent(scope, element, formDefinition);
        }
    }
}
