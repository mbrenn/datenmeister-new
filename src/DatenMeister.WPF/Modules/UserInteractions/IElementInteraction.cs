using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.UserInteractions;

/// <summary>
/// Defines a specific element interaction
/// </summary>
public interface IElementInteraction
{
    /// <summary>
    /// Gets the name of the interaction
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// DEfines the action that will be performed, if the user wants to activate the action
    /// </summary>
    /// <param name="navigationGuest">Defines the navigation guest which triggered the
    /// request for execution</param>
    /// <param name="element">Element on which the action shall be performed</param>
    /// <param name="parameters">Parameters of the action. Null, if there are no parameter</param>
    void Execute(INavigationGuest navigationGuest, IObject element, IObject? parameters);
}