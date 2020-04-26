using System.Security.RightsManagement;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Modules.ReportManager
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
    /// Defines the configuration
    /// </summary>
    public class ReportConfiguration
    {
        /// <summary>
        /// Gets or sets the flags which show also the descendents
        /// </summary>
        public bool showDescendents { get; set; }

        /// <summary>
        /// Defines the root element which is used as an anchor to get shown
        /// </summary>
        public IElement? rootElement { get; set; }

        /// <summary>
        /// Gets or sets the properties of the root elements
        /// </summary>
        public bool showRootElement { get; set; }
        
        /// <summary>
        /// Gets or sets the extent form being used for the given element.
        /// The type of the extent is the extent form 
        /// </summary>
        public IElement? form { get; set; }
    }
}