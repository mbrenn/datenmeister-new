using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DatenMeister.HtmlEngine
{
    public interface IHtmlElementWithMultipleItems
    {
        /// <summary>
        /// Gets a list of elements being concatenated
        /// </summary>
        List<HtmlElement> Items { get; }
    }

    /// <summary>
    /// Defines a content container for multiple html elements 
    /// </summary>
    public class HtmlContent : HtmlElement, IHtmlElementWithMultipleItems
    {
        /// <summary>
        /// Gets a list of elements being concatenated
        /// </summary>
        public List<HtmlElement> Items { get; } = new();

        /// <summary>
        /// Initializes a new instance of the HtmlContent
        /// </summary>
        public HtmlContent()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the HtmlContent
        /// </summary>
        /// <param name="items">Items to be added</param>
        public HtmlContent(params HtmlElement[] items)
        {
            Items.AddRange(items);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var item in Items)
            {
                builder.Append(item);
            }

            return builder.ToString();
        }
    }
}