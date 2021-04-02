using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
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
        /// <param name="newValue">Element which is recently created</param>
        /// <param name="metaClass">Defines the metaclass of the new value</param>
        public static void HandleNewItem(IElement newValue, IElement? metaClass)
        {
            // Gets the type and the associated properties
            var type = metaClass;
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
                    newValue.set(name, defaultValue);
                }
            }
        }
    }
}