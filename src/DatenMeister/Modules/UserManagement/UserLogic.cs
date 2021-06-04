﻿using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration;
using DatenMeister.Plugins;
using DatenMeister.Runtime.Functions.Algorithm;
using DatenMeister.Types;

namespace DatenMeister.Modules.UserManagement
{
    /// <summary>
    /// Implements the logic for the user
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
    [PluginDependency(typeof(LocalTypeSupport))]
    public class UserLogic : IDatenMeisterPlugin
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(UserLogic));

        /// <summary>
        /// Defines the default extent for the users
        /// </summary>
        public const string ExtentUri = "dm:///_internal/users";

        private const string ExtentTypeName = "DatenMeister.Users";

        private readonly LocalTypeSupport _localTypeSupport;

        private readonly ExtentCreator _extentCreator;

        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly IntegrationSettings _integrationSettings;
        private IList<IElement>? _types;
        private IElement? _settingsMetaClass;
        private IUriExtent? _extent;

        public UserLogic(
            LocalTypeSupport localTypeSupport,
            ExtentCreator extentCreator,
            IWorkspaceLogic workspaceLogic,
            IScopeStorage scopeStorage)
        {
            _localTypeSupport = localTypeSupport;
            _extentCreator = extentCreator;
            _workspaceLogic = workspaceLogic;
            _integrationSettings = scopeStorage.Get<IntegrationSettings>();
        }

        /// <summary>
        /// Clears the database and all user
        /// </summary>
        public void ClearDatabase()
        {
            GetUserDatabase().elements().clear();
        }

        public (string password, string salt) HashPassword(string password)
        {
            var userDatabase = GetUserDatabase();
            var salt = RandomFunctions.GetRandomAlphanumericString(16);
            var settingsMetaClass = _localTypeSupport.GetMetaClassFor(typeof(UserManagementSettings));

            if (!(userDatabase.elements().WhenMetaClassIs(settingsMetaClass).FirstOrDefault() is IElement settings))
            {
                throw new InvalidOperationException("UserManagementSettings is not found");
            }

            var globalSalt = settings.getOrDefault<string>(nameof(UserManagementSettings.salt));

            var totalPassword = salt + password + globalSalt;

            return (totalPassword, salt);
        }

        private IUriExtent GetUserDatabase()
        {
            var userDatabase = _workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceManagement)
                ?.FindExtent(ExtentTypeName);
            if (userDatabase == null)
            {
                throw new InvalidOperationException("User Database was not found");
            }

            return userDatabase;
        }

        public void AddUser(string username, string password)
        {
        }

        public bool VerifyUser(string username, string password) => true;

        public void ChangePassword(string username, string password)
        {
        }

        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    _types = _localTypeSupport.AddInternalTypes(
                        new[] {typeof(User), typeof(UserManagementSettings)},
                        "DatenMeister::UserManagement"
                    );

                    _settingsMetaClass = _localTypeSupport.GetMetaClassFor(typeof(UserManagementSettings));

                    break;

                case PluginLoadingPosition.AfterLoadingOfExtents:
                    _extent = _extentCreator.GetOrCreateXmiExtentInInternalDatabase(
                        WorkspaceNames.WorkspaceManagement,
                        ExtentUri,
                        ExtentTypeName,
                        "",
                        _integrationSettings.InitializeDefaultExtents
                            ? ExtentCreationFlags.CreateOnly
                            : ExtentCreationFlags.LoadOrCreate);

                    if (_extent == null)
                    {
                        throw new InvalidOperationException("Extent could not be created");
                    }

                    if (_types == null)
                    {
                        throw new InvalidOperationException($"_extent or _types are null");
                    }

                    if (!(_extent.elements().WhenMetaClassIs(_settingsMetaClass).FirstOrDefault() is IElement))
                    {
                        // Create new settings
                        var factory = new MofFactory(_extent);
                        var element = factory.create(_types[1]);
                        element.set(nameof(UserManagementSettings.salt),
                            RandomFunctions.GetRandomAlphanumericString(16));
                        _extent.elements().add(element);
                        Logger.Info("UserLogic - Created Salt...");
                    }
                    else
                    {
                        Logger.Debug("UserLogic - Salt is existing");
                    }

                    break;
            }
        }
    }
}