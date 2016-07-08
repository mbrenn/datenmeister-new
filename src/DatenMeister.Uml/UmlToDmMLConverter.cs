using DatenMeister.Runtime.Reflection;

namespace DatenMeister.Uml
{
    public class UmlToDmMLConverter
    {
        /// <summary>
        /// Converts a uml method to a DmML method by moving the supported content
        /// </summary>
        /// <param name="uml">Uml to be used</param>
        /// <param name="dmMl">DmML to be used</param>
        public static void Convert(_UML uml, DmML dmMl)
        {
            // Sets the classes
            dmMl.__NamedElement = uml.CommonStructure.__NamedElement;
            dmMl.__Class = uml.StructuredClassifiers.__Class;
            dmMl.__Property = uml.Classification.__Property;
            
            // Sets the attributes
            dmMl.NamedElement.Name = _UML._CommonStructure._NamedElement.name;
            dmMl.Class.Attribute = _UML._Classification._Classifier.attribute;
        }
    }
}