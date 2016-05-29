using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Reflection
{
    /// <summary>
    /// Defines the Datenmeister-Metamodel which is used to navigate through the instances
    /// of DatenMeister. This abstracts the Uml model to a more usable model.
    /// The model is directly used by DatenMeister.
    /// </summary>
    public class DmML
    {
        public class _NamedElement
        {
            public object Name = "name";
        }

        public class _Property
        {

        }

        public class _Class
        {
            public object Attribute = "attribute";
        }

        public IElement __NamedElement = new MofElement();

        public IElement __Class = new MofElement();

        public IElement __Property = new MofElement();

        public _NamedElement NamedElement = new _NamedElement();

        public _Class Class = new _Class();

        public _Property Property = new _Property();
    }
}