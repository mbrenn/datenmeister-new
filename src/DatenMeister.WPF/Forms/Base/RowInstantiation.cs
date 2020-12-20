using System.Collections.Generic;
using System.Windows.Media;

namespace DatenMeister.WPF.Forms.Base.GridControl
{
    public class RowInstantiation
    {
        public double Height { get; set; }

        public double PositionOffset { get; set; }
            
        public Brush? BackgroundColor { get; set; }

        public List<CellInstantiation> Cells { get; } = new List<CellInstantiation>();
    }
}