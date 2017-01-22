using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Provider;
using NPOI.SS.UserModel;

namespace DatenMeister.Excel.EMOF
{
    public class RowItem : IProviderObject
    {
        public IProvider Provider => SheetItem.Provider;

        public SheetItem SheetItem { get; }

        /// <inheritdoc />
        public string MetaclassUri { get; set; }

        public int Row { get; }

        public RowItem(SheetItem sheetItem, int row)
        {
            SheetItem = sheetItem;
            Row = row;
        }

        /// <inheritdoc />
        public bool IsPropertySet(string property)
        {
            return SheetItem.Columns.ContainsKey(property);
        }

        /// <inheritdoc />
        public object GetProperty(string property)
        {
            int column;
            if (SheetItem.Columns.TryGetValue(property, out column))
            {
                return SheetItem.Sheet.GetRow(Row)?.GetCell(column)?.GetStringContent();
            }

            return null;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetProperties()
        {
            return SheetItem.Columns.Keys.ToList();
        }

        /// <inheritdoc />
        public bool DeleteProperty(string property)
        {
            int column;
            if (SheetItem.Columns.TryGetValue(property, out column))
            {
                SheetItem.Sheet.GetRow(Row)?.GetCell(column)?.SetCellValue(string.Empty);
                SheetItem.Sheet.GetRow(Row)?.GetCell(column)?.SetCellType(CellType.Blank);
            }

            return true;
        }

        /// <inheritdoc />
        public void SetProperty(string property, object value)
        {
            int column;
            if (SheetItem.Columns.TryGetValue(property, out column))
            {
                SheetItem.Sheet.GetRow(Row)?.GetCell(column)?.SetCellValue(value.ToString());
            }
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

/*

        public bool @equals(object other)
        {
            var otherAsRowItem = other as RowItem;
            return otherAsRowItem != null &&
                   otherAsRowItem.Row == Row &&
                   SheetItem.Equals(otherAsRowItem.SheetItem);
        }

        public object get(string property)
        {
            int column;
            if (SheetItem.Columns.TryGetValue(property, out column))
            {
                return SheetItem.Value.GetRow(Row)?.GetCell(column)?.GetStringContent();
            }

            return null;
        }

        public void set(string property, object value)
        {
            int column;
            if (SheetItem.Columns.TryGetValue(property, out column))
            {
                SheetItem.Value.GetRow(Row)?.GetCell(column)?.SetCellValue(value.ToString());
            }
        }

        public bool isSet(string property)
        {
            return SheetItem.Columns.ContainsKey(property);
        }

        public void unset(string property)
        {
            int column;
            if (SheetItem.Columns.TryGetValue(property, out column))
            {
                SheetItem.Value.GetRow(Row)?.GetCell(column)?.SetCellValue(string.Empty);
                SheetItem.Value.GetRow(Row)?.GetCell(column)?.SetCellType(CellType.Blank);
            }
        }

        public IElement metaclass => _metaClass;

        public IElement getMetaClass()
        {
            return metaclass;
        }

        public IElement container()
        {
            return SheetItem;
        }

        /// <summary>
        ///     Returns an enumeration of all Properties which are currently set in the object
        /// </summary>
        /// <returns>Enumeration of all properties being set</returns>
        public IEnumerable<string> getPropertiesBeingSet()
        {
            return SheetItem.Columns.Keys.ToList();
        }*/

        /// <summary>
        /// Gets the id of a row
        /// </summary>
        public string Id
        {
            get
            {
                var firstPart = SheetItem.Sheet.SheetName;
                string secondPart;
                if (string.IsNullOrEmpty(SheetItem.Settings.IdColumn))
                {
                    secondPart = Row.ToString();
                }
                else
                {
                    secondPart = GetProperty(SheetItem.Settings.IdColumn).ToString();
                }

                return $"{firstPart}.{secondPart}";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public void EmptyListForProperty(string property)
        {
            throw new NotImplementedException();
        }
    }
}