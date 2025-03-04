﻿using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation
{
    public class FilterByMetaclassNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(FilterByMetaclassNodeEvaluation));

        public bool IsResponsible(IElement node)
        {
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.equals(_DatenMeister.TheOne.DataViews.__FilterByMetaclassNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {
            var inputNode = viewNode.getOrDefault<IElement>(_DatenMeister._DataViews._FilterByMetaclassNode.input);
            if (inputNode == null)
            {
                Logger.Warn("Input node not found");
                return new PureReflectiveSequence();
            }

            var input = evaluation.GetElementsForViewNode(inputNode);

            var type = viewNode.getOrDefault<IElement>(_DatenMeister._DataViews._FilterByMetaclassNode.metaClass);
            if (type == null)
            {
                return new TemporaryReflectiveSequence(input.WhenMetaClassIsNotSet());
            }

            return new TemporaryReflectiveSequence(
                input.WhenMetaClassIs(type));
        }
    }
}