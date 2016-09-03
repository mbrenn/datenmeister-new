using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.XMI.EMOF
{
    public class XmlReflectiveSequence : IReflectiveSequence
    {
        private readonly XmlUriExtent _extent;
        private readonly XElement _node;
        /// <summary>
        /// Defines the name of the property that is covered by the reflective sequence
        /// </summary>
        private readonly string _propertyName;

        public XmlReflectiveSequence(XmlUriExtent extent, XElement node, string propertyName)
        {
            _extent = extent;
            _node = node;
            _propertyName = propertyName;
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach (var subNode in _node.Elements(_propertyName))
            {
                yield return new XmlElement(subNode, _extent);
            }
        }

        public bool add(object value)
        {
            var valueAsXmlObject = ConvertValueAsXmlObject(value);
            _node.Add(valueAsXmlObject);

            return true;
        }

        public bool addAll(IReflectiveSequence value)
        {
            var result = false;
            foreach (var singleValue in value)
            {
                result |= add(singleValue);
            }

            return result;
        }

        public void clear()
        {
            _node.RemoveAll();
        }

        public bool remove(object value)
        {
            var valueAsXmlObject = ConvertValueAsXmlObject(value);
            foreach (var subElement in _node.Elements(_propertyName))
            {
                if (subElement.Equals(valueAsXmlObject))
                {
                    subElement.Remove();
                    return true;
                }
            }

            return false;
        }

        public int size()
        {
            return _node.Elements(_propertyName).Count();
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
                var addedBefore = _node.Elements(_propertyName).ElementAt(index);
                addedBefore.AddBeforeSelf(valueAsXmlObject);
            }
        }

        public object get(int index)
        {
            return new XmlElement(_node.Elements(_propertyName).ElementAt(index), _extent);
        }

        public void remove(int index)
        {
            var value = get(index);
            remove(value);
        }

        public object set(int index, object value)
        {
            var valueAsXmlObject = ConvertValueAsXmlObject(value);
            var toBeReplaced = _node.Elements(_propertyName).ElementAt(index);
            toBeReplaced.AddBeforeSelf(valueAsXmlObject);
            toBeReplaced.Remove();

            return new XmlElement(toBeReplaced, _extent);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private XElement ConvertValueAsXmlObject(object value)
        {
            var valueAsXmlObject = value as XmlElement;
            if (valueAsXmlObject != null)
            {
                return valueAsXmlObject.XmlNode;
            }

            var valueAsElement = value as IElement;
            if (valueAsElement != null)
            {
                var copier = new ObjectCopier(new XmlFactory { Owner = _extent, ElementName = _propertyName });
                return ((XmlElement) copier.Copy(valueAsElement)).XmlNode;
            }

            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                return new XElement(_propertyName, value.ToString());
            }

            throw new InvalidOperationException("Value is not an XmlObject or an IElement: " + value);
        }
    }
}