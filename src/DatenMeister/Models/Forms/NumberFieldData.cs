namespace DatenMeister.Models.Forms
{
    public class NumberFieldData : FieldData
    {
        public string? format { get; set; }
        
        /// <summary>
        /// Gets or sets the value whether the number is an integer number
        /// </summary>
        public bool isInteger { get; set; }
    }
}