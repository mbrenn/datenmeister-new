using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.Actions.Transformations
{
    public interface IItemTransformation
    {
        /// <summary>
        /// Implements the transformation of the item 
        /// </summary>
        /// <param name="element">Element to be transformed</param>
        void TransformItem(IElement element);
    }
}