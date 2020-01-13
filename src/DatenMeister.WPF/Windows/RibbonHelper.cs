#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Ribbon;
using BurnSystems.Logging;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Windows
{
    public class RibbonHelper : NavigationExtensionHelper
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(RibbonHelper));

        private readonly IHasRibbon _mainWindow;

        /// <summary>
        /// Stores the icon repository
        /// </summary>
        private static IIconRepository? IconRepository { get; set; }

        private readonly List<RibbonHelperItem> _buttons = new List<RibbonHelperItem>();

        private class RibbonHelperItem
        {
            public NavigationButtonDefinition Definition { get; set; }

            public RibbonButton Button { get; set; }

            public RoutedEventHandler? ClickEvent { get; set; }


            public RibbonHelperItem(NavigationButtonDefinition definition, RibbonButton button, RoutedEventHandler clickEvent)
            {
                Definition = definition;
                Button = button;
                ClickEvent = clickEvent;
            }
            /// <summary>
            /// Converts the item to a string
            /// </summary>
            /// <returns></returns>
            public override string ToString()
                => $"HelperItem: {Definition}";
        }

        /// <summary>
        /// Loads the icon repository.
        /// If DatenMeister.Icons is existing, then the full and cool icons will be used.
        /// </summary>
        public void LoadIconRepository()
        {
            if (IconRepository == null)
            {
                if (File.Exists("DatenMeister.Icons.dll"))
                {
                    var dllPath = Path.Combine(Environment.CurrentDirectory, "DatenMeister.Icons.dll");
                    var assembly = Assembly.LoadFile(dllPath);

                    var type = assembly.GetType("DatenMeister.Icons.NiceIconsRepository");
                    IconRepository = Activator.CreateInstance(type) as IIconRepository;
                }

                if (IconRepository == null)
                {
                    IconRepository = new StandardRepository();
                }
            }
        }

        public RibbonHelper(IHasRibbon mainWindow, NavigationScope navigationScope) : base(navigationScope)
        {
            _mainWindow = mainWindow;
        }

        /// <summary>
        /// Adds a navigational element to the ribbons
        /// </summary>
        /// <param name="definition">The definition to be used</param>
        private void AddNavigationButton(NavigationButtonDefinition definition)
        {
            if ( IconRepository == null )
                throw new InvalidOperationException("IconRepository not loaded");
            
            // Ok, we have not found it, so create the button
            var name = definition.Name;
            var categoryName = definition.CategoryName;
            var imageName = definition.ImageName;

            var clickMethod = CreateClickMethod(definition);
            if (clickMethod == null)
            {
                return;
            }

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

            var tab = _mainWindow.GetRibbon().Items.OfType<RibbonTab>().FirstOrDefault(x => x.Header?.ToString() == tabName);
            if (tab == null)
            {
                tab = new RibbonTab
                {
                    Header = tabName
                };

                // Find perfect position
                var posFound = Array.IndexOf(NavigationCategories.RibbonOrder, tabName);
                var ribbons = _mainWindow.GetRibbon();
                if (posFound == -1)
                {
                    ribbons.Items.Add(tab);
                }
                else
                {
                    // Find the correct position for the tab according to the ribbonorder
                    for (var n = 0; n <= posFound; n++)
                    {
                        if (ribbons.Items.Count <= n)
                        {
                            ribbons.Items.Add(tab);
                            break;
                        }

                        // Gets the value of the item
                        var posInIndex =
                            Array.IndexOf(
                                NavigationCategories.RibbonOrder,
                                ((RibbonTab) ribbons.Items[n]).Header.ToString());
                        if (posInIndex == -1)
                        {
                            posInIndex = int.MaxValue;
                        }

                        if (posInIndex > posFound || n == posFound)
                        {
                            ribbons.Items.Insert(n, tab);
                            break;
                        }
                    }
                }
            }

            var group = tab.Items.OfType<RibbonGroup>().FirstOrDefault(x => x.Header.ToString() == groupName);
            if (@group == null)
            {
                @group = new RibbonGroup
                {
                    Header = groupName
                };
                tab.Items.Add(@group);
            }

            var button = new RibbonButton
            {
                Label = name,
                LargeImageSource = string.IsNullOrEmpty(imageName) ? null : IconRepository.GetIcon(imageName),
                IsEnabled = definition.IsEnabled
            };

            var item = new RibbonHelperItem(
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

            @group.Items.Add(button);
        }

        /// <summary>
        /// Evaluates the viewExtensions to create the navigation buttons
        /// </summary>
        /// <param name="viewExtensions">Enumeration of view extensions</param>
        public void EvaluateExtensions(IEnumerable<ViewExtension> viewExtensions)
        {
            var copiedList = _buttons.ToList();

            foreach (var viewExtension in viewExtensions.OfType<NavigationButtonDefinition>()
                .OrderByDescending(x => x.Priority))
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
                    foundTuple.Button.IsEnabled = viewExtension.IsEnabled;
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