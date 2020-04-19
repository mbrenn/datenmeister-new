#nullable enable

using System;
using BurnSystems.Logging;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Provider.InMemory
{
    [ConfiguredBy(typeof(InMemoryLoaderConfig))]
    public class InMemoryProviderLoader : IProviderLoader
    {
        /// <summary>
        /// Sores the logger
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(InMemoryLoaderConfig));
        
        /// <summary>
        /// Just creates the provider for the memory
        /// </summary>
        /// <param name="configuration">Configuration to be used</param>
        /// <param name="extentCreationFlags"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            if (extentCreationFlags == ExtentCreationFlags.LoadOnly)
            {
                throw new NotImplementedException("Loading the extent via ExtentCreationFlags.LoadOnly is not possible");
            }
            
            Logger.Info("InMemoryProvider is created");
            
            var provider = new InMemoryProvider();
            return new LoadedProviderInfo(provider);
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            Logger.Info("Storing of in Memory Object is not possible");
        }
    }
}