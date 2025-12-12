using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.Excel.Models;

public class _ExcelModels
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Excel.Models.Workbook",
        TypeKind = TypeKind.ClassTree)]
    public class _Workbook
    {
        public static readonly string @tables = "tables";
        public IElement? @_tables = null;

    }

    public _Workbook @Workbook = new _Workbook();
    public MofObjectShadow @__Workbook = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Excel.Models.Workbook");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Excel.Models.Table",
        TypeKind = TypeKind.ClassTree)]
    public class _Table
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @items = "items";
        public IElement? @_items = null;

    }

    public _Table @Table = new _Table();
    public MofObjectShadow @__Table = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Excel.Models.Table");

    public static readonly _ExcelModels TheOne = new _ExcelModels();

}

