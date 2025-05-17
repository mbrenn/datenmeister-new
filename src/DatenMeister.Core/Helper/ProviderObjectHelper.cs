using DatenMeister.Core.Provider;

namespace DatenMeister.Core.Helper
{
    public static class ProviderObjectHelper
    {
        public static bool IsRoot(this IProviderObject providerObject)
        {
            return providerObject.Provider.GetRootObjects().Any(x => x.Equals(providerObject));
        }
    }
}