using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Modules.DefaultTypes
{
    public class ObjectOperations
    {
        /// <summary>
        /// Moves the object to the target 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetContainer">Defines the target container receiving the object</param>
        public void MoveObject(IObject value, IObject targetContainer)
        {
            var extent = value.GetExtentOf();
            var container = (value as IElement)?.container();

            if (container != null || extent != null)
            {
                DefaultClassifierHints.RemoveFromExtentOrElement(container ?? (IObject) extent!, value);

                if (value is MofObject mofObject)
                {
                    mofObject.Extent = null;
                }
            }
            
            DefaultClassifierHints.AddToExtentOrElement(targetContainer, value);    
        }
        
        /// <summary>
        /// Copies the object to the target 
        /// </summary>
        /// <param name="value">The value to be copied</param>
        /// <param name="targetContainer">Defines the target container receiving the object</param>
        public void CopyObject(IObject value, IObject targetContainer)
        {
            var options = new CopyOption {CloneAllReferences = false};
            var copied = ObjectCopier.Copy(new MofFactory(targetContainer), value, options);
            DefaultClassifierHints.AddToExtentOrElement(targetContainer, copied);
        }
    }
}