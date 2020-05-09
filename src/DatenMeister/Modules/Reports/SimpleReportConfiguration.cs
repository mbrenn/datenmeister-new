using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.Reports
{
    public enum DescendentMode
    {
        /// <summary>
        /// Shows no descendents
        /// </summary>
        None,
        /// <summary>
        /// Shows the descendents as inline
        /// </summary>
        Inline,
        /// <summary>
        /// Shows them in separate chapters per Pckage
        /// </summary>
        PerPackage
    }

    /// <summary>
    /// Defines the mode in which the elements shall be shown
    /// </summary>
    public enum ReportTableForTypeMode
    {

        /// <summary>
        /// A table shall be created per type
        /// </summary>
        PerType, 

        /// <summary>
        /// All types in one huge table
        /// </summary>
        AllTypes,

    }
    
    /// <summary>
    /// Defines the configuration
    /// </summary>
    public class SimpleReportConfiguration
    {
        /// <summary>
        /// Gets or sets the flags which show also the descendents
        /// </summary>
        public bool showDescendents { get; set; }

        /// <summary>
        /// Defines the root element which is used as an anchor to get shown
        /// </summary>
        public IObject? rootElement { get; set; }

        /// <summary>
        /// Gets or sets the properties of the root elements
        /// </summary>
        public bool showRootElement { get; set; }
        
        /// <summary>
        /// Gets or sets the flag whether metaclass shall be shown in the columns and rows
        /// </summary>
        public bool showMetaClasses { get; set; }
        
        /// <summary>
        /// Gets or sets a flag indicating whether the full name shall be added
        /// </summary>
        public bool showFullName { get; set; }
        
        /// <summary>
        /// Gets or sets the extent form being used for the given element.
        /// The type of the extent is the extent form.
        /// </summary>
        public IObject? form { get; set; }

        /// <summary>
        /// Gets or sets the mode how the elements shall be shown in the overview
        /// </summary>
        public DescendentMode descendentMode { get; set; }

        /// <summary>
        /// Gets or sets the type mode being used
        /// </summary>
        public ReportTableForTypeMode typeMode { get; set; }
    }
}