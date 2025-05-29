namespace DatenMeister.WPF.Controls.GridControl;

/// <summary>
/// Gets the column definition
/// </summary>
public class GridColumnDefinition
{
    /// <summary>
    /// Gets or sets the desired size of the Column definition for the content.
    /// </summary>
    public double DesiredWidth { get; set; }
            
    public string Title { get; set; } = string.Empty;
}