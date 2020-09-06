using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Models.EMOF;

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
            var metaClass = asElement?.getMetaClass();
            if (metaClass == null)
            {
                return false;
            }

            return metaClass.equals(_uml.Packages.__Package)
                   || metaClass.@equals(_CommonTypes.TheOne.Default.__Package);
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