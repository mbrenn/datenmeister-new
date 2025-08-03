using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Excel.Models;

public class ExcelModels
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Excel.Models.Workbook",
        TypeKind = TypeKind.WrappedClass)]
    public class Workbook_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // Not found
        public object? @tables
        {
            get =>
                innerDmElement.getOrDefault<object?>("tables");
            set => 
                innerDmElement.set("tables", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Excel.Models.Table",
        TypeKind = TypeKind.WrappedClass)]
    public class Table_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // Not found
        public object? @name
        {
            get =>
                innerDmElement.getOrDefault<object?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        // Not found
        public object? @items
        {
            get =>
                innerDmElement.getOrDefault<object?>("items");
            set => 
                innerDmElement.set("items", value);
        }

    }

}

