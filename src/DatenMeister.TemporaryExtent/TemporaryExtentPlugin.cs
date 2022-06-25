using System;
using System.Threading;
using System.Threading.Tasks;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Plugins;

namespace DatenMeister.TemporaryExtent
{
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterShutdownStarted)]
    public class TemporaryExtentPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(TemporaryExtentPlugin));
        
        /// <summary>
        /// The Uri of the temporary extent
        /// </summary>
        public const string Uri = "dm:///_internal/temp";
        
        private readonly IWorkspaceLogic _workspaceLogic;
        private CancellationToken _taskCancellation;
        private CancellationTokenSource? _source;

        /// <summary>
        /// Defines the period time in which the background task shall be activated
        /// </summary>
        public TimeSpan CleaningPeriod { get; } = TimeSpan.FromMinutes(1); 

        public TemporaryExtentPlugin(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }
        
        /// <summary>
        /// Starts the plugin by creating the InMemoryProvider. The provider will be directly added to
        /// the workspace logic since it shall not be persisted. Upon restart of server, the data may be lost
        /// </summary>
        /// <param name="position">Position to be used</param>
        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    _source = new CancellationTokenSource();
                    _taskCancellation = _source.Token;
                    var temporaryProvider = new InMemoryProvider();
                    var extent = new MofUriExtent(temporaryProvider, "dm:///_internal/temp", null);
                    _workspaceLogic.AddExtent(_workspaceLogic.GetDataWorkspace(), extent);
                    Task.Run(() => CleanTemporaryExtentRunAsync(_taskCancellation));
                    
                    break;
                case PluginLoadingPosition.AfterShutdownStarted:
                    // Being called after the shutdown started
                    if (_source != null)
                    {
                        _source.Cancel();
                    }
                    else
                    {
                        throw new InvalidOperationException("_source was not set");
                    }
                    break;
            }
        }

        /// <summary>
        /// The infinite loop taking care for cleaning up the temporary extent
        /// </summary>
        public async void CleanTemporaryExtentRunAsync(CancellationToken cancellationToken)
        {
            var logic = new TemporaryExtentLogic(_workspaceLogic);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(CleaningPeriod, cancellationToken);
                    logic.CleanElements();
                    ClassLogger.Info("Cleaning task executed");   
                }
            }
            catch (OperationCanceledException)
            {
                ClassLogger.Info("Cleaning task ended");   
            }
        }
    }
}