#nullable enable
using System;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// This helper stores the possible scopes for which a ribbon or a
    /// menu shall be created
    /// </summary>
    public class NavigationExtensionHelper
    {
        public NavigationExtensionHelper(NavigationScope navigationScope)
        {
            NavigationScope = navigationScope;
        }

        /// <summary>
        /// Defines the menuscope
        /// </summary>
        public NavigationScope NavigationScope
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the extent to which the menu is associated. This extent is given to the
        /// action which is called
        /// </summary>
        public IExtent? Extent { get; set; }

        /// <summary>
        /// Gets the collection to which the menu is associated. This extent is given to the
        /// action which is called
        /// </summary>
        public IReflectiveCollection? Collection { get; set; }

        /// <summary>
        /// Gets the item to which the menu is associated. This extent is given to the
        /// action which is called
        /// </summary>
        public IObject? Item { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application items shall be shown
        /// </summary>
        public bool ShowApplicationItems { get; set; } = true;

        protected Action? CreateClickMethod(NavigationButtonDefinition definition)
        {
            // Defines the clickmethod by the definition
            if (ShowApplicationItems
                && definition is ApplicationMenuButtonDefinition applicationMenuButtonDefinition
                && NavigationScope.HasFlag(NavigationScope.Application))
            {
                return applicationMenuButtonDefinition.OnPressed;
            }

            if (Extent != null
                && definition is ExtentMenuButtonDefinition extentMenuButtonDefinition
                && NavigationScope.HasFlag(NavigationScope.Extent))
            {
                return () => extentMenuButtonDefinition.OnPressed(Extent);
            }

            if (Collection != null
                && definition is CollectionMenuButtonDefinition collectionMenuButtonDefinition
                && NavigationScope.HasFlag(NavigationScope.Collection))
            {
                return () => collectionMenuButtonDefinition.OnPressed(Collection);
            }

            if (Item != null
                && definition is ItemMenuButtonDefinition itemMenuButtonDefinition
                && NavigationScope.HasFlag(NavigationScope.Item))
            {
                return () => itemMenuButtonDefinition.OnPressed(Item);
            }

            //throw new InvalidOperationException("No click method could be generated");

            return null;
        }
    }
}