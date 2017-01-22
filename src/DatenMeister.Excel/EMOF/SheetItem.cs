using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Provider;
using NPOI.SS.UserModel;

namespace DatenMeister.Excel.EMOF
{
    public class SheetItem : IProviderObject
    {
        private readonly ExcelExtent _extent;
        public ISheet Sheet { get; set; }

        /// <summary>
        /// Gets the provider being used to create the element
        /// </summary>
        public IProvider Provider { get; }

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
        /// Initializes a new instance of the SheetItem
        /// </summary>
        /// <param name="extent">Extent to which the item belongs</param>
        /// <param name="sheet">The sheet that is used for access</param>
        public SheetItem(ExcelExtent extent, ISheet sheet)
        {
            _extent = extent;
            Sheet = sheet;

            InitializeData();
        }

        /// <summary>
        /// Initializes data
        /// </summary>
        private void InitializeData()
        {
            var n = ColumnOffset;
            while (true)
            {
                var headline = Sheet.GetRow(RowOffset)?.GetCell(n)?.GetStringContent();
                if (string.IsNullOrEmpty(headline))
                {
                    break;
                }

                Columns[headline] = n;
                n++;
            }

            RowOffset++;
        }

        /// <inheritdoc />
        public bool IsPropertySet(string property)
        {
            return property == "items" || property == "name";
        }

        /// <inheritdoc />
        public object GetProperty(string property)
        {
            if (property == "items")
            {
                var collection = new List<object>();

                var n = RowOffset;
                while (true)
                {
                    var cell = Sheet.GetRow(n)?.GetCell(ColumnOffset);
                    if (string.IsNullOrEmpty(cell?.GetStringContent()))
                    {
                        break;
                    }

                    collection.Add(new RowItem(this, n));
                    n++;
                }

                return collection;
            }

            if (property == "name")
            {
                return Sheet.SheetName;
            }

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<string> GetProperties()
        {
            return new[] {"items", "name"};
        }

        /// <inheritdoc />
        public bool DeleteProperty(string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void SetProperty(string property, object value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool AddToProperty(string property, object value, int index)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool RemoveFromProperty(string property, object value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string MetaclassUri { get; set; }

        /// <inheritdoc />
        public void EmptyListForProperty(string property)
        {
            throw new System.NotImplementedException();
        }
    }
}
