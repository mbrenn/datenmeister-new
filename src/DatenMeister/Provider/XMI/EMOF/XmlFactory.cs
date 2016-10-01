using System;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Provider.XMI.EMOF
{
    public class XmlFactory : IFactory
    {
        public XmlUriExtent Owner { get; set; }

        public string ElementName { get; set; } = "item";

        public IElement package { get; }
        
        public XmlFactory()
        {
        }

        /*public XmlFactory(XmlUriExtent owner)
        {
            _owner = owner;
            package = null;
        }*/

        public IElement create(IElement metaClass)
        {
            var node = new XElement(ElementName);
            if (metaClass != null)
            {
                node.Add(new XAttribute(XmlElement.TypeAttribute, metaClass.GetUri()));
            }

            return new XmlElement(node, Owner);
        }

        public IObject createFromString(IElement dataType, string value)
        {
            throw new NotImplementedException();
        }

        public string convertToString(IElement dataType, IObject value)
        {
            throw new NotImplementedException();
        }
    }
}