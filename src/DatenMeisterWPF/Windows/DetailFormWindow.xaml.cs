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
            RibbonHelper.ClearRibbons();
            RibbonHelper.PrepareDefaultNavigation();
            RibbonHelper.FinalizeRibbons();
        }

        public IControlNavigation NavigateTo(Func<UserControl> factoryMethod, NavigationMode navigationMode)
        {
            return Navigator.NavigateByCreatingAWindow(this, factoryMethod);
        }

        /// <summary>
        /// Adds a navigational element to the ribbons
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <param name="clickMethod">Method, that shall be called, when the user clicks on the item</param>
        /// <param name="imageName">Name of the image being allocated</param>
        /// <param name="categoryName">Category of the MainRibbon to be added</param>
        public void AddNavigationButton(string name, Action clickMethod, string imageName, string categoryName)
        {
            RibbonHelper.AddNavigationButton(name, clickMethod, imageName, categoryName);
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
        }

        public void SetFocus()
        {
            Focus();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
