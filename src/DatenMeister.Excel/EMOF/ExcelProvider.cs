using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider;
using DatenMeister.Excel.Helper;
using DatenMeister.Provider;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.EMOF
{
    public class ExcelProvider : IProvider
    {
        private readonly XSSFWorkbook _workbook;
        
        public IElement Settings { get; }
        
        public ExcelColumnTranslator ColumnTranslator { get; } = new ExcelColumnTranslator();

        public ExcelProvider(XSSFWorkbook workbook, IElement settings)
        {
            _workbook = workbook;
            Settings = settings;
            ColumnTranslator.LoadTranslation(settings);
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
        public void AddElement(IProviderObject valueAsObject, int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DeleteAllElements()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the capabilities of the provider
        /// </summary>
        /// <returns></returns>
        public ProviderCapability GetCapabilities()
        {
            return ProviderCapabilities.None;
        }
    }
}