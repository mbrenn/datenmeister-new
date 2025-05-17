using System.Diagnostics;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView
{
    public class DataViewEvaluation
    {
        private const int MaximumReferenceCount = 1000;

        /// <summary>
        /// Stores the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(DataViewEvaluation));

        /// <summary>
        /// Adds the dynamic sources
        /// </summary>
        private readonly Dictionary<string, IReflectiveCollection> _dynamicSources = new();

        private readonly IWorkspaceLogic? _workspaceLogic;

        private DataViewNodeFactories _dataViewFactories;

        private int _referenceCount;

        private Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Gets or sets the time for the maximum execution timing of the dataview. If the execution time is longer, the evaluation will be stopped
        /// and an exception will be thrown
        /// </summary>
        public TimeSpan MaximumExecutionTiming = TimeSpan.MaxValue;

        public DataViewEvaluation(DataViewNodeFactories dataViewFactories)
        {
            _dataViewFactories = dataViewFactories;
        }

        public DataViewEvaluation(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _dataViewFactories = scopeStorage.Get<DataViewNodeFactories>();
        }

        public DataViewEvaluation(IWorkspaceLogic workspaceLogic, DataViewNodeFactories dataViewFactories)
        {
            _workspaceLogic = workspaceLogic;
            _dataViewFactories = dataViewFactories;
        }

        /// <summary>
        /// Adds the dynamic sources
        /// </summary>
        public Dictionary<string, IReflectiveCollection> DynamicSources => _dynamicSources;

        public IWorkspaceLogic? WorkspaceLogic => _workspaceLogic;

        /// <summary>
        /// Adds a dynamic source for the collection
        /// </summary>
        /// <param name="name">Name of the </param>
        /// <param name="collection"></param>
        public void AddDynamicSource(string name, IReflectiveCollection collection)
        {
            _dynamicSources[name] = collection;
        }

        /// <summary>
        /// Parses the given view node and return the values of the viewnode as a reflective sequence
        /// </summary>
        /// <param name="viewNode">View Node to be parsed</param>
        /// <returns>The reflective Sequence</returns>
        public IReflectiveCollection GetElementsForViewNode(IElement viewNode)
        {
            if (_referenceCount == 0)
            {
                stopwatch.Start();
            }

            _referenceCount++;
            try
            {
                if (_referenceCount > MaximumReferenceCount)
                {
                    Logger.Error("Maximum number of recursions are evaluated in dataview evaluation");
                    throw new InvalidOperationException("Maximum number of recursions are evaluated in dataview evaluation");
                }

                // Checks the timeout
                if (MaximumExecutionTiming != TimeSpan.MaxValue && stopwatch.Elapsed > MaximumExecutionTiming)
                {
                    Logger.Error("Maximum execution timing exceeded");
                    throw new InvalidOperationException("Maximum execution timing exceeded");
                }

                // Gets the elements
                var result = GetElementsForViewNodeInternal(viewNode);
                return result;
            }
            finally
            {
                _referenceCount--;

                if (_referenceCount == 0)
                {
                    stopwatch.Stop();
                    Logger.Info($"Time for evaluation: {stopwatch.ElapsedMilliseconds}ms");
                    stopwatch.Reset();
                }
            }
        }

        /// <summary>
        /// Parses the given view node and return the values of the viewnode as a reflective sequence
        /// </summary>
        /// <param name="viewNode">View Node to be parsed</param>
        /// <returns>The reflective Sequence</returns>
        private IReflectiveCollection GetElementsForViewNodeInternal(IElement viewNode)
        {
            if (viewNode == null)
            {
                throw new ArgumentException(nameof(viewNode));
            }

            // Check, if viewnode has been visited
            foreach (var evaluation in
                     from x in _dataViewFactories.Evaluations
                     where x.IsResponsible(viewNode)
                     select x)
            {
                return evaluation.Evaluate(this, viewNode);
            }

            var metaClass = viewNode.getMetaClass();
            if (metaClass == null)
            {
                Logger.Warn("Unknown type of viewnode: null");
                throw new InvalidOperationException("Unknown type of viewnode: null");
            }

            Logger.Warn($"Unknown type of viewnode: {viewNode.getMetaClass()}");
            throw new InvalidOperationException($"Unknown type of viewnode: {viewNode.getMetaClass()}");
        }
    }
}