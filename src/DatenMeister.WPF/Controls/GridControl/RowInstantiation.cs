using System.Windows.Media;

namespace DatenMeister.WPF.Controls.GridControl;

public class RowInstantiation
{
    /// <summary>
    /// Defines the height of the row
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Defines the Offset Position of the top boundary of the cell excluding the padding
    /// of the cell itself
    /// </summary>
    public double OffsetHeight { get; set; }
            
    /// <summary>
    /// Defines the background color of the row
    /// </summary>
    public Brush? BackgroundColor { get; set; }

    /// <summary>
    /// Defines the cell instantiations containing the UI element
    /// </summary>
    public List<CellInstantiation> Cells { get; } = new();
        
    /// <summary>
    /// Gets or sets the index in the data. -1 if not an index 
    /// </summary>
    public int IndexInData { get; set; }
}