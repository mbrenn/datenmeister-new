using System.Collections.Specialized;

namespace DatenMeister.Core.EMOF.Implementation.Hooks
{
    public record ResolveHookParameters
    {
        public ResolveHookParameters(NameValueCollection queryString, object? currentItem, MofUriExtent extent)
        {
            QueryString = queryString;
            CurrentItem = currentItem;
            Extent = extent;
        }

        /// <summary>
        ///     Stores the value of the parameters (&para=324234)
        /// </summary>
        public NameValueCollection QueryString { get; set; }

        /// <summary>
        ///     Gets or sets the item that has been evaluated up to now
        /// </summary>
        public object? CurrentItem { get; set; }

        /// <summary>
        ///     Stores the extent being connected to the
        /// </summary>
        public MofUriExtent Extent { get; set; }
    }
}