namespace DatenMeister.WPF.Forms.Base.GridControl
{
    public class ColumnInstantiation
    {
        public double PositionOffset { get; set; }
            
        public double Width { get; set; }
            
        /// <summary>
        /// Gets or sets the desired width of the column
        /// </summary>
        public double DesiredWidth { get; set; }
            
        /// <summary>
        /// Gets or sets the desired height of the column
        /// </summary>
        public double DesiredHeight { get; set; }

        public GridColumnDefinition? ColumnDefinition { get; set; }
    }
}