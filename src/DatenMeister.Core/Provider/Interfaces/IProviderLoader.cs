﻿using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using System.Threading.Tasks;

namespace DatenMeister.Core.Provider.Interfaces
{
    public class ProviderLoaderCapabilities
    {
        /// <summary>
        /// Gets or sets the information whether the data is loaded from a
        /// persistant storage or whether it has been created completely from memory.
        /// The value is true, when the content is the same at two start-ups. 
        /// </summary>
        public bool IsPersistant = false;

        /// <summary>
        /// Gets or sets the information whether changes which have been done
        /// are also persistant on the dist. Or whether this is a read-only
        /// provider.
        /// </summary>
        public bool AreChangesPersistant = false;

        /// <inheritdoc />
        public override string ToString()
        {
            return $"IsPersistant: {IsPersistant}, AreChangesPersistant: {AreChangesPersistant}";
        }
    }
    /// <summary>
    /// Defines the interface to store an extent persistently whether in a database or
    /// in the file.
    /// The application can use this implementation to load and store a file on startup
    /// and end of application
    /// </summary>
    public interface IProviderLoader
    {
        /// <summary>
        /// Gets or sets the workspace logic
        /// </summary>
        IWorkspaceLogic? WorkspaceLogic { get; set;  }
        
        /// <summary>
        /// Gets or sets the scope storage
        /// </summary>
        IScopeStorage? ScopeStorage { get; set;  }
        
        /// <summary>
        /// Loads the extent according to the given configuration
        /// </summary>
        /// <param name="configuration">Configuration to be used to retrieve the information.
        /// The configuration may be changed, if the provider
        /// loader is just a placeholder for another configuration</param>
        /// <param name="extentCreationFlags">true, if the extent shall also be created, if it is empty.
        /// Can be used to create an empty extent. </param>
        /// <returns>Loaded extent</returns>
        Task<LoadedProviderInfo> LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags);

        /// <summary>
        /// Sores the extent according to the given configuration
        /// </summary>
        /// <param name="provider">Provider to be stored</param>
        /// <param name="configuration">Configuration to be added</param>
        Task StoreProvider(IProvider provider, IElement configuration);

        /// <summary>
        /// Gets the provider loader capabilities
        /// </summary>
        ProviderLoaderCapabilities ProviderLoaderCapabilities { get; }
    }
}