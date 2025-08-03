using System.Collections.Specialized;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core.EMOF.Implementation.Hooks;

public class ResolveHookParameters(
    IScopeStorage? scopeStorage,
    NameValueCollection queryString,
    object? currentItem,
    IUriExtent extent)
{
    public IScopeStorage? ScopeStorage { get; } = scopeStorage;

    /// <summary>
    /// Stores the value of the parameters (&amp;para=324234)
    /// </summary>
    public NameValueCollection QueryString { get; } = queryString;

    /// <summary>
    ///     Gets or sets the item that has been evaluated up to now.
    /// </summary>
    public object? CurrentItem { get; set; } = currentItem;

    /// <summary>
    ///     Stores the extent being connected to the
    /// </summary>
    public IUriExtent Extent { get; } = extent;
}