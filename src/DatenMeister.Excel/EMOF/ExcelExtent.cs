using System;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.ManualMapping;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.EMOF
{
    public class ExcelExtent : MMUriExtent
    {
        private readonly _ExcelModels models = new _ExcelModels();

        private readonly XSSFWorkbook _workbook;

        public ExcelExtent(string url, XSSFWorkbook workbook) : base (url)
        {
            _workbook = workbook;

            // Maps the table to sheet item
            var typeMapping = AddMappingForType<SheetItem, ISheet>(models.__Table);
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
                    var collection = new MMReflectiveCollection();

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
                (x, value) => { throw new InvalidOperationException(); });
        }

        public override IReflectiveSequence elements()
        {
            var result = new MMReflectiveCollection();
            for (var n = 0; n < _workbook.NumberOfSheets; n++)
            {
                var sheet = _workbook.GetSheetAt(n);
                var element = ConvertToElement(models.__Table, sheet) as SheetItem;
                element.InitializeData();

                result.add(element);
            }

            return result;
        }
    }
}