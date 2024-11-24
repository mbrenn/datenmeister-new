using System.Collections.Generic;

namespace DatenMeister.Json
{
    /// <summary>
    /// This class defines the structure of the MofObject interface between
    /// the webserver and the webclient when MofObjects are sent via Json.
    /// </summary>
    public class MofObjectAsJson
    {
        /// <summary>
        /// If the element can't be fully resolved, then this reference value
        /// will contain the url of the original object. 
        /// </summary>
        public string? r { get; set; } = string.Empty;
        
        /// <summary>
        /// If the element can't be fully resolved, then this reference value
        /// will contain the workspace of the original object 
        /// </summary>
        public string? w { get; set; }= string.Empty;
        
        /// <summary>
        /// Defines the values of the element
        /// </summary>
        public Dictionary<string, object> v { get; set; }= new();

        /// <summary>
        /// The metaclass of the element including some additional information
        /// </summary>
        public ItemWithNameAndId? m { get; set; }

        /// <summary>
        /// The uri of the element to retrieve additional information, 
        /// </summary>
        public string? u { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the id of the element
        /// </summary>
        public string? id { get; set; } = string.Empty;
    }
}