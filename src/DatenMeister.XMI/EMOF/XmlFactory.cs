using System.Xml.Linq;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.XMI.EMOF
{
    public class XmlFactory : IFactory
    {
        public XmlFactory()
        {
            package = null;
        }

        public IElement package { get; }

        public IElement create(IElement metaClass)
        {
            var node = new XElement("item");
            if (metaClass != null)
            {
                node.Add(new XAttribute(Namespaces.Xmi + "type", metaClass.GetUri()));
            }

            return new XmlElement(node);
        }

        public IObject createFromString(IElement dataType, string value)
        {
            throw new System.NotImplementedException();
        }

        public string convertToString(IElement dataType, IObject value)
        {
            throw new System.NotImplementedException();
        }
    }
}