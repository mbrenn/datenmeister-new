using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Modules.DataViews.Evaluation
{
    public class FilterTypeNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(FilterTypeNodeEvaluation));
        public bool IsResponsible(IElement node)
        {
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.@equals(_DatenMeister.TheOne.DataViews.__FilterTypeNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {
            var inputNode = viewNode.getOrDefault<IElement>(_DatenMeister._DataViews._FilterTypeNode.input);
            if (inputNode == null)
            {
                Logger.Warn($"Input node not found");
                return new PureReflectiveSequence();
            }

            var input = evaluation.GetElementsForViewNode(inputNode);

            var type = viewNode.getOrDefault<IElement>(_DatenMeister._DataViews._FilterTypeNode.type);
            if (type == null)
            {
                return new TemporaryReflectiveSequence(input.WhenMetaClassIsNotSet());
            }

            return new TemporaryReflectiveSequence(
                input.WhenMetaClassIs(type));
        }
    }
}