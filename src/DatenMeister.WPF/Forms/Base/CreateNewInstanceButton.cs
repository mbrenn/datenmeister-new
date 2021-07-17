using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    /// Defines a button which creates a context menu when the user clicks the button.
    /// The context menu lists all derived types
    /// </summary>
    public class CreateNewInstanceButton : Button
    {
        private IEnumerable<IElement>? _typesForCreation;

        public CreateNewInstanceButton()
        {
            Initialized += NewTypeButton_Initialized;
            Content = "New Item...";
        }

        private void NewTypeButton_Initialized(object? sender, EventArgs e)
        {
            Click += NewTypeButton_Click;
        }

        /// <summary>
        /// Sets the types that will be selected for the creation
        /// </summary>
        /// <param name="types">Types to be selected</param>
        public void SetDefaultTypesForCreation(IEnumerable<IElement>? types)
        {
            _typesForCreation = types ?? new List<IElement>();
        }

        /// <summary>
        /// Sets the types that will be selected for the creation
        /// </summary>
        /// <param name="types">Types to be selected</param>
        public void SetDefaultTypeForCreation(IElement? types)
        {
            if (types == null)
            {
                _typesForCreation = new List<IElement>();
            }
            else
            {
                _typesForCreation = new []{types};
            }
        }

        private void NewTypeButton_Click(object sender, RoutedEventArgs e)
        {
            _ = new ContextMenu {ItemsSource = GetMenuItems(), IsOpen = true};
        }

        /// <summary>
        /// This event is called when the user has selected a certain type
        /// </summary>
        public event EventHandler<CreateNewInstanceButtonEventArgs>? TypeSelected;

        /// <summary>
        /// This method gets called when the users selects one type
        /// </summary>
        /// <param name="selectedType"></param>
        protected void OnTypeSelected(IElement? selectedType)
        {
            TypeSelected?.Invoke(this, new CreateNewInstanceButtonEventArgs(selectedType));
        }

        /// <summary>
        /// Creates the menu and the buttons for the default types
        /// </summary>
        /// <returns>List of menu items being used as context menu</returns>
        private List<MenuItem> GetMenuItems()
        {

            // Stores the menu items for the context menu
            var menuItems = new List<MenuItem>();
            var firstMenuItem = new MenuItem
            {
                Header = "Select Type..."
            };

            firstMenuItem.Click += (x, y) => OnTypeSelected(null);

            var alreadyIncludedElements = new HashSet<IElement>();

            if (_typesForCreation != null)
            {
                // Sets the generic buttons to create the new types
                foreach (var element in _typesForCreation)
                {
                    IElement? type = null;
                    if (element.metaclass?.equals(
                        _DatenMeister.TheOne.Forms.__DefaultTypeForNewElement) == true)
                    {
                        var newType =
                            element.getOrDefault<IElement>(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass);
                        /*var tempParentProperty =
                            element.getOrDefault<string>(_DatenMeister._Forms._DefaultTypeForNewElement.parentProperty)
                            ?? parentProperty;*/

                        if (newType != null)
                        {
                            type = newType;
                        }
                    }
                    else
                    {
                        type = element;
                    }

                    if (type != null)
                    {
                        foreach (var newSpecializationType in ClassifierMethods.GetSpecializations(type,
                            alreadyIncludedElements))
                        {
                            // Stores the menu items for the context menu
                            var menuItem = new MenuItem {Header = $"New {newSpecializationType}"};

                            menuItem.Click += (x, y) => OnTypeSelected(newSpecializationType);
                            menuItems.Add(menuItem);
                        }
                    }
                }
            }

            menuItems = menuItems.OrderBy(x => x.Header.ToString()).ToList();
            menuItems.Insert(0, firstMenuItem);

            return menuItems;
        }
    }
}
