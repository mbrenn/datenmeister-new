using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.DataViews
{
    public interface IDataViewNodeFactory
    {
        /// <summary>
        /// Checks whether the given node is responsible for the dataview node factory
        /// </summary>
        /// <param name="node">Node to be evaluated</param>
        /// <returns>true, if the factory is responsible</returns>
        public bool IsResponsible(IElement node);

        /// <summary>
        /// Evaluates the given view node and returns the resulting reflective collection
        /// </summary>
        /// <param name="evaluation">The evaluation engine being used to get the context and the
        /// source reflection</param>
        /// <param name="viewNode">The element's definition of the view node</param>
        /// <returns>The resulting reflective collection</returns>
        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode);
    }
}