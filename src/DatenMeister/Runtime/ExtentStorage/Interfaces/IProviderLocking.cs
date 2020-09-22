using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.ExtentStorage.Interfaces
{
    /// <summary>
    /// Performs the locking of the provider
    /// </summary>
    public interface IProviderLocking
    {
        /// <summary>
        /// Checks whether the provider with the given configuration is currently locked
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>true, if the provider is currently locked</returns>
        public bool IsLocked(IElement configuration);

        /// <summary>
        /// Locks the given provider, so another instance cannot open the given provider
        /// </summary>
        /// <param name="configuration"></param>
        public void Lock(IElement configuration);

        /// <summary>
        /// Unlocks the given provider information. Usually, this method is called during the unloading of the
        /// provider. But the providerloader shall be capable to 
        /// </summary>
        /// <param name="configuration"></param>
        public void Unlock(IElement configuration);
    }
}