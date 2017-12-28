using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Provider;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.EMOF
{
    public class ExcelExtent : IProvider
    {
        private readonly _ExcelModels models = new _ExcelModels();

        private readonly XSSFWorkbook _workbook;
        public ExcelSettings Settings { get; }

        public ExcelExtent(XSSFWorkbook workbook, ExcelSettings settings)
        {
            _workbook = workbook;
            Settings = settings ?? new ExcelSettings();
        }

        /// <inheritdoc />
        public IProviderObject CreateElement(string metaClassUri)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool DeleteElement(string id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IProviderObject Get(string id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<IProviderObject> GetRootObjects()
        {
            for (var n = 0; n < _workbook.NumberOfSheets; n++)
            {
                var sheet = _workbook.GetSheetAt(n);
                yield return new SheetItem(this, sheet);
            }
        }

        /// <inheritdoc />
        public IEnumerable<IProviderObject> GetAllObjects()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void AddElement(IProviderObject valueAsObject, int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DeleteAllElements()
        {
            throw new NotImplementedException();
        }
    }
}