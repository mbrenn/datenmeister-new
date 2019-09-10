using System;
using System.Collections.Generic;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions
{
    /// <summary>
    /// Defines the ribbon
    /// </summary>
    public abstract class RibbonButtonDefinition : ViewExtension
    {
        /// <summary>
        /// Initializes a new instance of the RibbonButtonDefinition class
        /// </summary>
        /// <param name="name">Name of the button being shown</param>
        /// <param name="imageName">Name of the image to be shown</param>
        /// <param name="categoryName">Name of the category being used</param>
        /// <param name="priority">Index being used to provide the correct sort order.
        /// The higher the number, the higher the priority</param>
        public RibbonButtonDefinition(string name, string imageName, string categoryName, int priority = 0)
        {
            Name = name;
            // OnPressed = onPressed;
            ImageName = imageName;
            CategoryName = categoryName;
            Priority = Math.Min(priority, 65535) * 65536 + 
                    name.GetHashCode() % 65535;
        }

        /// <summary>
        /// Gets or sets the index of the ribbon definition which is used to order the items.
        /// The higher the number, the higher the priority
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Gets the name of the ribbon
        /// </summary>
        public string Name { get; }

        /*///<summary>
        /// Gets the action being executed when the user clicked upon the button
        /// </summary>
        public Action OnPressed { get; }*/

        /// <summary>
        /// Gets the name of the image
        /// </summary>
        public string ImageName { get; }

        /// <summary>
        /// Gets the name of category
        /// </summary>
        public string CategoryName { get; private set; }

        public bool IsTopCategoryFixed { get; set; }

        public override string ToString()
        {
            return $"{CategoryName}.{Name} [{ImageName}]";
        }

        /// <summary>
        /// Sets the top category name by overwriting the
        /// first part of the <c>CategoryName</c>. If the Category name
        /// was already fixed, then no overwriting will be performed
        /// </summary>
        /// <param name="topCategory">Name of the category</param>
        public void FixTopCategoryIfNotFixed(string topCategory)
        {
            if (IsTopCategoryFixed)
            {
                return;
            }

            IsTopCategoryFixed = true;

            var indexPositionDot = CategoryName.IndexOf('.');
            if (indexPositionDot == -1)
            {
                CategoryName = topCategory;
            }
            else
            {
                CategoryName = topCategory + CategoryName.Substring(indexPositionDot);
            }
        }
        
        /// <summary>
        /// Verifies whether two ribbon button definitions can be regarded as absolutely equal
        /// </summary>
        /// <param name="first">First parameter to be evaluated</param>
        /// <param name="second">Second parameter to be evaluated</param>
        /// <returns>true, if both values are equal</returns>
        public static bool AreEqual(RibbonButtonDefinition first, RibbonButtonDefinition second)
        {
            if (first == second)
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            return first.Name == second.Name &&
                   first.CategoryName == second.CategoryName &&
                   first.ImageName == second.ImageName;
        }

        /// <summary>
        /// Gets the hashcode of the given instance by querying the values
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var result = 0;
            if (Name != null)
            {
                result ^= Name.GetHashCode();
            }

            if (ImageName != null)
            {
                result ^= ImageName.GetHashCode();
            }

            return result;
        }

        /// <summary>
        /// Gets the equality comparer
        /// </summary>
        public class Comparer : IEqualityComparer<RibbonButtonDefinition>
        {
            public bool Equals(RibbonButtonDefinition x, RibbonButtonDefinition y)
            {
                return RibbonButtonDefinition.AreEqual(x, y);
            }

            public int GetHashCode(RibbonButtonDefinition obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}