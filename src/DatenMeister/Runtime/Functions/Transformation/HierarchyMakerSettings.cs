using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Transformation
{
    public class HierarchyMakerSettings
    {
        /// <summary>
        /// Gets or sets the extent that is used to convert the elements
        /// </summary>
        public IReflectiveSequence Sequence { get; set; }

        /// <summary>
        /// Gets or sets the target extent that shall retrieve the converted elements. 
        /// If this element is null, then the original extent will be modified
        /// </summary>
        public IReflectiveSequence TargetSequence { get; set; }

        /// <summary>
        /// Gets or sets the factory being used to create the objects in the target extent
        /// </summary>
        public IFactory TargetFactory { get; set; }

        /// <summary>
        /// Gets or sets the name of the column containing the id
        /// </summary>
        public string OldIdColumn { get; set; }

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