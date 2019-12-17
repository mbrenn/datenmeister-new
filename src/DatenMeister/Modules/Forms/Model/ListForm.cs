﻿using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.Forms.Model
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
        /// Gets or sets a value whether the delete button shall be shown. 
        /// </summary>
        public bool inhibitDeleteItems { get; set; }

        /// <summary>
        ///     Stores an enumeration of default types that can be used for creation
        /// </summary>
        public IList<DefaultTypeForNewElement> defaultTypesForNewElements { get; set; }

        /// <summary>
        /// Gets an enumeration of fast view filters
        /// </summary>
        public IList<IElement> fastViewFilters { get; set; }
    }
}