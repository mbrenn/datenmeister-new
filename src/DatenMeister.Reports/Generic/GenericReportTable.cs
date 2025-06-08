using System.Globalization;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormCreator;
using DatenMeister.Forms.FormFactory;
using DatenMeister.TextTemplates;

namespace DatenMeister.Reports.Generic;

public abstract class GenericReportTable<T> : IGenericReportEvaluator<T> where T : GenericReportCreator
{
    public class TableCellHeader
    {
        public string ColumnName { get; set; } = string.Empty;
    }
        
    public class TableCellContent { 
        public string Content { get; set; } = string.Empty;

        public string CssClass { get; set; } = string.Empty;
    }

    public bool IsRelevant(IElement element)
    {
        var metaClass = element.getMetaClass();
        return metaClass?.equals(_Reports.TheOne.Elements.__ReportTable) == true;
    }

    /// <summary>
    /// Starts the table and gives the cssClass
    /// </summary>
    /// <param name="reportCreator">Report ReportCreator to be used</param>
    /// <param name="cssClass">CssClass defining the style</param>
    public abstract void StartTable(T reportCreator, string cssClass);

    /// <summary>
    /// Ends the table after all column headers and rows are added
    /// </summary>
    /// <param name="reportCreator">Report ReportCreator To be used</param>
    public abstract void EndTable(T reportCreator);

    /// <summary>
    /// Writes a row containing the column headers. 
    /// </summary>
    /// <param name="reportCreator">Report ReportCreator To be used</param>
    /// <param name="cellHeaders">Enumeration of cell header definitions</param>
    public abstract void WriteColumnHeader(T reportCreator, IEnumerable<TableCellHeader> cellHeaders);

    /// <summary>
    /// Writes a row with the columns
    /// </summary>
    /// <param name="reportCreator">Report ReportCreator To be used</param>
    /// <param name="cellContents"></param>
    public abstract void WriteRow(T reportCreator, IEnumerable<TableCellContent> cellContents);

    public void Evaluate(ReportLogic reportLogic, T reportCreator, IElement reportNode)
    {
        var viewNode =
            ReportLogic.GetViewNode(
                reportNode,
                _Reports._Elements._ReportTable.viewNode);

        var form = reportNode.getOrDefault<IElement>(_Reports._Elements._ReportTable.form);

        var dataviewEvaluation = reportLogic.GetDataViewEvaluation();
        var elements = dataviewEvaluation.GetElementsForViewNode(viewNode);

        // Find form 
        if (form == null)
        {
            // Create form
            var formCreator = new TableFormFactory(reportLogic.WorkspaceLogic, reportLogic.ScopeStorage);
            form = formCreator.CreateTableFormForCollection(elements, new FormFactoryConfiguration());
        }

        // Creates the table
        var cssClass = reportNode.getOrDefault<string>(_Reports._Elements._ReportTable.cssClass);
        StartTable(reportCreator, cssClass);

        var cellHeaders = new List<TableCellHeader>();
        var fields = form.get<IReflectiveCollection>(_Forms._TableForm.field);
        foreach (var field in fields.OfType<IElement>())
        {
            cellHeaders.Add(
                new TableCellHeader
                {
                    ColumnName = field.getOrDefault<string>(_Forms._FieldData.title)
                });
        }

        WriteColumnHeader(reportCreator, cellHeaders);

        foreach (var listElement in elements.OfType<IElement>())
        {
            var cellContent = new List<TableCellContent>();

            foreach (var field in fields.OfType<IElement>())
            {
                cellContent.Add(CreateCellForField(listElement, field));
            }

            WriteRow(reportCreator, cellContent);
        }

        EndTable(reportCreator);
    }
    /// <summary>
    /// Creates the cell for a specific element and field
    /// </summary>
    /// <param name="listElement">Element to be shown</param>
    /// <param name="field">Field definition for the element</param>
    /// <returns>The created Html Table Cell</returns>
    private TableCellContent CreateCellForField(IObject listElement, IElement field)
    {
        var property = field.getOrDefault<string>(_Forms._FieldData.name);
        var metaClass = field.getMetaClass();
        var isPropertySet = listElement.isSet(property);
        if (metaClass?.equals(_Forms.TheOne.__DateTimeFieldData) == true)
        {
            if (isPropertySet)
            {
                var hasDate = field.getOrDefault<bool>(_Forms._DateTimeFieldData.hideDate) != true;
                var hasTime = field.getOrDefault<bool>(_Forms._DateTimeFieldData.hideTime) != true;
                var date = listElement.getOrDefault<DateTime>(property);

                var result = string.Empty;
                if (hasDate && hasTime)
                {
                    result = date.ToString(CultureInfo.CurrentCulture);
                }
                else if (hasDate)
                {
                    result = date.ToShortDateString();
                }
                else if (hasTime)
                {
                    result = date.ToShortTimeString();
                }

                return new TableCellContent { Content = result };
            }

            return new TableCellContent {Content = "-"};
        }

        if (metaClass?.equals(_Forms.TheOne.__CheckboxFieldData) == true)
        {
            if (isPropertySet)
            {
                var value = listElement.getOrDefault<bool>(property);
                return new TableCellContent {Content = value ? "X" : "-"};
            }

            return new TableCellContent {Content = "-"};
        }

        if (metaClass?.equals(_Forms.TheOne.__NumberFieldData) == true)
        {
            var format = field.getOrDefault<string>(_Forms._NumberFieldData.format) ?? "";
            var isInteger = field.getOrDefault<bool>(_Forms._NumberFieldData.isInteger);

            if (isPropertySet)
            {
                if (isInteger)
                {
                    var value = listElement.getOrDefault<int>(property);
                    return new TableCellContent {Content = value.ToString(format, CultureInfo.CurrentCulture)};
                }
                else
                {
                    var value = listElement.getOrDefault<double>(property);
                    return new TableCellContent {Content = value.ToString(format, CultureInfo.CurrentCulture)};

                }
            }

            return new TableCellContent { Content = "0"};
        }

        if (metaClass?.equals(_Forms.TheOne.__EvalTextFieldData) == true)
        {
            var cellInformation = InMemoryObject.CreateEmpty();
            var defaultText = listElement.getOrDefault<string>(property);
            cellInformation.set("text", defaultText);

            var evalProperties =
                field.getOrDefault<string>(_Forms._EvalTextFieldData.evalCellProperties);
            if (evalProperties != null)
            {
                defaultText = TextTemplateEngine.Parse(
                    evalProperties,
                    new Dictionary<string, object>
                    {
                        ["i"] = listElement,
                        ["c"] = cellInformation
                    });
            }

            var cssClassName = cellInformation.getOrDefault<string>("cssClass") ?? string.Empty;
            return new TableCellContent
            {
                Content = cellInformation.isSet("text")
                    ? cellInformation.getOrDefault<string>("text")
                    : defaultText,
                CssClass = cssClassName
            };
        }

        if (metaClass?.equals(_Forms.TheOne.__MetaClassElementFieldData) == true)
        {
            var defaultText = NamedElementMethods.GetName((listElement as IElement)?.metaclass);
            var cssClassName = listElement.getOrDefault<string>("cssClass") ?? string.Empty;
            return new TableCellContent
            {
                Content = defaultText,
                CssClass = cssClassName
            };
        }

        if (isPropertySet)
        {
            return new TableCellContent {Content = listElement.getOrDefault<string>(property)};
        }

        return new TableCellContent {Content = "Null"};
    }
}