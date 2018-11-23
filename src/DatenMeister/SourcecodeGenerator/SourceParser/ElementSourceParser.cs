using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;

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
            return asElement?.getMetaClass()?.equals(_uml.Packages.__Package) == true;
            
        }

        public bool IsClass(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_uml.StructuredClassifiers.__Class) == true;
        }

        public bool IsEnum(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_uml.SimpleClassifiers.__Enumeration) == true;
        }

        public bool IsEnumLiteral(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_uml.SimpleClassifiers.__EnumerationLiteral) == true;
        }

        public bool IsProperty(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_uml.Classification.__Property) == true;
        }

        public bool IsPrimitiveType(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_uml.SimpleClassifiers.__PrimitiveType) == true;
        }
    }
}