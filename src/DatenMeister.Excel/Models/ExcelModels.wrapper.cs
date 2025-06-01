using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Excel.Models
{
    public class ExcelModels
    {
        public class Workbook_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @tables
            {
                get =>
                    innerDmElement.get("tables");
                set => 
                    innerDmElement.set("tables", value);
            }

        }

        public class Table_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @name
            {
                get =>
                    innerDmElement.get("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public object? @items
            {
                get =>
                    innerDmElement.get("items");
                set => 
                    innerDmElement.set("items", value);
            }

        }

    }

}
