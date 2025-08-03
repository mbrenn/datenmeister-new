using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Controls.GridControl;

/// <summary>
/// Gets the column definition
/// </summary>
public class GridTextBlockColumnDefinition : GridColumnDefinition
{
    /// <summary>
    /// Gets or sets the field being used for the textblock
    /// </summary>
    public IElement? Field { get; set; }
}