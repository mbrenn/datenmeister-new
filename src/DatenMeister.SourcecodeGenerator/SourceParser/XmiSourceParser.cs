﻿using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.XMI;

namespace DatenMeister.SourcecodeGenerator.SourceParser
{
    /// <summary>
    /// Parses the raw elements directly imported from the xmi file
    /// </summary>
    public class XmiSourceParser : ISourceParser
    {
        public bool IsPackage(IObject element)
        {
            var attributeXmi = "{" + Namespaces.Xmi + "}type";

            return element.isSet(attributeXmi) &&
                   element.get(attributeXmi).ToString() == "uml:Package";
        }

        public bool IsClass(IObject element)
        {
            var attributeXmi = "{" + Namespaces.Xmi + "}type";

            return element.isSet(attributeXmi) &&
                   element.get(attributeXmi).ToString() == "uml:Class";
        }

        public bool IsProperty(IObject element)
        {
            var attributeXmi = "{" + Namespaces.Xmi + "}type";

            return element.isSet(attributeXmi) &&
                   element.get(attributeXmi).ToString() == "uml:Property";
        }

        public bool IsPrimitiveType(IObject element)
        {
            var attributeXmi = "{" + Namespaces.Xmi + "}type";

            return element.isSet(attributeXmi) &&
                   element.get(attributeXmi).ToString() == "uml:PrimitiveType";
        }
    }
}