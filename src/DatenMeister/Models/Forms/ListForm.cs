using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Forms
{
    public class ListForm : Form
    {
        public ListForm()
        {
        }

        public ListForm(string name) : base(name)
        {
        }

        public ListForm(string name, params FieldData[] fieldsToBeAdded) : base(name, fieldsToBeAdded)
        {
        }

        public string property { get; set; }

        public IElement metaClass { get; set; }
        
        public bool noItemsWithMetaClass { get; set; }

        /// <summary>
        /// Gets or sets a value whether new values shall be allowed
        /// </summary>
        public bool inhibitNewItems { get; set; }

        /// <summary>
        ///     Stores an enumeration of default types that can be used for creation
        /// </summary>
        public IList<DefaultTypeForNewElement> defaultTypesForNewElements { get; set; }

        /// <summary>
        /// Gets an enumeration of fast view filters
        /// </summary>
        public IList<IElement> fastViewFilters { get; set; }
    }

    /// <summary>
    /// Gets or sets a structure defining the type of the new element
    /// but also the property to which the new element is associated
    /// </summary>
    public class DefaultTypeForNewElement
    {
        public IElement metaClass { get; set; }

        public string parentProperty { get; set; }
    }
}