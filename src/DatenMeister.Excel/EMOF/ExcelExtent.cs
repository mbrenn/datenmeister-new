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
        private readonly ExcelSettings _settings;

        public ExcelExtent(string url, XSSFWorkbook workbook, ExcelSettings settings)
        {
            /*_workbook = workbook;
            _settings = settings;

            // Maps the table to sheet item
            var typeMapping = AddMappingForType<SheetItem, ISheet>(
                models.__Table,
                x => x.Value.SheetName,
                x =>
                {
                    x.Settings = _settings;
                });

            typeMapping.AddProperty<SheetItem, string>(
                "name",
                x => x.Value.SheetName,
                (x, value) =>
                {
                    throw new NotImplementedException();
                });

            typeMapping.AddProperty<SheetItem, IReflectiveSequence>(
                "items",
                x =>
                {
                    var collection = new MMReflectiveCollection(this);

                    var n = x.RowOffset;
                    while (true)
                    {
                        var cell = x.Value.GetRow(n)?.GetCell(x.ColumnOffset);
                        if (string.IsNullOrEmpty(cell?.GetStringContent()))
                        {
                            break;
                        }

                        collection.add(new RowItem(x, n, null));
                        n++;
                    }

                    return collection;
                },
                (x, value) => { throw new InvalidOperationException(); });*/
        }

        public IReflectiveSequence elements()
        {
            /*var result = new MMReflectiveCollection(this);
            for (var n = 0; n < _workbook.NumberOfSheets; n++)
            {
                var sheet = _workbook.GetSheetAt(n);
                var element = (SheetItem) ConvertToElement(models.__Table, sheet);
                element.InitializeData();

                result.add(element);
            }

            return result;*/
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IProviderObject CreateElement(string metaClassUri)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DeleteElement(string id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IProviderObject Get(string id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<ElementReference> GetRootObjects()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<ElementReference> GetAllObjects()
        {
            throw new NotImplementedException();
        }
    }
}