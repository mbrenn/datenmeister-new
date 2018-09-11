using System;

namespace DatenMeisterWPF.Forms.Base.ViewExtensions
{
    /// <summary>
    /// Defines the ribbon
    /// </summary>
    public class RibbonButtonDefinition : ViewExtension
    {
        public RibbonButtonDefinition(string name, Action onPressed, string imageName, string categoryName)
        {
            Name = name;
            OnPressed = onPressed;
            ImageName = imageName;
            CategoryName = categoryName;
        }

        /// <summary>
        /// Gets the name of the ribbon
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the action being executed when the user clicked upon the button
        /// </summary>
        public Action OnPressed { get; }

        /// <summary>
        /// Gets the name of the image
        /// </summary>
        public string ImageName { get; }

        /// <summary>
        /// Gets the name of category
        /// </summary>
        public string CategoryName { get; }

        public override string ToString()
        {
            return $"RibbonButtonDefinition: {Name}";
        }
    }
}