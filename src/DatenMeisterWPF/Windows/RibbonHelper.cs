using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Ribbon;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Windows
{
    public class RibbonHelper
    {
        private readonly IHasRibbon _mainWindow;

        /// <summary>
        /// Stores the icon repository
        /// </summary>
        private static IIconRepository IconRepository { get; set; }

        private readonly List<RibbonTab> _ribbonTabs = new List<RibbonTab>();

        private List<RibbonHelperItem> _buttons =
            new List<RibbonHelperItem>();

        private class RibbonHelperItem
        {
            public RibbonButtonDefinition Definition { get; set; }

            public RibbonButton Button { get; set; }

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

        public RibbonHelper(IHasRibbon mainWindow)
        {
            _mainWindow = mainWindow;
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

            var tab = _ribbonTabs.FirstOrDefault(x => x.Header?.ToString() == tabName);
            if (tab == null)
            {
                tab = new RibbonTab
                {
                    Header = tabName
                };

                _ribbonTabs.Add(tab);
                _mainWindow.GetRibbon().Items.Add(tab);
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
                LargeImageSource = string.IsNullOrEmpty(imageName) ? null : IconRepository.GetIcon(imageName)
            };

            var item = new RibbonHelperItem
            {
                Definition = definition,
                Button = button,
                ClickEvent = (x, y) => clickMethod()
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

        /// <summary>
        /// Clears the complete MainRibbon navigation
        /// </summary>
        private void ClearRibbons()
        {
            _ribbonTabs.Clear();
            _mainWindow.GetRibbon().Items.Clear();
        }

        /// <summary>
        /// Prepares the default navigation
        /// </summary>
        public IEnumerable<ViewExtension> GetDefaultNavigation()
        {
            return new [] {
                new RibbonButtonDefinition(
                    "Close", 
                    () => (_mainWindow as Window)?.Close(),
                    "file-exit",
                    NavigationCategories.File),
                new RibbonButtonDefinition("About",
                    () => new AboutDialog
                    {
                        Owner = _mainWindow as Window
                    }.ShowDialog(),
                    "file-about",
                    NavigationCategories.File)
        };
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
                    return;
                }

                AddNavigationButton(viewExtension);
            }

            // Now, remove the buttons that are not needed anymore
            foreach (var obsolete in copiedList)
            {
                ((RibbonGroup) obsolete.Button.Parent).Items.Remove(obsolete.Button);
            }
        }
    }
}