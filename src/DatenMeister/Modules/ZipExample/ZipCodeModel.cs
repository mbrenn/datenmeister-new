using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.ZipExample
{
    public class ZipCodeModel
    {
        /// <summary>
        /// Stores the path for the packages
        /// </summary>
        public const string PackagePath = "Apps::ZipCode";

        /// <summary>
        /// Gets or sets the type for the zipcode
        /// </summary>
        public IElement ZipCode { get; set; }
    }
}