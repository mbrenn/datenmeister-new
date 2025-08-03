namespace DatenMeister.WPF.Controls.GridControl;

public class ColumnInstantiation
{
    /// <summary>
    /// Defines the Offset Position of the left boundary of the cell excluding the padding
    /// of the cell itself
    /// </summary>
    public double OffsetWidth { get; set; }
            
    public double Width { get; set; }
            
    /// <summary>
    /// Gets or sets the desired width of the column. This width is evaluated out of the size of the
    /// returned UI element
    /// </summary>
    public double DesiredWidth { get; set; }
            
    /// <summary>
    /// Gets or sets the desired height of the column
    /// </summary>
    public double DesiredHeight { get; set; }

    public GridColumnDefinition? ColumnDefinition { get; set; }
}