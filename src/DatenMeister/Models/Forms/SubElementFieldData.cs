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
        public IElement? metaClass { get; set; }

        /// <summary>
        /// Gets or sets the fields being shown in the subelements. If null, the fields will be automatically generated.
        /// </summary>
        public Form? form { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether only existing elements are allowed.
        /// If the value is set, then the use cannot create new elements.
        /// This should be set, if the associated element is not a composite
        /// </summary>
        public bool allowOnlyExistingElements { get; set; }

        /// <summary>
        /// Initializes a new element of the subelement field data and sets the field as an enumeration
        /// </summary>
        public SubElementFieldData()
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
        public IList<IElement>? defaultTypesForNewElements { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether specializations shall also be included
        /// to the user as option to get directly created
        /// </summary>
        public bool includeSpecializationsForDefaultTypes { get; set; }

        /// <summary>
        /// Gets or sets the name of the workspace that shall be preselected, if the user clicks on 'New Type'
        /// </summary>
        public string defaultWorkspaceOfNewElements { get; set; } = string.Empty;
        

        /// <summary>
        /// Gets or sets the name of the extent that shall be preselected, if the user clicks on 'New Type'
        /// </summary>
        public string defaultExtentOfNewElements { get; set; } = string.Empty;
    }
}