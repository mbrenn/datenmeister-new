using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using BurnSystems.Logging;
using DatenMeister.WPF.Forms.Base.ViewExtensions;

namespace DatenMeister.WPF.Windows
{
    public class MenuHelper
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(MenuHelper));

        private readonly Menu _menu;

        private readonly List<MenuHelperItem> _buttons =
            new List<MenuHelperItem>();

        private class MenuHelperItem
        {
            public RibbonButtonDefinition Definition { get; set; }

            public MenuItem Button { get; set; }

            public RoutedEventHandler ClickEvent { get; set; }

            /// <summary>
            /// Converts the item to a string
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"HelperItem: {Definition}";
            }
        }

        public MenuHelper(Menu menu)
        {
            _menu = menu;
        }

        /// <summary>
        /// Adds a navigational element to the ribbons
        /// </summary>
        /// <param name="definition">The definition to be used</param>
        private void AddNavigationButton(RibbonButtonDefinition definition)
        {
            // Ok, we have not found it, so create the button
            var name = definition.Name;
            var categoryName = definition.CategoryName;
            var imageName = definition.ImageName;
            var clickMethod = definition.OnPressed;

            string tabName, groupName;
            var indexOfSemicolon = categoryName.IndexOf('.');
            if (indexOfSemicolon == -1)
            {
                tabName = categoryName;
                groupName = "Standard";
            }
            else
            {
                tabName = categoryName.Substring(0, indexOfSemicolon);
                groupName = categoryName.Substring(indexOfSemicolon + 1);
            }

            var tab = _menu.Items.OfType<MenuItem>().FirstOrDefault(x => x.Header?.ToString() == tabName);
            if (tab == null)
            {
                tab = new MenuItem
                {
                    Header = tabName
                };

                _menu.Items.Add(tab);
            }
            
            var group = tab.Items.OfType<MenuItem>().FirstOrDefault(x => x.Header.ToString() == groupName);
            if (@group == null)
            {
                @group = new MenuItem
                {
                    Header = groupName
                };
                tab.Items.Add(@group);
            }

            var button = new MenuItem
            {
                Header = name
            };

            var item = new MenuHelperItem
            {
                Definition = definition,
                Button = button,
                ClickEvent = (x, y) =>
                {
                    if (clickMethod == null)
                    {
                        Logger.Error("No method defined which is called after a click");
                    }
                    else
                    {
                        clickMethod();
                    }
                }
            };
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

            @group.Items.Add(button);
        }

        public void EvaluateExtensions(IEnumerable<ViewExtension> viewExtensions)
        {
            var copiedList = _buttons.ToList();

            foreach (var viewExtension in viewExtensions.OfType<RibbonButtonDefinition>())
            {
                // Check, navigation button is already given
                var foundTuple = _buttons.Find(x => RibbonButtonDefinition.AreEqual(viewExtension, x.Definition));
                if (foundTuple != null)
                {
                    copiedList.Remove(foundTuple);

                    // Reorganizes the buttons
                    foundTuple.Button.Click -= foundTuple.ClickEvent;
                    foundTuple.ClickEvent = (x, y) => viewExtension.OnPressed();
                    foundTuple.Button.Click += foundTuple.ClickEvent;
                }
                else
                {
                    AddNavigationButton(viewExtension);
                }
            }

            // Now, remove the buttons that are not needed anymore
            foreach (var obsolete in copiedList)
            {
                var group = (RibbonGroup) obsolete.Button.Parent;
                group.Items.Remove(obsolete.Button);
                _buttons.Remove(obsolete);

                // Removes the groups and tabs if necessary
                if (group.Items.Count == 0)
                {
                    var tab = (RibbonTab) group.Parent;
                    tab.Items.Remove(group);

                    if (tab.Items.Count == 0)
                    {
                        ((Ribbon) tab.Parent).Items.Remove(tab);
                    }
                }
            }
        }
    }
}