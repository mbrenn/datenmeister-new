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

        private Dictionary<RibbonButtonDefinition, RibbonButton> _buttons =
            new Dictionary<RibbonButtonDefinition, RibbonButton>(
                new RibbonButtonDefinition.Comparer());

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
        public void AddNavigationButton(RibbonButtonDefinition definition)
        {
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

            _buttons[definition] = button;

            button.Click += (x, y) => clickMethod();

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
        /// Clears the complete MainRibbon navigaton
        /// </summary>
        public void ClearRibbons()
        {
            _ribbonTabs.Clear();
            _mainWindow.GetRibbon().Items.Clear();
        }

        /// <summary>
        /// After having received the MainRibbon requests, this method builds up the real navigation
        /// </summary>
        public void FinalizeRibbons()
        {
            AddNavigationButton(
                new RibbonButtonDefinition("About",
                    () => new AboutDialog
                    {
                        Owner = _mainWindow as Window
                    }.ShowDialog(),
                    "file-about",
                    NavigationCategories.File));
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
                    NavigationCategories.File)
            };
        }

        public void EvaluateExtensions(IEnumerable<ViewExtension> viewExtensions)
        {
            ClearRibbons();

            foreach (var viewExtension in viewExtensions.OfType<RibbonButtonDefinition>())
            {
                AddNavigationButton(viewExtension);
            }

            FinalizeRibbons();
        }
    }
}