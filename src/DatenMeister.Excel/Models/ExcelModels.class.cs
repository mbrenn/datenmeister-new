using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Excel
{
    public class _ExcelModels
    {
        public class _Workbook
        {
            public static string @tables = "tables";
            public IElement _tables = null;

        }

        public _Workbook @Workbook = new _Workbook();
        public IElement @__Workbook = new MofObjectShadow("dm:///DatenMeister/Types/Excel#DatenMeister.Excel.Models.Workbook");

        public class _Table
        {
            public static string @name = "name";
            public IElement _name = null;

            public static string @items = "items";
            public IElement _items = null;

        }

        public _Table @Table = new _Table();
        public IElement @__Table = new MofObjectShadow("dm:///DatenMeister/Types/Excel#DatenMeister.Excel.Models.Table");

        public static _ExcelModels TheOne = new _ExcelModels();

    }

}
