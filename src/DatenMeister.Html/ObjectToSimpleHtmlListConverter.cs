using System.Collections;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.HtmlEngine;

namespace DatenMeister.Html
{
    /// <summary>
    /// Takes an object and converts it to a div containing all properties and enumeration of arrays 
    /// </summary>
    public class ObjectToSimpleHtmlListConverter
    {
        /// <summary>
        /// Gets or sets the recursion depth 
        /// </summary>
        public int RecursionDepth { get; set; } = 5;

        /// <summary>
        /// Converts the element to a div
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="currentRecursion">Describes the depth of the current recursion.
        /// If recursion is too deep, then the digging will be aborted</param>
        /// <returns>Div Element including sub elements</returns>
        public HtmlElement ConvertToHtmlList(IObject? value, int currentRecursion = 0)
        {
            if (value == null)
            {
                return new HtmlSpanElement(new HtmlRawString("<em>null</em>"));
            }

            var htmlListElement = new HtmlListElement();

            foreach (var property in ObjectHelper.GetPropertyNames(value))
            {
                var propertyValue = value.isSet(property) ? value.get(property) : null;

                ConvertPropertyValue(htmlListElement, property, propertyValue, currentRecursion);
            }

            return htmlListElement;
        }

        /// <summary>
        /// Converts a certain property value to an html, including the property text
        /// </summary>
        /// <param name="parent">The list element to which the property values shall be added</param>
        /// <param name="property">Property from which the current value is retrieved</param>
        /// <param name="propertyValue">The value of the property</param>
        /// <param name="currentRecursion">The recursion counter to avoid unlimited parsing</param>
        /// <returns>The found element</returns>
        private void ConvertPropertyValue(
            IHtmlElementWithMultipleItems parent,
            string property,
            object? propertyValue,
            int currentRecursion)
        {
            if (propertyValue == null)
            {
                parent.Items.Add(new HtmlSpanElement($"{property}: null"));
            }
            else if (propertyValue is IObject asObject)
            {
                if (currentRecursion >= RecursionDepth)
                {
                    parent.Items.Add(
                        new HtmlSpanElement($"{property}: {NamedElementMethods.GetName(propertyValue)} [...]"));
                }
                else
                {
                    var metaClass = "";
                    if (propertyValue is IElement asElement)
                    {
                        metaClass = $" [{NamedElementMethods.GetName(asElement.getMetaClassWithoutTracing())}]";
                    }

                    parent.Items.Add(
                        new HtmlContent(
                            new HtmlSpanElement($"{NamedElementMethods.GetName(propertyValue)}{metaClass}"),
                            new HtmlBrElement(),
                            ConvertToHtmlList(asObject, currentRecursion + 1)));
                }
            }
            else if (DotNetHelper.IsOfEnumeration(propertyValue) && propertyValue is IEnumerable enumerable)
            {
                var n = 0;
                
                var contentElement = new HtmlListElement {IsOrderedList = true};
                foreach (var element in enumerable)
                {
                    if (currentRecursion >= RecursionDepth)
                    {
                        contentElement.Items.Add(
                            new HtmlSpanElement($"{NamedElementMethods.GetName(propertyValue)} [...]"));
                    }
                    else
                    {
                        ConvertPropertyValue(contentElement, property, element, currentRecursion + 1);
                    }

                    n++;
                }

                parent.Items.Add(
                    new HtmlContent(
                        new HtmlSpanElement(property),
                        new HtmlBrElement(),
                        contentElement));
            }
            else
            {
                parent.Items.Add($"{property}: {DotNetHelper.AsString(propertyValue)}" );
            }
        }
    }
}