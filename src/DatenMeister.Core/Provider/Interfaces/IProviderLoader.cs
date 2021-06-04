﻿using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.Provider.Interfaces
{
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
        LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags);

        /// <summary>
        /// Sores the extent according to the given configuration
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="configuration">Configuration to be added</param>
        void StoreProvider(IProvider extent, IElement configuration);
    }
}