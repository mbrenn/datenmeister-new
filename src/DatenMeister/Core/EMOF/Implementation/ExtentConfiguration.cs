using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation.AutoEnumerate;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

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
        private const string DatenmeisterDefaultTypePackage = "__DatenMeister.DefaultTypePackage";

        /// <summary>
        /// Saves the type of the extent
        /// </summary>
        private const string ExtentTypeProperty = "__ExtentType";
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
        public string ExtentType
        {
            get => _extent?.getOrDefault<string>(ExtentTypeProperty) ?? string.Empty;
            set => _extent.set(ExtentTypeProperty, value);
        }

        /// <summary>
        /// Sets the default type package which is shown, when the user wants
        /// to create a new item
        /// </summary>
        /// <param name="extent">Extent shall get the default type package</param>
        /// <param name="defaultTypePackages">The elements which shall be considered as the
        /// default type package</param>
        public void SetDefaultTypePackages(IEnumerable<IElement> defaultTypePackages)
        {
            _extent.set(
                DatenmeisterDefaultTypePackage,
                defaultTypePackages);
        }

        public void AddDefaultTypePackages(IEnumerable<IElement> defaultTypePackages)
        {
            var found = GetDefaultTypePackages()?.ToList() ?? new List<IElement>();
            foreach (var newPackage in defaultTypePackages.Where(newPackage => !found.Contains(newPackage)))
            {
                found.Add(newPackage);
            }

            SetDefaultTypePackages(found);
        }

        /// <summary>
        /// Gets the default type package
        /// </summary>
        /// <param name="extent">Extent to be used</param>
        /// <returns>The found element</returns>
        public IEnumerable<IElement> GetDefaultTypePackages() =>
            _extent.get<IReflectiveCollection>(DatenmeisterDefaultTypePackage).OfType<IElement>();
    }
}