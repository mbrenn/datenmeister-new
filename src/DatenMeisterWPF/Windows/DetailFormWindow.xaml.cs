using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für DetailFormWindow.xaml
    /// </summary>
    public partial class DetailFormWindow : Window, IHasRibbon, INavigationHost
    {
        /// <summary>
        /// Gets the ribbon
        /// </summary>
        /// <returns></returns>
        public Ribbon GetRibbon() => MainRibbon;

        private RibbonHelper RibbonHelper { get; set; }

        public DetailFormWindow()
        {
            InitializeComponent();
            RibbonHelper = new RibbonHelper(this);
        }

        private void DetailFormWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            RebuildNavigation();
        }

        public IControlNavigation NavigateTo(Func<UserControl> factoryMethod, NavigationMode navigationMode)
        {
            return Navigator.NavigateByCreatingAWindow(this, factoryMethod, navigationMode);
        }

        /// <summary>
        /// Sets the main content to be shown
        /// </summary>
        /// <param name="element">Element to be shown</param>
        public void SetMainContent(UIElement element)
        {
            this.MainContent.Content = element;
            if (element is DetailFormControl control)
            {
                if (control.IsDesignMinimized())
                {
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
            }

            Focus();
        }


        /// <summary>
        /// Rebuild the complete navigation
        /// </summary>
        public void RebuildNavigation()
        {
            RibbonHelper.EvaluateExtensions(RibbonHelper.GetDefaultNavigation());
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

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
