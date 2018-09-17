using System;
using System.Collections.Generic;

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

            if (CategoryName != null)
            {
                result ^= CategoryName.GetHashCode();
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