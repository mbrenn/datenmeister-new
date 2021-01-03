using DatenMeister.Provider;
using DatenMeister.Provider.InMemory;

namespace DatenMeister.Runtime
{
    public static class ProviderObjectHelper
    {
        /// <summary>
        /// Defines the provider object which is set for root items 
        /// </summary>
        public static readonly IProviderObject RootContainerProviderObject;

        /// <summary>
        /// Performs the static initialization
        /// </summary>
        static ProviderObjectHelper()
        {
            RootContainerProviderObject = new InMemoryObject(InMemoryProvider.TemporaryProvider);
        }

        /// <summary>
        /// Sets that the given provider object is attached to a root item
        /// </summary>
        /// <param name="providerObject">Provider Object which shall be set as a root</param>
        public static void SetAsRoot(this IProviderObject providerObject)
        {
            providerObject.SetContainer(RootContainerProviderObject);
        }
        
        /// <summary>
        /// Checks whether the given item is a root item
        /// </summary>
        /// <param name="providerObject">Provider Object to be evaluated</param>
        /// <returns>true, when the item is a root item</returns>
        public static bool IsRoot(this IProviderObject providerObject)
        {
            return providerObject.GetContainer() == RootContainerProviderObject;
        }
    }
}