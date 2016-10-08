namespace DatenMeister.Runtime.Functions.Transformation
{
    public class HierarchyByChildrenSettings : HierarchyMakerBase
    {
        /// <summary>
        /// Gets or sets the name of the column containing the ids of the children.
        /// The text of the element can be a csv-separated list of multiple elements since one parent element
        /// might have a relationship to multiple parents
        /// </summary>
        public string OldChildrenColumn { get; set; }

        /// <summary>
        /// Gets or sets the separator being used to split the values of child id
        /// </summary>
        public string ChildIdSeparator { get; set; }

        /// <summary>
        /// Gets or sets the name of the column that shall store the list of child elements
        /// </summary>
        public string NewChildColumn { get; set; }
    }
}