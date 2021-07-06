namespace DatenMeister.HtmlEngine
{
    /// <summary>
    /// Defines the interface for html report which receives
    /// different Html Element
    /// </summary>
    public interface IHtmlReport
    {
        /// <summary>
        /// Adds the element that should be added to the report
        /// </summary>
        /// <param name="elementToBeAdded">Element that should be added
        /// should be an element within this namespace. </param>
        void Add(HtmlElement elementToBeAdded);
    }
}