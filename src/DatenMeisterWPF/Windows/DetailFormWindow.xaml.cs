using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    ///     Interaktionslogik für DetailFormWindow.xaml
    /// </summary>
    public partial class DetailFormWindow : Window, IHasRibbon, INavigationHost
    {
        /// <summary>
        /// Defines the logger for the DetailFormWindow
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(DetailFormWindow));
        /// <summary>
        /// Defines the event that will be thrown, when the user has clicked upon 'save' in the inner form.
        /// This event has to be invoked by the child elements. 
        /// </summary>
        public event EventHandler<ItemEventArgs> Saved;

        public DetailFormWindow()
        {
            InitializeComponent();
            RibbonHelper = new RibbonHelper(this);
        }

        private RibbonHelper RibbonHelper { get; }

        /// <summary>
        ///     Gets the ribbon
        /// </summary>
        /// <returns></returns>
        public Ribbon GetRibbon()
        {
            return MainRibbon;
        }

        public IControlNavigation NavigateTo(Func<UserControl> factoryMethod, NavigationMode navigationMode)
        {
            return Navigator.NavigateByCreatingAWindow(this, factoryMethod, navigationMode);
        }

        /// <summary>
        ///     Rebuild the complete navigation
        /// </summary>
        public void RebuildNavigation()
        {
            var extensions = RibbonHelper.GetDefaultNavigation();
            var otherExtensions = (MainContent?.Content as DetailFormControl)?.GetViewExtensions();
            if (otherExtensions != null) extensions = extensions.Union(otherExtensions);

            RibbonHelper.EvaluateExtensions(extensions);
        }

        public void SetFocus()
        {
            if (MainContent == null)
            {
                Focus();
            }
            else
            {
                MainContent?.Focus();
            }
        }

        /// <inheritdoc />
        public Window GetWindow()
        {
            return this;
        }

        private void DetailFormWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            RebuildNavigation();
        }

        /// <summary>
        ///     Sets the main content to be shown
        /// </summary>
        /// <param name="element">Element to be shown</param>
        public void SetMainContent(UIElement element)
        {
            MainContent.Content = element;
            if (element is DetailFormControl control)
            {
                if (control.IsDesignMinimized())
                {
                    SwitchToMinimumSize();
                    MainRibbon.IsMinimized = true;
                }

                var size = control.DefaultSize;
                if (Math.Abs(size.Width) > 1E-7 && size.Height > 1E-7)
                {
                    var window = GetWindow(this);
                    if (window != null)
                    {
                        window.Width = size.Width;
                        window.Height = size.Height;
                    }
                }

                var name = NamedElementMethods.GetName(control.DetailElement);
                if (!string.IsNullOrEmpty(name))
                {
                    Title = $"Edit element: {name}";
                }
            }

            if (element is IHasTitle title)
            {
                Title = title.Title;
            }

            Focus();
        }

        /// <summary>
        /// Takes the content of the window and resizes the window
        /// to the detail form elements. 
        /// </summary>
        public void SwitchToMinimumSize()
        {
            var control = MainContent.Content as DetailFormControl;
            if (control?.EffectiveForm == null)
            {
                return;
            }

            var width = control.EffectiveForm.getOrDefault<double>(_FormAndFields._Form.defaultWidth);
            var height = control.EffectiveForm.getOrDefault<double>(_FormAndFields._Form.defaultWidth);
            if (width <= 0 && height <= 0)
            {
                width = 1000;
                height = 1000;
            }
            
            MainRibbon.Measure(new Size(width,height));
            var heightOffset = MainRibbon.DesiredSize.Height;

            control.Measure(new Size(width, height - heightOffset));
            Width = Math.Ceiling(control.DesiredSize.Width) + 50;
            Height = Math.Ceiling(control.DesiredSize.Height) + 50 + heightOffset;

            Logger.Trace(
                $"Size: {Height}: {control.DesiredSize.Height}(ctrl) + {MainRibbon.DesiredSize.Height}");
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void OnSaved(IObject detailElement, IObject attachedElement)
        {
            Saved?.Invoke(this, new ItemEventArgs(
                detailElement, 
                attachedElement));
        }
    }
}