using System.Collections.Generic;
using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Adoc
{
    public class AdocEvalReportTable : GenericReportTable<AdocReportCreator>
    {
        public override void StartTable(AdocReportCreator reportCreator, string cssClass)
        {
            reportCreator.TextWriter.WriteLine("[%header]");
            reportCreator.TextWriter.WriteLine("|===");
        }

        public override void EndTable(AdocReportCreator reportCreator)
        {
            reportCreator.TextWriter.WriteLine("|===");
            reportCreator.TextWriter.WriteLine(string.Empty);
        }

        public override void WriteColumnHeader(AdocReportCreator reportCreator, IEnumerable<TableCellHeader> cellHeaders)
        {
            foreach (var header in cellHeaders)
            {
                reportCreator.TextWriter.Write("|" + header.ColumnName);
            }

            reportCreator.TextWriter.WriteLine(string.Empty);
        }

        public override void WriteRow(AdocReportCreator reportCreator, IEnumerable<TableCellContent> cellContents)
        {
            foreach (var field in cellContents)
            {
                reportCreator.TextWriter.Write("|" + field.Content);
            }

            reportCreator.TextWriter.WriteLine(string.Empty);
        }
    }
}