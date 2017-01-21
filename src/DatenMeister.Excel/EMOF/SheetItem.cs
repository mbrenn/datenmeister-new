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
        /// <summary>
        /// Gets the provider being used to create the element
        /// </summary>
        public IProvider Provider { get; }

        public SheetItem() 
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

        public SheetItem(ExcelExtent provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// Initializes data
        /// </summary>
        public void InitializeData()
        {/*
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
            */
        }

        /// <inheritdoc />
        public bool IsPropertySet(string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public object GetProperty(string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<string> GetProperties()
        {
            throw new System.NotImplementedException();
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
