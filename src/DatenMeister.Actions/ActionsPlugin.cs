﻿using DatenMeister.Core;
using DatenMeister.Plugins;
using System.Threading.Tasks;

namespace DatenMeister.Actions
{
    /// <inheritdoc />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ActionsPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Gets the scope storage
        /// </summary>
        private readonly IScopeStorage _scopeStorage;

        public ActionsPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }
        
        public Task Start(PluginLoadingPosition position)
        {
            _scopeStorage.Add(ActionLogicState.GetDefaultLogicState());

            return Task.CompletedTask;
        }
    }
}