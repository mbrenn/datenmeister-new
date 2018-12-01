using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using DatenMeister.Core.EMOF.Interface.Reflection;
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
                Focus();
            else
                MainContent?.Focus();
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
                if (control.IsDesignMinimized()) MainRibbon.IsMinimized = true;

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
            }

            Focus();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void OnSaved(IObject detailElement)
        {
            Saved?.Invoke(this, new ItemEventArgs(detailElement));
        }
    }
}