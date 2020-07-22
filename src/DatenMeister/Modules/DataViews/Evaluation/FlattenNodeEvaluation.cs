using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.DataViews;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Modules.DataViews.Evaluation
{
    public class FlattenNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(FlattenNodeEvaluation));
        public bool IsResponsible(IElement node)
        {
            
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.@equals(_DataViews.TheOne.__FlattenNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {          
            var inputNode = viewNode.getOrDefault<IElement>(_DataViews._FlattenNode.input);
            if (inputNode == null)
            {
                Logger.Warn($"Input node not found");
                return new PureReflectiveSequence();
            }

            return evaluation.GetElementsForViewNode(inputNode).GetAllDescendants();
        }
    }
}