using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Core.EMOF.Implementation.DefaultValue
{
    /// <summary>
    /// The handler to set the default value 
    /// </summary>
    public static class DefaultValueHandler
    {
        /// <summary>
        /// Handles the new item 
        /// </summary>
        /// <param name="parentExtent">Extent in which the item is created</param>
        /// <param name="element">Element which is recently created</param>
        public static void HandleNewItem(IExtent parentExtent, IElement element)
        {
            // Gets the type and the associated properties
            var type = element.getMetaClass();
            if (type == null)
            {
                // Nothing to do
                return;
            }

            var properties = ClassifierMethods.GetPropertiesOfClassifier(type);
            foreach (var property in properties)
            {
                var defaultValue = property.getOrDefault<object>(_UML._Classification._Property.defaultValue);
                var name = property.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
                
                if (defaultValue != null)
                {
                    element.set(name, defaultValue);
                }
            }
        }
    }
}