using System.Buffers.Text;
using System.Text;
using BurnSystems;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.HtmlEngine;
using DatenMeister.Web.StaticPages.HtmlElements;

namespace DatenMeister.Web.StaticPages;

/// <summary>
/// Action handler responsible for creating, downloading, or rendering static HTML web pages.
/// </summary>
/// <param name="scopeStorage">The scope storage providing static page element registrations.</param>
public class CreateStaticWebPageActionHandler(IScopeStorage scopeStorage) : IActionHandlerWithVerbs {
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

    /// <summary>
    /// Gets the supported action verbs
    /// </summary>
    public string[] SupportedVerbs { get; } = [ActionVerbs.Execute, ActionVerbs.Render, ActionVerbs.Download];

    /// <summary>
    /// Evaluates the Create Static Web Page action based on the specified action verb.
    /// </summary>
    /// <param name="actionLogic">The action logic context.</param>
    /// <param name="action">The action element containing page configuration and HTML element definitions.</param>
    /// <param name="actionVerb">The verb under which to evaluate: execute (write to file), render (browser render), or download (browser download).</param>
    /// <returns>The action result containing client actions or success/failure indicator.</returns>
    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action, string? actionVerb)
    {
        var filename = action.getOrDefault<string>(Model._Root._CreateStaticWebPageAction.htmlFilename);
        
        // Create an InMemoryStream and adds it the the constructor of HtmlReport
        var stream = new MemoryStream();
        var htmlReport = new HtmlReport(stream);
        
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
        var title= action.getOrDefault<string?>(Model._Root._CreateStaticWebPageAction.pageTitle) ?? "Unnamed Page";
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
        
        // Converts the content of the stream to a string
        var byteBuffer = stream.ToArray();
        var content = Encoding.UTF8.GetString(byteBuffer);

        switch (actionVerb)
        {
            case ActionVerbs.Execute:
                if(string.IsNullOrEmpty(filename))
                {
                    throw new InvalidOperationException("Filename is not set");
                }
                await File.WriteAllBytesAsync(filename, byteBuffer);
                return ActionModelHelper.CreateSuccessActionResult();
            case ActionVerbs.Download:
            {
                if(string.IsNullOrEmpty(filename))
                {
                    throw new InvalidOperationException("Filename is not set");
                }
                
                var clientResult = InMemoryObject.CreateEmpty(_Actions.TheOne.ClientActions.__DownloadHtmlClientAction);
                clientResult.set(_Actions._ClientActions._DownloadHtmlClientAction.html, content);
                clientResult.set(_Actions._ClientActions._DownloadHtmlClientAction.filename, Path.GetFileName(filename));

                return ActionModelHelper.CreateActionResult(true, [clientResult]);

            }
            case ActionVerbs.Render:
            {
                var clientResult = InMemoryObject.CreateEmpty(_Actions.TheOne.ClientActions.__RenderHtmlClientAction);
                clientResult.set(_Actions._ClientActions._RenderHtmlClientAction.html, content);
                clientResult.set(_Actions._ClientActions._RenderHtmlClientAction.renderCompletePage, true);

                return ActionModelHelper.CreateActionResult(true, [clientResult]);
            }

            default:
                return ActionModelHelper.CreateFailedActionResult("Unknown verb: " + actionVerb);
        }
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