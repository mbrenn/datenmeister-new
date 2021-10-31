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
        IElement? CreateExtentFormForItem(IObject element, FormFactoryConfiguration configuration);

        IElement? CreateDetailFormForItem(IObject element, FormFactoryConfiguration configuration);

        /// <summary>
        /// Gets the extent form for a certain item's metaclass.
        /// This method can be used when the object to which a form shall be provided is not available 
        /// </summary>
        /// <param name="metaClass">MetaClass to which the form shall be provided</param>
        /// <param name="configuration">Configuration to be used</param>
        /// <returns>The instance of the extent form</returns>
        IElement? CreateExtentFormForItemsMetaClass(IElement metaClass, FormFactoryConfiguration configuration);

        IElement? CreateListFormForCollection(IReflectiveCollection collection, FormFactoryConfiguration configuration);

        IElement? CreateExtentFormForExtent(IExtent extent, FormFactoryConfiguration configuration);

        IElement? CreateListFormForPropertyValues(IObject element, string propertyName, IElement? propertyType, FormFactoryConfiguration configuration);
        
        
        public IElement? CreateListFormForPropertyValues(IObject element, string propertyName, FormFactoryConfiguration configuration)
        {
            return CreateListFormForPropertyValues(element, propertyName, null, configuration);
        }
    }
}