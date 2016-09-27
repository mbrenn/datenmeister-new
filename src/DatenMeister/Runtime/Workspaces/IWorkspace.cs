﻿using DatenMeister.Core.Filler;

namespace DatenMeister.Runtime.Workspaces
{
    public interface IWorkspace
    {
        /// <summary>
        /// Gets an instance of the filled type by using the filler. 
        /// The instance will be cached on first call of the method
        /// </summary>
        /// <typeparam name="TFiller">Filler to be used to create the filled type</typeparam>
        /// <typeparam name="TFilledType">The filled type which is returned</typeparam>
        /// <returns>The filled type, could also be cached</returns>
        TFilledType Create<TFiller, TFilledType>()
            where TFiller : IFiller<TFilledType>, new()
            where TFilledType : class, new();

        /// <summary>
        /// Gets a cached instance of the filled type. 
        /// This cached instance has to be created by the Create method before. If not found, 
        /// null will be returned
        /// </summary>
        /// <typeparam name="TFilledType">Type of the filled type</typeparam>
        /// <param name="layer">Layer whose filled type shall be retrieved</param>
        /// <returns>The found instance</returns>
        TFilledType Get<TFilledType>()
            where TFilledType : class, new();

        /// <summary>
        /// Clears the cache, so a new instance can be created
        /// </summary>
        /// <param name="layer">Layer, whose cache needs to be deleted</param>
        void ClearCache();

        /// <summary>
        /// Sets a filled type which is already prepared
        /// </summary>
        /// <param name="layer">Datalayer being filled</param>
        /// <param name="value">Value to be set for the datalayer</param>
        void Set<TFilledType>(TFilledType value)
            where TFilledType : class, new();
    }
}