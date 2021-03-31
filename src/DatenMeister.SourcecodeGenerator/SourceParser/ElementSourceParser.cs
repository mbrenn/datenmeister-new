using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Models.EMOF;

namespace DatenMeister.SourcecodeGenerator.SourceParser
{
    public class ElementSourceParser : ISourceParser
    {
        public bool IsPackage(IObject element)
        {
            var asElement = element as IElement;
            var metaClass = asElement?.getMetaClass();
            if (metaClass == null)
            {
                return false;
            }

            return metaClass.equals(_UML.TheOne.Packages.__Package)
                   || metaClass.@equals(_DatenMeister.TheOne.CommonTypes.Default.__Package);
        }

        public bool IsClass(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_UML.TheOne.StructuredClassifiers.__Class) == true;
        }

        public bool IsEnum(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_UML.TheOne.SimpleClassifiers.__Enumeration) == true;
        }

        public bool IsEnumLiteral(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_UML.TheOne.SimpleClassifiers.__EnumerationLiteral) == true;
        }

        public bool IsProperty(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_UML.TheOne.Classification.__Property) == true;
        }

        public bool IsPrimitiveType(IObject element)
        {
            var asElement = element as IElement;
            return asElement?.getMetaClass()?.equals(_UML.TheOne.SimpleClassifiers.__PrimitiveType) == true;
        }
    }
}