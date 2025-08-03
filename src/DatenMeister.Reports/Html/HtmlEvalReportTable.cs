using DatenMeister.HtmlEngine;
using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Html;

public class HtmlReportTable : GenericReportTable<HtmlReportCreator>
{
    private HtmlTable? _table;

    public override void StartTable(HtmlReportCreator reportCreator, string cssClass)
    {
        _table = new HtmlTable
        {
            CssClass = cssClass
        };
    }

    public override void EndTable(HtmlReportCreator reportCreator)
    {
        if (_table == null) throw new InvalidOperationException(nameof(_table) + " is null");

        reportCreator.HtmlReporter.Add(_table);
    }

    public override void WriteColumnHeader(HtmlReportCreator reportCreator, IEnumerable<TableCellHeader> cellHeaders)
    {
        if (_table == null) throw new InvalidOperationException(nameof(_table) + " is null");
            
        var cells = new List<HtmlTableCell>();
        foreach (var field in cellHeaders)
        {
            cells.Add(
                new HtmlTableCell(field.ColumnName)
                {
                    IsHeading = true
                });
        }

        _table.AddRow(new HtmlTableRow(cells));
    }

    public override void WriteRow(HtmlReportCreator reportCreator, IEnumerable<TableCellContent> cellContents)
    {
        if (_table == null) throw new InvalidOperationException(nameof(_table) + " is null");

        var cells = new List<HtmlTableCell>();
        foreach (var field in cellContents)
        {
            var cell = new HtmlTableCell(
                field.Content,
                field.CssClass);
            cells.Add(cell);
        }

        _table.AddRow(new HtmlTableRow(cells));
    }
}