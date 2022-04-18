using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation.AutoEnumerate;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines the configuration of the extent being used to reflect MOF and UML properties
    /// </summary>
    public class ExtentConfiguration
    {
        /// <summary>
        /// Defines the name of the default type packages
        /// </summary>
        public const string ExtentDefaultTypes = "__DatenMeister.DefaultTypes";

        /// <summary>
        /// Saves the type of the extent
        /// </summary>
        public const string ExtentTypeProperty = "__ExtentType";

        /// <summary>
        /// Saves the type of the extent
        /// </summary>
        public const string NameProperty = "__Name";

        /// <summary>
        /// Stores the extent
        /// </summary>
        private readonly MofExtent _extent;

        /// <summary>
        /// Initializes a new instance of the extent
        /// </summary>
        /// <param name="extent"></param>
        public ExtentConfiguration(MofExtent extent)
        {
            _extent = extent;
        }

        /// <summary>
        /// Defines the type of the enumeration being used for properties with 'IsID == true'
        /// </summary>
        public AutoEnumerateType AutoEnumerateType
        {
            get => _extent.getOrDefault<AutoEnumerateType>(AutoEnumerateHandler.AutoEnumerateTypeProperty);
            set => _extent.set(AutoEnumerateHandler.AutoEnumerateTypeProperty, value);
        }

        /// <summary>
        /// Gets the extent type
        /// </summary>
        public string Name
        {
            get => _extent.getOrDefault<string>(NameProperty) ?? string.Empty;
            set => _extent.set(NameProperty, value);
        }

        /// <summary>
        /// Gets the extent type
        /// </summary>
        public string ExtentType
        {
            get => _extent.getOrDefault<string>(ExtentTypeProperty) ?? string.Empty;
            set => _extent.set(ExtentTypeProperty, value);
        }

        /// <summary>
        /// Gets or sets the extent types
        /// </summary>
        public IEnumerable<string> ExtentTypes
        {
            get => ExtentType.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            set => ExtentType = value.Aggregate((x, y) => $"{x} {y}");
        }

        /// <summary>
        /// Checks whether the extent's types contains at least the given extent type
        /// </summary>
        /// <param name="extentType">Type of the extent being queried</param>
        /// <returns>true, if the extent's type contains the given extent type</returns>
        public bool ContainsExtentType(string extentType)
            => ExtentType.Contains(extentType);

        /// <summary>
        /// Sets the default type package which is shown, when the user wants
        /// to create a new item
        /// </summary>
        /// <param name="defaultTypes">The elements which shall be considered as the
        /// default type package</param>
        public void SetDefaultTypes(IEnumerable<IElement> defaultTypes)
        {
            _extent.set(
                ExtentDefaultTypes,
                defaultTypes);
        }

        public void AddDefaultTypes(IEnumerable<IElement> defaultTypes)
        {
            var found = GetDefaultTypes().ToList() ?? new List<IElement>();
            foreach (var newPackage in defaultTypes)
            {
                if (found.Any(x => MofObject.AreEqual(x, newPackage)))
                {
                    continue;
                }

                found.Add(newPackage);
            }

            SetDefaultTypes(found);
        }

        /// <summary>
        /// Gets the default type package
        /// </summary>
        /// <returns>The found element</returns>
        public IEnumerable<IElement> GetDefaultTypes()
        {
            return _extent.get<IReflectiveCollection>(ExtentDefaultTypes).OfType<IElement>();
        }
    }
}