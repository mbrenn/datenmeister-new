﻿using System;
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

        public ExcelExtent(XSSFWorkbook workbook, ExcelSettings settings)
        {
            _workbook = workbook;
            _settings = settings;

            /*// Maps the table to sheet item
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