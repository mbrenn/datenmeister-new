using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

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

        public string ZipCodeUri => ZipCode?.GetUri();
    }
}