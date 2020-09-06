using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Runtime.Proxies;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DataViews
{
    public class DataViewEvaluation
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(DataViewEvaluation));

        private readonly IWorkspaceLogic? _workspaceLogic;

        private int _referenceCount;

        private const int MaximumReferenceCount = 1000;

        /// <summary>
        /// Adds the dynamic sources
        /// </summary>
        private readonly Dictionary<string, IReflectiveCollection> _dynamicSources =
            new Dictionary<string, IReflectiveCollection>();

        private DataViewNodeFactories _dataViewFactories;

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
                Logger.Warn($"Unknown type of viewnode: null");
                return new PureReflectiveSequence();
            }

            Logger.Warn($"Unknown type of viewnode: {viewNode.getMetaClass()}");
            return new PureReflectiveSequence();
        }
    }
}