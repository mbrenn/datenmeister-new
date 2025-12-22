using System.Runtime.CompilerServices;
using DatenMeister.Core.Interfaces.Provider;

namespace DatenMeister.Core.Helper;

public static class ProviderObjectHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsRoot(this IProviderObject providerObject)
    {
        return providerObject.Provider.GetRootObjects().Any(x => x.Equals(providerObject));
    }
}