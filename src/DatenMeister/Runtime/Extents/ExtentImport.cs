﻿using System;
using System.IO;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Exceptions;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.Runtime.Extents
{
    public class ExtentImport
    {
        private readonly ExtentManager _extentManager;

        /// <summary>
        /// Stores the local type support
        /// </summary>
        private readonly LocalTypeSupport _localTypeSupport;

        public ExtentImport(ExtentManager extentManager, LocalTypeSupport localTypeSupport)
        {
            _extentManager = extentManager;
            _localTypeSupport = localTypeSupport;
        }

        /// <summary>
        /// Imports the file for according the given settings.
        /// The settings are of type "DatenMeister.UserFormSupport.LoadFileFromXmi"
        /// </summary>
        /// <param name="mofImportSettings">Import settings being used</param>
        /// <returns>The created uri extent</returns>
        public IUriExtent ImportExtent(IObject mofImportSettings)
        {
            var extentUri = mofImportSettings.getOrDefault<string>("extentUri");
            var workspaceId = mofImportSettings.getOrDefault<string>("workspace");
            var filePath = mofImportSettings.getOrDefault<string>("filePath");
            var pathExtension = Path.GetExtension(filePath).ToLower();
            if (pathExtension == ".xmi" || pathExtension == ".xml")
            {
                var configuration =
                    InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
                configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri, extentUri);
                configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath, filePath);
                configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId, workspaceId);
                
                var resultingExtent = _extentManager.LoadExtent(configuration);

                if (resultingExtent.LoadingState == ExtentLoadingState.Failed)
                {
                    var exception = _localTypeSupport.CreateInternalType("#DatenMeister.Models.ExtentManager.ImportException");
                    exception.set("message", "Loading did not succeed");
                    throw new DatenMeisterException(exception);
                }

                return resultingExtent.Extent ?? throw new InvalidOperationException("Loading should have succeeded");
            }

            var resultingException = _localTypeSupport.CreateInternalType("#DatenMeister.Models.ExtentManager.ImportException");
            resultingException.set("message", "Unknown file extension (.xmi and .xml are supported)");
            throw new DatenMeisterException(resultingException);
        }
    }
}