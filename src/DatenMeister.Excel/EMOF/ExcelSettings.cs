namespace DatenMeister.Excel.EMOF
{
    public class ExcelSettings
    {
        /// <summary>
        /// Gets or sets the column containing the id of the elements. 
        /// If this column is not set, the id will be generated as a unique id
        /// </summary>
        public string IdColumn { get; set; }
    }
}