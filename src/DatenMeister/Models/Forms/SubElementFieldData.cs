using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Defines a field to which subelements are allowed and to which new elements can be added
    /// </summary>
    public class SubElementFieldData : FieldData
    {
        /// <summary>
        /// Gets or sets the metaclass that will be created when the user creates a new instance
        /// </summary>
        public string? metaClassUri { get; set; }

        /// <summary>
        /// Gets or sets the fields being shown in the subelements. If null, the fields will be automatically generated.
        /// </summary>
        public Form? form { get; set; }

        /// <summary>
        /// Initializes a new element of the subelement field data and sets the field as an enumeration
        /// </summary>
        public SubElementFieldData() : base()
        {
            isEnumeration = true;
        }

        /// <summary>
        /// Initializes a new element of the subelement field data and sets the field as an enumeration
        /// </summary>
        /// <param name="name">Name of the field</param>
        /// <param name="title">Title of the field</param>
        public SubElementFieldData(string name, string title) : base(name, title)
        {
            isEnumeration = true;
        }

        /// <summary>
        /// Stores an enumeration of default types that can be used for creation
        /// </summary>
        public IList<IElement> defaultTypesForNewElements { get; set; }
    }
}