﻿using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation
{
    public class DynamicSourceNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(DynamicSourceNodeEvaluation));

        public bool IsResponsible(IElement node)
        {
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.equals(_DatenMeister.TheOne.DataViews.__DynamicSourceNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {
            var name = viewNode.getOrDefault<string>(_DatenMeister._DataViews._DynamicSourceNode.name);
            var nodeName = viewNode.getOrDefault<string>(_DatenMeister._DataViews._DynamicSourceNode.nodeName);

            nodeName = string.IsNullOrEmpty(nodeName) ? name : nodeName;
            if (nodeName == null)
            {
                Logger.Warn("Input node not found");
                return new PureReflectiveSequence();
            }

            if (evaluation.DynamicSources.TryGetValue(nodeName, out var collection))
            {
                return collection;
            }

            Logger.Warn($"Input node {name} not set");
            return new PureReflectiveSequence();
        }
    }
}