namespace DatenMeister.Runtime.Functions.Transformation
{
    public class HierarchyByParentSettings : HierarchyMakerBase
    {


        /// <summary>
        /// Gets or sets the name of the column containing the parent-relationship
        /// </summary>
        public string OldParentColumn { get; set; }

        /// <summary>
        /// Gets or sets the name of the column that shall store the list of child elements
        /// </summary>
        public string NewChildColumn { get; set; }
    }
}