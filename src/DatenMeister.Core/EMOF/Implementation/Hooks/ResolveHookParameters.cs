using System.Collections.Specialized;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core.EMOF.Implementation.Hooks
{
    public class ResolveHookParameters
    {
        public ResolveHookParameters(IScopeStorage? scopeStorage, NameValueCollection queryString, object? currentItem,
            IUriExtent extent)
        {
            ScopeStorage = scopeStorage;
            QueryString = queryString;
            CurrentItem = currentItem;
            Extent = extent;
        }

        public IScopeStorage? ScopeStorage { get; }

        /// <summary>
        ///     Stores the value of the parameters (&para=324234)
        /// </summary>
        public NameValueCollection QueryString { get; }

        /// <summary>
        ///     Gets or sets the item that has been evaluated up to now.
        /// </summary>
        public object? CurrentItem { get; set; }

        /// <summary>
        ///     Stores the extent being connected to the
        /// </summary>
        public IUriExtent Extent { get; }
    }
}