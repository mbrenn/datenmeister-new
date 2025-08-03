using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Html;
using DatenMeister.HtmlEngine;
using static DatenMeister.Core.Models._Reports;

namespace DatenMeister.Reports.Simple;

/// <summary>
/// Defines the engine for simple reporting
/// </summary>
public class SimpleReportCreator
{
    /// <summary>
    /// Stores the creation context which is used to create the correspondign views
    /// </summary>
    private FormCreationContextFactory _formContextFactory;

    /// <summary>
    /// Gets or sets the workspace logic
    /// </summary>
    private readonly IWorkspaceLogic _workspaceLogic;

    /// <summary>
    /// Stores the configuration of the report engine
    /// </summary>
    private readonly IElement _reportConfiguration;

    private ItemFormatter? _itemFormatter;

    /// <summary>
    /// Initializes a new instance of the simple report reportCreator. 
    /// </summary>
    /// <param name="workspaceLogic">The workspace logic to be used</param>
    /// <param name="scopeStorage">The scope storage</param>
    /// <param name="simpleReportConfiguration">The configuration defining the layout of the report</param>
    public SimpleReportCreator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage, IElement simpleReportConfiguration)
    {
        _workspaceLogic = workspaceLogic;
        _reportConfiguration = simpleReportConfiguration;
        _formContextFactory = new FormCreationContextFactory(workspaceLogic, scopeStorage);
        
    }

    /// <summary>
    /// Creates the report in a file
    /// </summary>
    /// <param name="filePath">Path of the file that shall be created</param>
    public void CreateReportInFile(string filePath)
    {
        using var writer = new StreamWriter(filePath);
        CreateReport(writer);
    }

    /// <summary>
    /// Creates the html report according the given form and elements
    /// </summary>
    /// <param name="textWriter">Text Writer to be used for html file creation</param>
    public void CreateReport(TextWriter textWriter)
    {
        var rootElementUrl = _reportConfiguration.getOrDefault<string>(_SimpleReportConfiguration.rootElement);
        var workspaceId = _reportConfiguration.getOrDefault<string>(_SimpleReportConfiguration.workspaceId)
                          ?? WorkspaceNames.WorkspaceData;
        var rootElement = ExtentHelper.TryGetItemByWorkspaceAndPath(
            _workspaceLogic,
            workspaceId,
            rootElementUrl);
        if (rootElement == null)
        {
            throw new InvalidOperationException("rootElement is null");
        }

        using var report = new HtmlReport(textWriter);
        _itemFormatter = new ItemFormatter(report);

        report.SetDefaultCssStyle();
            
        // And now start the report
        report.StartReport("Extent: " + NamedElementMethods.GetName(rootElement));

        if (_reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showRootElement))
        {
            var name = NamedElementMethods.GetFullName(rootElement);
            report.Add(new HtmlHeadline($"Reported Item '{name}'", 1));
            var detailForm = FormCreation.CreateRowForm(
                                     new RowFormFactoryParameter
                                     {
                                         Element = rootElement
                                     },
                                     _formContextFactory.Create(string.Empty))
                                 .Forms.FirstOrDefault()
                             ?? throw new InvalidOperationException("detailForm is null");

            _itemFormatter.FormatItem(rootElement, detailForm);
        }

        // First, gets the elements to be shown
        IReflectiveCollection elements =
            new TemporaryReflectiveCollection(DefaultClassifierHints.GetPackagedElements(rootElement));
        if (_reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showDescendents))
        {
            elements = elements.GetAllCompositesIncludingThemselves();
        }

        WriteReportForCollection(report, elements);

        report.EndReport();
    }

    /// <summary>
    /// Creates the report in a file
    /// </summary>
    /// <param name="filePath">Path of the file that shall be created</param>
    /// <param name="collection">Collection to be used for the items to be shown</param>
    public void CreateReportForCollection(string filePath, IReflectiveCollection collection)
    {
        using var writer = new StreamWriter(filePath);
        CreateReportForCollection(writer, collection);
    }

    /// <summary>
    /// Creates the report for a collection of items.
    /// The items are directly retrieved from the field itself, so the
    /// configuration to identify the root element is not used
    /// </summary>
    /// <param name="textWriter">Text writer to be used</param>
    /// <param name="collection">Reflective collection of elements to be shown</param>
    public void CreateReportForCollection(TextWriter textWriter, IReflectiveCollection collection)
    {
        using var report = new HtmlReport(textWriter);
        _itemFormatter = new ItemFormatter(report);
            
        report.SetDefaultCssStyle();
        report.StartReport("DatenMeister - Report");
        WriteReportForCollection(report, collection);
            
        report.EndReport();
    }

    /// <summary>
    /// Writes the report for the attached elements
    /// </summary>
    /// <param name="report"></param>
    /// <param name="elements"></param>
    private void WriteReportForCollection(
        IHtmlReport report,
        IReflectiveCollection elements)
    {
        report.Add(new HtmlHeadline("Items in collection", 1));

        var reportTableMode =
            _reportConfiguration.getOrDefault<_Elements.___ReportTableForTypeMode>(
                _SimpleReportConfiguration.typeMode);
        var foundForm = _reportConfiguration.getOrDefault<IElement>(_SimpleReportConfiguration.form);
        var addFullNameColumn =
            _reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showFullName);
        var collectionReporter = new SimpleReportForCollection(
            _formContextFactory.Create(string.Empty),
            _itemFormatter ?? throw new InvalidOperationException("itemFormatter is null"), 
            report)
        {
            TableForTypeMode = reportTableMode,
            Form = foundForm,
            AddFullNameColumn = addFullNameColumn
        };
            
        collectionReporter.WriteReportForCollection(elements);

    }
}