using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Proxies;
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
            _referenceCount++;
            if (_referenceCount > MaximumReferenceCount)
            {
                Logger.Warn("Maximum number of recursions are evaluated in dataview evaluation");
                return new PureReflectiveSequence();
            }

            var result = GetElementsForViewNodeInternal(viewNode);
            _referenceCount--;

            return result;
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
                return new PureReflectiveSequence();
            }

            Logger.Warn($"Unknown type of viewnode: {viewNode.getMetaClass()}");
            return new PureReflectiveSequence();
        }
    }
}