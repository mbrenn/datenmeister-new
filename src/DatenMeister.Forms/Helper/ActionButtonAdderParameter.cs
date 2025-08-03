using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms.Helper;

/// <summary>
/// Defines the parameter for the ActionButtonToFormadder
/// </summary>
public class ActionButtonAdderParameter
{
    public ActionButtonAdderParameter(string actionName, string title, string buttonText = "")
    {
        ActionName = actionName;
        Title = title;
        ButtonText = buttonText;
    }

    public string ActionName { get; set; }

    public string Title { get; set; }

    public string ButtonText { get; set; }

    /// <summary>
    /// Gets the dictionary of parameters being used to give additional information to the action buttons.
    /// These parameter will be moved to the client within the action button information as subelement
    /// </summary>
    public Dictionary<string, string> Parameter { get; } = new();

    /// <summary>
    /// Gets or sets the position at which the button shall be added.
    /// -1, if the button shall be set at the end of the table. 
    /// </summary>
    public int ActionButtonPosition { get; set; } = -1;

    /// <summary>
    /// Gets or sets a predicate that can be used as an additional filtering option.
    /// If the method is not set, the element will be considered as fitting.
    /// If the element is set, then the predicate must return true, to add the filter element 
    /// </summary>
    public Func<IObject?, bool>? PredicateForElement { get; set; }
}

public class ActionButtonAdderParameterForRow : ActionButtonAdderParameter
{
    public ActionButtonAdderParameterForRow(
        string actionName,
        string title,
        string buttonText = "") : base(actionName, title, buttonText)
    {
    }

    public Predicate<RowFormFactoryParameter> PredicateForParameter { get; set; } = _ => true;

    /// <summary>
    /// Gets or sets the delegate that will be called, when the 
    /// the filter is evaluated. This allows setting a breakpoint for the debugger
    /// during issue finding
    /// </summary>
    public Action<RowFormFactoryParameter>? OnCall { get; set; }
        
    /// <summary>
    /// Gets or sets the delegate that will be called, when the 
    /// the filter is evaluated and a match has been figured out.
    /// This allows setting a breakpoint for the debugger during issue finding
    /// </summary>
    public Action<RowFormFactoryParameter>? OnCallSuccess { get; set; }
}

public class ActionButtonAdderParameterForTable : ActionButtonAdderParameter
{
    public ActionButtonAdderParameterForTable(
        string actionName,
        string title,
        string buttonText = "") : base(actionName, title, buttonText)
    {
    }

    public Predicate<TableFormFactoryParameter> PredicateForParameter { get; set; } = _ => true;

    /// <summary>
    /// Gets or sets the delegate that will be called, when the 
    /// the filter is evaluated. This allows setting a breakpoint for the debugger
    /// during issue finding
    /// </summary>
    public Action<TableFormFactoryParameter>? OnCall { get; set; }
        
    /// <summary>
    /// Gets or sets the delegate that will be called, when the 
    /// the filter is evaluated and a match has been figured out.
    /// This allows setting a breakpoint for the debugger during issue finding
    /// </summary>
    public Action<TableFormFactoryParameter>? OnCallSuccess { get; set; }
}