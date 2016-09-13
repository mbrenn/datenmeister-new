using System;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
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

            var typeMapping = AddMappingForType<SheetItem, ISheet>(models.__Table);
            typeMapping.AddProperty<SheetItem, string>(
                "name",
                (x) => x.GetName(),
                (x, value) =>
                {
                    throw new NotImplementedException();
                });
            ;
        }

        public override IReflectiveSequence elements()
        {
            var result = new MMReflectiveCollection();
            for (var n = 0; n < _workbook.NumberOfSheets; n++)
            {
                var sheet = _workbook.GetSheetAt(n);
                var element = ConvertToElement(models.__Table, sheet);

                result.add(element);
            }

            return result;
        }
    }
}