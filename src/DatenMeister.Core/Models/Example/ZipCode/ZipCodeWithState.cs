// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DatenMeister.Models.Example.ZipCode
{
    /// <summary>
    /// Just a demo for the inheritance tests
    /// </summary>
    public class ZipCodeWithState : ZipCode
    {
        /// <summary>
        /// State to which the zipcode belongs
        /// </summary>
        public string? state { get; set; }
    }
}