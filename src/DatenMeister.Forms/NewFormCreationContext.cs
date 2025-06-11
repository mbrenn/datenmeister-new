using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms;

/// <summary>
/// A configuration being used for the form factories to define the behavior of how to
/// create a form upon a certain request
/// </summary>
public record NewFormCreationContext
{
    public class GlobalContext
    {
        /// <summary>
        /// Defines a list of collection form factories which have to be parsed through
        /// </summary>
        public List<INewCollectionFormFactory> CollectionFormFactories { get; } = [];
        
        /// <summary>
        /// Defines a list of table form factories which have to be parsed through
        /// </summary>
        public List<INewTableFormFactory> TableFormFactories { get; } = [];
        
        /// <summary>
        /// Defines a list of table object factories which have to be parsed through
        /// </summary>
        public List<INewObjectFormFactory> ObjectFormFactories { get; } = [];
        
        /// <summary>
        /// Defines a list of table object factories which have to be parsed through
        /// </summary>
        public List<INewFieldFactory> FieldFormFactories { get; } = [];

        /// <summary>
        /// Gets the scope storage which allows the transfer of information between the different handlers
        /// </summary>
        public ScopeStorage ScopeStorage { get; } = new(); 
        
        /// <summary>
        /// Defines the factory to be used to create the items
        /// </summary>
        public required IFactory Factory { get; init; }
    }

    public ScopeStorage ScopeStorage { get; } = new();

    public required GlobalContext Global { get; init; }
    
    /// <summary>
    /// Gets or sets whether all fields shall be generated as a read-only field
    /// </summary>
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// Gets or sets the id of the view mode being used for create or find the best form
    /// </summary>
    public string ViewModeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the information whether the created form is just for a tableform.
    /// If that this is the case, then no fields like SubElementField will be created
    /// </summary>
    public bool IsForTableForm { get; set; }
}
