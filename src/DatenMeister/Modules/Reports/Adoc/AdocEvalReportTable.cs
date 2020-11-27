using System.Collections.Generic;
using DatenMeister.Modules.Reports.Generic;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocEvalReportTable : GenericReportTable<AdocGenericReportCreator>
    {
        public override void StartTable(AdocGenericReportCreator reportCreator, string cssClass)
        {
            reportCreator.TextWriter.WriteLine("[%header]");
            reportCreator.TextWriter.WriteLine("|===");
        }

        public override void EndTable(AdocGenericReportCreator reportCreator)
        {
            reportCreator.TextWriter.WriteLine("|===");
            reportCreator.TextWriter.WriteLine(string.Empty);
        }

        public override void WriteColumnHeader(AdocGenericReportCreator reportCreator, IEnumerable<TableCellHeader> cellHeaders)
        {
            foreach (var header in cellHeaders)
            {
                reportCreator.TextWriter.Write("|" + header.ColumnName);
            }
        }

        public override void WriteRow(AdocGenericReportCreator reportCreator, IEnumerable<TableCellContent> cellContents)
        {
            foreach (var field in cellContents)
            {
                reportCreator.TextWriter.Write("|" + field.Content);
            }

            reportCreator.TextWriter.WriteLine(string.Empty);
        }
    }
}