using System.Linq;
using DatenMeister.Provider;

namespace DatenMeister.Runtime
{
    public static class ProviderObjectHelper
    {
        public static bool IsRoot( this IProviderObject providerObject)
        {
            return providerObject.Provider.GetRootObjects().Any(x => x.Equals(providerObject));
        }
    }
}