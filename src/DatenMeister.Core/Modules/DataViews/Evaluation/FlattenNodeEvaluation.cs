using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Modules.DataViews.Evaluation
{
    public class FlattenNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(FlattenNodeEvaluation));
        public bool IsResponsible(IElement node)
        {
            
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.equals(_DatenMeister.TheOne.DataViews.__FlattenNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {          
            var inputNode = viewNode.getOrDefault<IElement>(_DatenMeister._DataViews._FlattenNode.input);
            if (inputNode == null)
            {
                Logger.Warn("Input node not found");
                return new PureReflectiveSequence();
            }

            return evaluation.GetElementsForViewNode(inputNode).GetAllDescendants();
        }
    }
}