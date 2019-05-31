﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für ListFormWindow.xaml
    /// </summary>
    public partial class ListFormWindow : Window, INavigationHost
    {
        /// <summary>
        /// Gets or sets the navigation host
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Initializes a new instance of ListFormWindow class.
        /// </summary>
        public ListFormWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigates to a new window
        /// </summary>
        /// <param name="factoryMethod">Factory method being used</param>
        /// <param name="navigationMode">Navigation mode</param>
        /// <returns></returns>
        public Task<NavigateToElementDetailResult> NavigateTo(Func<UserControl> factoryMethod, NavigationMode navigationMode)
        {
            return Navigator.NavigateByCreatingAWindow(this, factoryMethod, navigationMode);
        }

        public void AddNavigationButton(string name, Action clickMethod, string imageName, string categoryName)
        {
            throw new NotImplementedException();
        }

        public void SetFocus()
        {
            throw new NotImplementedException();
        }

        public void RebuildNavigation()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Window GetWindow()
        {
            return this;
        }
    }
}