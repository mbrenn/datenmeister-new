using System.Dynamic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace StundenMeister.Logic
{
    public class StundenMeisterData
    {
        /// <summary>
        /// Singleton for the data
        /// </summary>
        public static StundenMeisterData TheOne { get; } = new StundenMeisterData();
        
        /// <summary>
        /// Stores the UML class for the cost center
        /// </summary>
        public IElement ClassCostCenter { get; set; }
        
        /// <summary>
        /// Stores the UML Class for the time recording
        /// </summary>
        public IElement ClassTimeRecording { get; set; }
        
        /// <summary>
        /// Gets the extent with the data
        /// </summary>
        public IUriExtent Extent { get; set; }
        
        
        /// <summary>
        /// Gets the Recording Element which is reflecting the current time
        /// </summary>
        public IElement CurrentTimeRecording { get; set; }
    }
}