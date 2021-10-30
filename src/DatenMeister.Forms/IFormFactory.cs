using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms
{
    public interface IFormFactory
    {
        /// <summary>
        /// Gets the extent form for a certain item
        /// </summary>
        /// <param name="element">Element to which the form is requested</param>
        /// <param name="configuration">Configuration to be used</param>
        /// <returns>The instance of the extent form</returns>
        IElement? GetExtentFormForItem(IObject element, FormFactoryConfiguration configuration);

        IElement? GetDetailFormForItem(IObject element, FormFactoryConfiguration configuration);

        /// <summary>
        /// Gets the extent form for a certain item's metaclass.
        /// This method can be used when the object to which a form shall be provided is not available 
        /// </summary>
        /// <param name="metaClass">MetaClass to which the form shall be provided</param>
        /// <param name="configuration">Configuration to be used</param>
        /// <returns>The instance of the extent form</returns>
        IElement? GetExtentFormForItemsMetaClass(IElement metaClass, FormFactoryConfiguration configuration);

        IElement? GetListFormForCollection(IReflectiveCollection collection, FormFactoryConfiguration configuration);

        IElement? GetExtentFormForExtent(IExtent extent, FormFactoryConfiguration configuration);

        IElement? GetListFormForPropertyValues(IObject element, string propertyName, IElement? propertyType, FormFactoryConfiguration configuration);
        
        
        public IElement? GetListFormForPropertyValues(IObject element, string propertyName, FormFactoryConfiguration configuration)
        {
            return GetListFormForPropertyValues(element, propertyName, null, configuration);
        }
    }
}