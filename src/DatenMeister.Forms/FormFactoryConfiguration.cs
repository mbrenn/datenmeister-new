using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms
{
    /// <summary>
    /// A configuration being used for the form factories to define the behavior of how to
    /// create a form upon a certain request
    /// </summary>
    public record FormFactoryConfiguration
    {
        /// <summary>
        /// Allows the use of the form finder
        /// </summary>
        public bool ViaFormFinder { get; set; } = true;

        /// <summary>
        /// Allows the use of the automatic form creator
        /// </summary>
        public bool ViaFormCreator { get; set; } = true;

        /// <summary>
        /// Allows the call of the form modification plugins
        /// </summary>
        public bool AllowFormModifications { get; set; } = true;

        /// <summary>
        /// Allows the automatic creation of the metaclass field at the end of the form
        /// </summary>
        public bool AutomaticMetaClassField { get; set; } = true;

        /// <summary>
        /// Gets or sets whether all fields shall be generated as a read-only field
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a flag which indicates that for a collection only these properties shall be
        /// included which are in common between the elements. This reduces the number of
        /// columns
        /// </summary>
        public bool IncludeOnlyCommonProperties { get; set; }

        /// <summary>
        /// Gets or sets the property whether the element shall be also created by the metaclass information
        /// of the properties
        /// </summary>
        public bool CreateByMetaClass { get; set; } = true;

        /// <summary>
        /// Gets or sets the value whether the element shall be created by the properties being
        /// actually set.  
        /// </summary>
        public bool CreateByPropertyValues { get; set; } = true;

        /// <summary>
        /// Gets or sets the value whether the element shall be created by the properties being
        /// actually set only if the metaclass itself was not found
        /// </summary>
        public bool OnlyCreateByValuesWhenMetaClassIsNotSet { get; set; } = false;

        /// <summary>
        /// Gets or sets the id of the view mode being used for create or find the best form
        /// </summary>
        public string ViewModeId { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the information whether the created form is just for a tableform.
        ///     If that this is the case, then no fields like SubElementField will be created
        /// </summary>
        public bool IsForTableForm { get; set; }

        public static FormFactoryConfiguration CreateByMetaClassOnly =>
            new() {AutomaticMetaClassField = false, CreateByPropertyValues = false};

        /// <summary>
        /// Defines the factory to be used to create the items
        /// </summary>
        public IFactory? Factory { get; set; }
    }
}