using BurnSystems;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.HtmlEngine;
using DatenMeister.Web.StaticPages.HtmlElements;

namespace DatenMeister.Web.StaticPages;

public class CreateStaticWebPageActionHandler(IScopeStorage scopeStorage) : IActionHandler {
    /// <summary>
    /// Determines whether the given element is responsible for handling the "Create Static Web Page" action.
    /// </summary>
    /// <param name="node">The element to be evaluated for responsibility.</param>
    /// <returns>True if the element is responsible for the given action; otherwise, false.</returns>
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
        var bootstrap = ResourceHelper.LoadStringFromAssembly(
            typeof(CreateStaticWebPageActionHandler), 
            "DatenMeister.Web.StaticPages.Assets.Css.bootstrap.min.css");
        htmlReport.AddCssStyleSheet(bootstrap);
        var cssFile = ResourceHelper.LoadStringFromAssembly(
            typeof(CreateStaticWebPageActionHandler), 
            "DatenMeister.Web.StaticPages.Assets.Css.datenmeister-web.min.css");
        htmlReport.AddCssStyleSheet(cssFile);
        
        var jquery = ResourceHelper.LoadStringFromAssembly(
            typeof(CreateStaticWebPageActionHandler), 
            "DatenMeister.Web.StaticPages.Assets.Js_Bundled.jquery.min.js");
        
        var jsFile = ResourceHelper.LoadStringFromAssembly(
            typeof(CreateStaticWebPageActionHandler), 
            "DatenMeister.Web.StaticPages.Assets.Js_Bundled.EntryPoint.js");
        
        // Start the report
        var title= action.getOrDefault<string>(Model._Root._CreateStaticWebPageAction.pageTitle) ?? "Unnamed Page";
        htmlReport.StartReport(title);
        htmlReport.Add(new HtmlInlineScript(jquery));
        htmlReport.Add(new HtmlInlineScript(jsFile));

        var elements = action.getOrDefault<IReflectiveCollection>(Model._Root._CreateStaticWebPageAction.htmlElements);
        foreach (var element in elements.OfType<IElement>())
        {
            var htmlElement = FindElement(element);
            htmlElement.Generate(htmlReport, element);
        }
        
        // End the report
        htmlReport.EndReport();

        return null;
    }

    /// <summary>
    /// Finds the responsible html generator for a specific html element definition
    /// </summary>
    /// <param name="htmlElementData">Data element describing the html to be added</param>
    /// <returns>In case a responsible element is found, it is returned</returns>
    /// <exception cref="InvalidOperationException">Thrown if no responsible html element found</exception>
    public IHtmlElement FindElement(IElement htmlElementData)
    {
        var pagesData = scopeStorage.Get<StaticPagesData>();
        var relevant = 
            pagesData.HtmlElements.FirstOrDefault(x => x.IsResponsible(htmlElementData));
        if (relevant == null)
        {
            throw new InvalidOperationException("No responsible html element found: " +
                                                NamedElementMethods.GetName(htmlElementData));
        }

        return relevant;
    }
}