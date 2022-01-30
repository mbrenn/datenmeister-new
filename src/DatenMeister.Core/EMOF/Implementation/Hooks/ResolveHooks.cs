using System.Collections.Generic;

namespace DatenMeister.Core.EMOF.Implementation.Hooks
{
    /// <summary>
    ///     This class contains all resolver hooks that are allocated to the parsing of urls.
    ///     The resolverhooks are called whenever a parameter is given in the query string and
    ///     it can't be handled by the core.
    /// </summary>
    public class ResolveHooks
    {
        /// <summary>
        ///     Stores the resolvehooks
        /// </summary>
        private readonly Dictionary<string, IResolveHook> _resolveHooks = new();

        /// <summary>
        ///     Adds a resolve hook
        /// </summary>
        /// <param name="parameter">Parameter to be evaluated</param>
        /// <param name="resolveHook">Resolvehook to be connected to the parameter</param>
        public void Add(string parameter, IResolveHook resolveHook)
        {
            _resolveHooks[parameter] = resolveHook;
        }

        /// <summary>
        ///     Gets one resolver hook
        /// </summary>
        /// <param name="parameter">Parameter to be evaluated</param>
        /// <returns>The resolver hook or null, if not found</returns>
        public IResolveHook? Get(string parameter)
        {
            if (_resolveHooks.TryGetValue(parameter, out var result)) return result;

            return null;
        }
    }
}