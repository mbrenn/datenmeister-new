using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Provider.ManualMapping;
using NPOI.SS.UserModel;

namespace DatenMeister.Excel.EMOF
{
    public class SheetItem : MMElement<ISheet>
    {
        public SheetItem() : base(null)
        {
        }

        public SheetItem(IUriExtent localExtent) : base(localExtent)
        {
        }

        /// <summary>
        /// Gets or sets the columns and their names
        /// The name of the column mapping to the column
        /// </summary>
        public Dictionary<string, int> Columns { get; set; } = new Dictionary<string, int>();

        public ExcelSettings Settings { get; set; }

        /// <summary>
        /// First column where data can be found
        /// </summary>
        public int ColumnOffset { get; set; }

        /// <summary>
        /// First of data
        /// </summary>
        public int RowOffset { get; set; }

        /// <summary>
        /// Initializes data
        /// </summary>
        public void InitializeData()
        {
            var n = ColumnOffset;
            while (true)
            {
                var headline = Value.GetRow(RowOffset)?.GetCell(n)?.GetStringContent();
                if (string.IsNullOrEmpty(headline))
                {
                    break;
                }

                Columns[headline] = n;
                n++;
            }

            RowOffset++;
        }
    }
}
