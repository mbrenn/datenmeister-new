using System.Collections.Generic;
using DatenMeister.Core.EMOF.Implementation.Hooks.Library;

namespace DatenMeister.Core.EMOF.Implementation.Hooks
{
    /// <summary>
    ///     This class contains all resolver hooks that are allocated to the parsing of urls.
    ///     The resolver hooks are called whenever a parameter is given in the query string and
    ///     it can't be handled by the core.
    /// </summary>
    public class ResolveHookContainer
    {
        /// <summary>
        ///     Stores the resolvehooks
        /// </summary>
        public List<IResolveHook> ResolveHooks { get; } = new();

        /// <summary>
        ///     Adds a resolve hook
        /// </summary>
        /// <param name="parameter">Parameter to be evaluated</param>
        /// <param name="resolveHook">Resolvehook to be connected to the parameter</param>
        public void Add(IResolveHook resolveHook)
        {
            ResolveHooks.Add(resolveHook);
        }

        /// <summary>
        /// Adds the default hooks
        /// </summary>
        /// <param name="scopeStorage">Scopestorage to which the default hooks shall be added</param>
        public static void AddDefaultHooks(IScopeStorage scopeStorage)
        {
            scopeStorage.Get<ResolveHookContainer>().Add(new FullNameResolveHook());
            scopeStorage.Get<ResolveHookContainer>().Add(new PropertyResolveHook());
            scopeStorage.Get<ResolveHookContainer>().Add(new CompositeResolveHook());
            scopeStorage.Get<ResolveHookContainer>().Add(new FilterMetaClassResolveHook());
        }
    }
}