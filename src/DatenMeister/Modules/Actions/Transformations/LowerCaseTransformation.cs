using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.Actions.Transformations
{
    /// <summary>
    /// This is an example implementation which converts all properties to an upper case    
    /// </summary>
    public class LowerCaseTransformation : IItemTransformation
    {
        public void TransformItem(IElement element, IElement actionConfiguration)
        {
            if (!(element is IObjectAllProperties asHasProperties)) return;

            foreach (var property in asHasProperties.getPropertiesBeingSet().ToList())
            {
                if ((!element.isSet(property) ? null : element.get(property)) is string propertyAsString)
                {
                    element.set(property, propertyAsString.ToLowerInvariant());   
                }
            }
        }
    }
}