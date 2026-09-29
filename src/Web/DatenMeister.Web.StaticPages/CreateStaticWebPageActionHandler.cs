using BurnSystems;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.HtmlEngine;

namespace DatenMeister.Web.StaticPages;

public class CreateStaticWebPageActionHandler : IActionHandler {
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            Model._Root.TheOne.__CreateStaticWebPageAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action, string? actionVerb)
    {
        var filename = action.getOrDefault<string>(Model._Root._CreateStaticWebPageAction.htmlFilename);
        if(string.IsNullOrEmpty(filename))
        {
            throw new InvalidOperationException("Filename is not set");
        }
        
        var htmlReport = new HtmlReport(filename);
        
        // Now, we need to add the .css file
        var cssFile = ResourceHelper.LoadStringFromAssembly(
            typeof(CreateStaticWebPageActionHandler), 
            "DatenMeister.Web.StaticPages.Assets.Css.datenmeister-web.min.css");
        htmlReport.CssStyleSheets.Add(cssFile);
        
        // Start the report
        htmlReport.StartReport("Static Export");
        
        htmlReport.EndReport();

        return null;
    }
}