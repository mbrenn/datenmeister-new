using BurnSystems.Logging;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.DataView;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.HtmlEngine;
using DatenMeister.Web.Json;
using DatenMeister.Web.StaticPages.Model;

namespace DatenMeister.Web.StaticPages.HtmlElements;

public class HtmlDataTable(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IHtmlElement
{
    /// <summary>
    /// Stores the logger for this class
    /// </summary>
    private static ILogger logger = new ClassLogger(typeof(HtmlDataTable));

    /// <summary>
    /// Stores the number of calls to explicitly name the instances
    /// </summary>
    private static uint _currentCall = 0;

    /// <summary>
    /// Gets the next call id
    /// </summary>
    /// <returns>Gets next call id</returns>
    public uint GetNextCallId()
    {
        Interlocked.Increment(ref _currentCall);
        return _currentCall;
    }

    public bool IsResponsible(IElement element)
    {
        return element.getMetaClass()?.equals(
            _Reports.TheOne.Elements.__ReportTable) == true;
    }

    public void Generate(HtmlReport htmlReport, IElement element)
    {
        logger.Debug("Generating HTML table");
        var cssClass = element.getOrDefault<string>(_Root._StaticTableOfDataElement.cssClass);
        var form = element.getOrDefault<IElement?>(_Root._StaticTableOfDataElement.form);
        var viewNode = element.getOrDefault<IElement?>(_Root._StaticTableOfDataElement.viewNode)
                       ?? throw new InvalidOperationException("View node is not specified");
        var visibleItems = element.getOrDefault<int>(_Root._StaticTableOfDataElement.visibleItems);
        if (visibleItems == 0)
        {
            visibleItems = 100;
        }

        // Gets the data from the view point
        var dataViewEvaluation = new DataViewLogic(workspaceLogic, scopeStorage);
        var elements = dataViewEvaluation.GetElementsForViewNode(viewNode);

        // Creates the data from the form in case the form is not specified
        if (form == null)
        {
            logger.Debug("Form is not specified, creating form");
            var formCreationFactory = new FormCreationContextFactory(
                workspaceLogic,
                scopeStorage);

            var context = formCreationFactory.Create(string.Empty);
            // Create form
            form = FormCreation.CreateTableForm(
                new TableFormFactoryParameter
                {
                    Collection = elements
                }, context).Forms.FirstOrDefault();

            if (form == null)
            {
                throw new InvalidOperationException("Form could not be created");
            }
        }
        
        // Creates the container
        var htmlContainer = new HtmlDivElement("Loading table...");
        htmlContainer.Id = "dataTable" + GetNextCallId();
        htmlReport.Add(htmlContainer);

        // Enumerates the objects
        var elementsAsArray = elements.ToList();
        var converter = new MofJsonConverter
        {
            ResolveReferenceToOtherExtents = false
        };

        // Creates the helper method
        var data = InMemoryObject.CreateEmpty();
        data.set("cssClass", cssClass);
        data.set("visibleItems", visibleItems);

        HtmlInlineScript sourceTag;
        var variableName = "data" + GetNextCallId();
        
        using (new StopWatchLogger(logger, "Enumerating objects"))
        {
            var formAsJson = converter.ConvertToJsonString(form);
            var elementsAsJson = converter.ConvertToJsonString(elementsAsArray);
            var configAsJson = converter.ConvertToJsonString(data);
            sourceTag =
                new HtmlInlineScript(
                    $"var {variableName} = " +
                    $"{{config: {configAsJson}, " +
                    $"form: {formAsJson}, " +
                    $"data: {elementsAsJson}, " +
                    $"htmlContainer: $('#{htmlContainer.Id}')}}" +
                    $";");
        }

        htmlReport.Add(sourceTag);
        htmlReport.Add(new HtmlInlineScript($"renderStaticDataPage({variableName});"));
    }
}