#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using BurnSystems.Logging;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;

namespace DatenMeister.WPF.Windows
{
    public class MenuHelper : NavigationExtensionHelper
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(MenuHelper));

        private readonly Menu _menu;

        private readonly List<MenuHelperItem> _buttons = new List<MenuHelperItem>();

        private class MenuHelperItem
        {
            public NavigationButtonDefinition Definition { get; set; }

            public MenuItem Button { get; set; }

            public RoutedEventHandler ClickEvent { get; set; }

            /// <summary>
            /// Converts the item to a string
            /// </summary>
            /// <returns></returns>
            public override string ToString() => $"HelperItem: {Definition}";

            public MenuHelperItem(NavigationButtonDefinition definition, MenuItem button, RoutedEventHandler clickEvent)
            {
                Definition = definition;
                Button = button;
                ClickEvent = clickEvent;
            }
        }

        public MenuHelper(Menu menu, NavigationScope navigationScope) : base(navigationScope)
        {
            _menu = menu;
        }

        /// <summary>
        /// Adds a navigational element to the ribbons
        /// </summary>
        /// <param name="definition">The definition to be used</param>
        private void AddNavigationButton(NavigationButtonDefinition definition)
        {
            // Ok, we have not found it, so create the button
            var name = definition.Name;
            var categoryName = definition.CategoryName;
            var imageName = definition.ImageName;

            var clickMethod = CreateClickMethod(definition);
            if (clickMethod == null)
            {
                // The method is not valid => no addition to the menu
                return;
            }

            string tabName;
            string? groupName;
            var indexOfSemicolon = categoryName.IndexOf('.');
            if (indexOfSemicolon == -1)
            {
                tabName = categoryName;
                groupName = null;
            }
            else
            {
                tabName = categoryName.Substring(0, indexOfSemicolon);
                groupName = categoryName.Substring(indexOfSemicolon + 1);
            }

            // If the main window is the datenmeister, then skip
            // this navigational element because the standard menu does not contain
            // sufficient hierarchical depth
            /*if (tabName == NavigationCategories.DatenMeister)
            {
                tabName = groupName;
                groupName = null;
            }*/

            var tab = _menu.Items.OfType<MenuItem>().FirstOrDefault(x => x.Header?.ToString() == tabName);
            if (tab == null)
            {
                tab = new MenuItem
                {
                    Header = tabName
                };

                _menu.Items.Add(tab);
            }

            MenuItem? group;
            if (!string.IsNullOrEmpty(groupName))
            {
                group = tab.Items.OfType<MenuItem>().FirstOrDefault(x => x.Header.ToString() == groupName);
                if (group == null)
                {
                    group = new MenuItem
                    {
                        Header = groupName
                    };
                    tab.Items.Add(group);
                }
            }
            else
            {
                group = tab;
            }

            var button = new MenuItem
            {
                Header = name,
                IsEnabled = definition.IsEnabled
            };

            var item = new MenuHelperItem(
                definition,
                button, 
                (x, y) => { clickMethod(); });
            _buttons.Add(item);
            button.Click += item.ClickEvent;

            // Check correct position for button... First, the buttons are shown, then the texts
            if (imageName != null)
            {
                for (var n = 0; n < group.Items.Count; n++)
                {
                    if (group.Items[n] is RibbonButton existingButton
                        && existingButton.LargeImageSource == null)
                    {
                        // Ok, we have a button, but the existing one does not have, so add the button at the given position
                        group.Items.Insert(n, button);
                        return;
                    }
                }
            }

            group.Items.Add(button);
        }

        private void ClearNavigationButtons()
        {
            _menu.Items.Clear();
            _buttons.Clear();
        }

        public void EvaluateExtensions(IEnumerable<ViewExtension> viewExtensions)
        {
            ClearNavigationButtons();
            var copiedList = _buttons.ToList();

            foreach (var viewExtension in viewExtensions.OfType<NavigationButtonDefinition>().OrderByDescending(x => x.Priority))
            {
                // Check, navigation button is already given
                var foundTuple = _buttons.Find(x => NavigationButtonDefinition.AreEqual(viewExtension, x.Definition));
                if (foundTuple != null)
                {
                    copiedList.Remove(foundTuple);

                    // Reorganizes the buttons
                    foundTuple.Button.Click -= foundTuple.ClickEvent;
                    var clickMethod = CreateClickMethod(viewExtension);
                    if (clickMethod == null)
                    {
                        Logger.Error($"No further action defined anymore for item{viewExtension.Name}");
                        continue;
                    }

                    foundTuple.ClickEvent = (x, y) => clickMethod();
                    foundTuple.Button.Click += foundTuple.ClickEvent;
                }
                else
                {
                    AddNavigationButton(viewExtension);
                }
            }
        }

        public static MenuItem GetOrCreateMenu(List<MenuItem> menuItems, string category, string text)
        {
            if (string.IsNullOrEmpty(category))
            {
                var result = new MenuItem
                {
                    Header = text
                };
                
                menuItems.Add(result);

                return result;
            }

            var menu = GetOrCreateMenuForCategory(menuItems, category);
            
            var innerResult = new MenuItem
            {
                Header = text
            };

            menu.Items.Add(innerResult);
            return innerResult;
        }

        public static MenuItem GetOrCreateMenuForCategory(IList<MenuItem> menuItems, string category)
        {
            foreach (var menuItem in menuItems
                .Where(menuItem => menuItem.Header.ToString() == category))
            {
                return menuItem;
            }

            var newMenuItem = new MenuItem {Header = category};
            menuItems.Add(newMenuItem);
            return newMenuItem;
        }
    }
}