using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.SourcecodeGenerator.SourceParser
{
    public class ElementSourceParser : ISourceParser
    {
        private readonly _UML _uml;

        public ElementSourceParser(_UML uml)
        {
            _uml = uml;
        }

        public bool IsPackage(IObject element)
        {
            var asElement = element as IElement;
            return Equals(asElement?.getMetaClass(), _uml.Packages.__Package);
            
        }

        public bool IsClass(IObject element)
        {
            var asElement = element as IElement;
            return Equals(asElement?.getMetaClass(), _uml.StructuredClassifiers.__Class);
        }

        public bool IsProperty(IObject element)
        {
            var asElement = element as IElement;
            return Equals(asElement?.getMetaClass(), _uml.Classification.__Property);
        }

        public bool IsPrimitiveType(IObject element)
        {
            var asElement = element as IElement;
            return Equals(asElement?.getMetaClass(), _uml.SimpleClassifiers.__PrimitiveType);
        }
    }
}