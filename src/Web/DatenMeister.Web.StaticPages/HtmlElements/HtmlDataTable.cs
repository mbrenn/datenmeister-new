using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.DataView;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.HtmlEngine;

namespace DatenMeister.Web.StaticPages.HtmlElements;

public class HtmlDataTable(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IHtmlElement
{
    public bool IsResponsible(IElement element)
    {
        return element.getMetaClass()?.equals(
            _Reports.TheOne.Elements.__ReportTable) == true;
    }

    public void Generate(HtmlReport htmlReport, IElement element)
    {
        var cssClass = element.getOrDefault<string>(_Reports._Elements._ReportTable.cssClass);
        var form = element.getOrDefault<IElement?>(_Reports._Elements._ReportTable.form);
        var viewNode = element.getOrDefault<IElement?>(_Reports._Elements._ReportTable.viewNode)
            ?? throw new InvalidOperationException("View node is not specified");

        
        // Gets the data from the view point
        var dataViewEvaluation = new DataViewLogic(workspaceLogic, scopeStorage); 
        var elements = dataViewEvaluation.GetElementsForViewNode(viewNode);
        
        // Creates the data from the form in case the form is not specified
        if (form == null)
        {
            var formCreationFactory = new FormCreationContextFactory(
                workspaceLogic,
                scopeStorage);

            var context = formCreationFactory.Create(string.Empty);
            // Create form
            form = FormCreation.CreateTableForm(
                new TableFormFactoryParameter()
                {
                    Collection = elements
                }, context).Forms.FirstOrDefault();
        }
        
        // Creates the JavaScript that invokes the table creation
        var script = new HtmlInlineScript("testEntryPoint();");
        htmlReport.Add(script);        

    }
}