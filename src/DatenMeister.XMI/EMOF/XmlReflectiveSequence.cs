using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Common;

namespace DatenMeister.XMI.EMOF
{
    public class XmlReflectiveSequence : IReflectiveSequence
    {
        private XElement _node;

        public XmlReflectiveSequence(XElement node)
        {
            _node = node;
        }
        
        public IEnumerator<object> GetEnumerator()
        {
            foreach (var subNode in _node.Elements())
            {
                yield return new XmlElement(subNode);
            }
        }

        public bool add(object value)
        {
            var valueAsXmlObject = ConvertValueAsXmlObject(value);
            _node.Add(valueAsXmlObject.XmlNode);

            return true;
        }

        public bool addAll(IReflectiveSequence value)
        {
            var result = false;
            foreach (var singleValue in value)
            {
                result |= add(singleValue);
            }

            return true;
        }

        public void clear()
        {
            _node.RemoveAll();
        }

        public bool remove(object value)
        {
            var valueAsXmlObject = ConvertValueAsXmlObject(value);
            foreach (var subElement in _node.Elements())
            {
                if (subElement.Equals(valueAsXmlObject.XmlNode))
                {
                    subElement.Remove();
                    return true;
                }
            }

            return false;
        }

        public int size()
        {
            return _node.Elements().Count();
        }

        public void add(int index, object value)
        {
            if (index == size())
            {
                add(value);
            }
            else
            {
                var valueAsXmlObject = ConvertValueAsXmlObject(value);
                var addedBefore = _node.Elements().ElementAt(index);
                addedBefore.AddBeforeSelf(valueAsXmlObject.XmlNode);
            }
        }

        public object get(int index)
        {
            return new XmlElement(_node.Elements().ElementAt(index));
        }

        public void remove(int index)
        {
            var value = get(index);
            remove(value);
        }

        public object set(int index, object value)
        {
            var valueAsXmlObject = ConvertValueAsXmlObject(value);
            var toBeReplaced = _node.Elements().ElementAt(index);
            toBeReplaced.AddBeforeSelf(valueAsXmlObject.XmlNode);
            toBeReplaced.Remove();

            return new XmlElement(toBeReplaced);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private XmlElement ConvertValueAsXmlObject(object value)
        {
            var valueAsXmlObject = value as XmlElement;
            if (valueAsXmlObject == null)
            {
                throw new InvalidOperationException("Value is not an XmlObject: " + value.ToString());
            }

            return valueAsXmlObject;
        }
    }
}