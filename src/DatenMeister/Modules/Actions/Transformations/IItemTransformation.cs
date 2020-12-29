using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.Actions.Transformations
{
    /// <summary>
    /// This interface is implemented by the item transformater being callable via the corresponding action. 
    /// </summary>
    public interface IItemTransformation
    {
        /// <summary>
        /// Implements the transformation of the item 
        /// </summary>
        /// <param name="element">Element to be transformed</param>
        /// <param name="actionConfiguration">Defines the action element which contains also
        /// the configuration</param>
        void TransformItem(IElement element, IElement actionConfiguration);
    }
}