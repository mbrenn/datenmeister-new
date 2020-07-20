using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.DataViews;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Proxies;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.DataViews.Evaluation
{
    public class SelectPathNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(SelectPathNodeEvaluation));
        public bool IsResponsible(IElement node)
        {
            
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.@equals(_DataViews.TheOne.__SelectPathNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {
            
            var inputNode = viewNode.getOrDefault<IElement>(_DataViews._SelectPathNode.input);
            if (inputNode == null)
            {
                Logger.Warn($"Input node not found");
                return new PureReflectiveSequence();
            }

            var input = evaluation.GetElementsForViewNode(inputNode);

            var pathNode = viewNode.getOrDefault<string>(_DataViews._SelectPathNode.path);
            if (pathNode == null)
            {
                Logger.Warn($"Path is not set");
                return new PureReflectiveSequence();
            }

            var targetElement = NamedElementMethods.GetByFullName(input, pathNode);
            if (targetElement == null)
            {
                // Path is not found
                return new PureReflectiveSequence();
            }
            
            return new TemporaryReflectiveSequence(NamedElementMethods.GetAllPropertyValues(targetElement));
        }
    }
}