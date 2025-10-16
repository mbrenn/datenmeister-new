using DatenMeister.Core;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.FormFinder;

namespace DatenMeister.Forms;

/// <summary>
/// A configuration being used for the form factories to define the behavior of how to
/// create a form upon a certain request
/// </summary>
public class FormCreationContext
{
    public class GlobalContext
    {
        /// <summary>
        /// Defines a list of collection form factories which have to be parsed through
        /// </summary>
        public List<ICollectionFormFactory> CollectionFormFactories { get; } = [];
        
        /// <summary>
        /// Defines a list of table form factories which have to be parsed through
        /// </summary>
        public List<ITableFormFactory> TableFormFactories { get; } = [];
        
        /// <summary>
        /// Defines a list of table form factories which have to be parsed through
        /// </summary>
        public List<IRowFormFactory> RowFormFactories { get; } = [];
        
        /// <summary>
        /// Defines a list of table object factories which have to be parsed through
        /// </summary>
        public List<IObjectFormFactory> ObjectFormFactories { get; } = [];
        
        /// <summary>
        /// Defines a list of table object factories which have to be parsed through
        /// </summary>
        public List<IFieldFactory> FieldFormFactories { get; } = [];

        /// <summary>
        /// Gets the scope storage which allows the transfer of information between the different handlers
        /// </summary>
        public ScopeStorage ScopeStorage { get; } = new(); 
        
        /// <summary>
        /// Defines the factory to be used to create the items
        /// </summary>
        public required IFactory Factory { get; set; }
    }

    public ScopeStorage LocalScopeStorage { get; } = new();

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
    /// Clones the context and copies all properties, except the LocalScopeStorage
    /// </summary>
    /// <returns>The cloned instance</returns>
    public FormCreationContext Clone()
    {
        return new FormCreationContext
        {
            IsReadOnly = IsReadOnly,
            Global = Global,
            ViewModeId = ViewModeId
        };
    }

    public FormCreationContext SetReadOnly(bool isReadOnly)
    {
        IsReadOnly = isReadOnly;
        return this;
    }

    public FormCreationContext SetViewModeId(string viewModeId)
    {
        ViewModeId = viewModeId;
        return this;
    }
}


public static class FormCreationContextHelper
{
    
    /// <summary>
    /// Removes all FormFinders from the FormCreationContext, so only Auto-Generated items are added
    /// </summary>
    /// <param name="context">Context to be evaluated</param>
    public static void RemoveFormFinder(this FormCreationContext context)
    {
        context.Global.CollectionFormFactories.RemoveAll(x => x is FormFinderFactory);
        context.Global.RowFormFactories.RemoveAll(x => x is FormFinderFactory);
        context.Global.ObjectFormFactories.RemoveAll(x => x is FormFinderFactory);
        context.Global.TableFormFactories.RemoveAll(x => x is FormFinderFactory);
    }
}