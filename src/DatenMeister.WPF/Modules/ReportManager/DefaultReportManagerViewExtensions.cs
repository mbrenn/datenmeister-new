using System.IO;
using System.Text;
using BurnSystems;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Html;
using DatenMeister.HtmlEngine;
using DatenMeister.Integration.DotNet;
using DatenMeister.Reports;
using DatenMeister.Reports.Adoc;
using DatenMeister.Reports.Html;
using DatenMeister.Reports.Simple;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ReportManager;

public class DefaultReportManagerViewExtensions : IViewExtensionFactory
{
    public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionInfo viewExtensionInfo)
    {
        foreach (var viewExtension in OfferReportForDetailForm(viewExtensionInfo)) 
            yield return viewExtension;

        foreach (var viewExtension in OfferSimpleReports(viewExtensionInfo)) 
            yield return viewExtension;

        foreach (var viewExtension in OfferHtmlReport(viewExtensionInfo)) 
            yield return viewExtension;

        foreach (var viewExtension in OfferAdocReport(viewExtensionInfo)) 
            yield return viewExtension;

        foreach (var viewExtension in OfferSimpleReportsInExplorer(viewExtensionInfo)) 
            yield return viewExtension;
    }

    private IEnumerable<ViewExtension> OfferReportForDetailForm(ViewExtensionInfo viewExtensionInfo)
    {
        // Offers the creation of a report in every item
        // Check if the current query is about the detail form
        var detailFormControl = viewExtensionInfo.GetDetailFormControlOfDetailWindow();
        if (detailFormControl != null)
        {
            var effectiveForm = detailFormControl.EffectiveForm ??
                                throw new InvalidOperationException("effectiveForm == null");
            yield return
                new ItemMenuButtonDefinition(
                    "As Html",
                    x => CreateReportForDetailElement(effectiveForm, x),
                    null,
                    "Item"
                );
        }
    }

    private static IEnumerable<ViewExtension> OfferSimpleReports(ViewExtensionInfo viewExtensionInfo)
    {
        // Handles the simple report
        var simpleReportInfo =
            viewExtensionInfo.IsItemInDetailWindowOfType(
                _DatenMeister.TheOne.Reports.__SimpleReportConfiguration);
        if (simpleReportInfo != null)
        {
            yield return
                new RowItemButtonDefinition(
                    "Create Report",
                    async (x, y) =>
                    {
                        var workspaceLogic = GiveMe.Scope.WorkspaceLogic;

                        var tempObject = InMemoryObject.CreateEmpty();
                        simpleReportInfo.Value.Item1.StoreDialogContentIntoElement(tempObject);

                        var result = await NavigatorForDialogs.Locate(viewExtensionInfo.NavigationHost,
                            new NavigatorForDialogs.NavigatorForDialogConfiguration
                            {
                                DefaultWorkspace = workspaceLogic.GetDataWorkspace(),
                                Title = "Create Report",
                                OkButtonText = "Create Report",
                                Description =
                                    "You can now select an object to which the simple report will be created. If you select a root element, then the report will be created upon all elements of the extent. "
                            });

                        if (result != null)
                        {
                            var simpleReport = new SimpleReportCreator(workspaceLogic, GiveMe.Scope.ScopeStorage, tempObject);

                            string tmpPath;
                            using (var streamWriter = GetRandomWriter(out tmpPath))
                            {
                                simpleReport.CreateReport(streamWriter);
                            }

                            DotNetHelper.CreateProcess(tmpPath);
                        }
                    });
        }
    }

    private static IEnumerable<ViewExtension> OfferHtmlReport(ViewExtensionInfo viewExtensionInfo)
    {
        // Creates a html report
        var reportInstance = viewExtensionInfo.IsItemInDetailWindowOfType(
            _DatenMeister.TheOne.Reports.__HtmlReportInstance);
        if (reportInstance != null)
        {
            yield return
                new RowItemButtonDefinition(
                    "Create Report",
                    (x, y) =>
                    {
                        using var streamWriter = GetRandomWriter(out string tmpPath);

                        var reportGenerator =
                            new HtmlReportCreator(streamWriter);
                        var reportlogic = new ReportLogic(
                            GiveMe.Scope.WorkspaceLogic,
                            GiveMe.Scope.ScopeStorage,
                            reportGenerator);
                        CreateReportWithDefinition(reportlogic, y);

                        DotNetHelper.CreateProcess(tmpPath);
                    });
        }
    }

    private static IEnumerable<ViewExtension> OfferAdocReport(ViewExtensionInfo viewExtensionInfo)
    {
        // Creates a html report
        var reportInstance = viewExtensionInfo.IsItemInDetailWindowOfType(
            _DatenMeister.TheOne.Reports.__AdocReportInstance);
        if (reportInstance != null)
        {
            yield return
                new RowItemButtonDefinition(
                    "Create Report",
                    (x, definition) =>
                    {
                        string tmpPath;
                        using (var streamWriter = GetRandomWriter(out tmpPath, ".adoc"))
                        {
                            var reportGenerator =
                                new AdocReportCreator(streamWriter);
                            var reportLogic = new ReportLogic(
                                GiveMe.Scope.WorkspaceLogic,
                                GiveMe.Scope.ScopeStorage,
                                reportGenerator);

                            CreateReportWithDefinition(reportLogic, definition);
                        }
                    });
        }
    }

    /// <summary>
    /// Creates a report with the given definition 
    /// </summary>
    /// <param name="reportLogic">Report generator to be used</param>
    /// <param name="definition">Definition to be used for the report</param>
    private static void CreateReportWithDefinition(ReportLogic reportLogic, IObject definition)
    {
        var sources = reportLogic.EvaluateSources(definition);
        foreach (var source in sources)
        {
            reportLogic.AddSource(source.Name, source.Collection);
        }

        var reportDefinition =
            definition.getOrDefault<IElement>(_DatenMeister._Reports._HtmlReportInstance.reportDefinition);
        if (reportDefinition == null)
        {
            MessageBox.Show($"The report is not found: {NamedElementMethods.GetName(definition)}");
            return;
        }

        reportLogic.GenerateReportByDefinition(reportDefinition);
    }

    private IEnumerable<ViewExtension> OfferSimpleReportsInExplorer(ViewExtensionInfo viewExtensionInfo)
    {
        var itemExplorerControl = viewExtensionInfo.GetItemExplorerControl();
        if (itemExplorerControl != null)
        {
            yield return new ItemMenuButtonDefinition(
                "Report as Html (Default)",
                x =>
                {
                    if (x is IExtent asExtent)
                    {
                        CreateReportForExplorerView(asExtent);
                    }
                    else
                    {
                        CreateReportForExplorerView(x);
                    }
                },
                null,
                "Export") {Priority = 2};


            yield return new ItemMenuButtonDefinition(
                "Report as Html",
                async x =>
                {
                    var workspaceLogic = GiveMe.Scope.WorkspaceLogic;
                    var simpleConfigurationType =
                        workspaceLogic.GetTypesWorkspace()
                            .ResolveById(
                                "DatenMeister.Models.Reports.Simple.SimpleReportConfiguration")
                        ?? throw new InvalidOperationException("SimpleReportConfiguration not found");

                    var form = workspaceLogic.GetInternalFormsExtent().element("#Form.Report.SimpleConfiguration");

                    var simpleConfiguration = InMemoryObject.TemporaryFactory.create(simpleConfigurationType);
                    var result = await NavigatorForItems.NavigateToElementDetailView(
                        viewExtensionInfo.NavigationHost,
                        new NavigateToItemConfig(simpleConfiguration)
                        {
                            Title = "Configure simple report",
                            Form = new FormDefinition(form)
                        });

                    if (result?.Result == NavigationResult.Saved)
                    {
                        if (x is IExtent asExtent)
                        {
                            CreateReportForExplorerView(
                                asExtent,
                                simpleConfiguration);
                        }
                        else
                        {
                            CreateReportForExplorerView(
                                x,
                                simpleConfiguration);
                        }
                    }
                },
                null,
                "Export") {Priority = 1};
        }
    }

    /// <summary>
    /// Creates the report for the currently selected element.
    /// </summary>
    /// <param name="rootElement">Defines the item that is selected</param>
    /// <param name="simpleReportConfiguration">Describes the configuration to be used, otherwise a default
    /// configuration will be created</param>
    private void CreateReportForExplorerView(IObject rootElement, IElement? simpleReportConfiguration = null)
    {
        simpleReportConfiguration ??=
            new MofFactory(rootElement)
                .create(_DatenMeister.TheOne.Reports.__SimpleReportConfiguration)
                .SetProperty(_DatenMeister._Reports._SimpleReportConfiguration.showDescendents, true)
                .SetProperty(_DatenMeister._Reports._SimpleReportConfiguration.showRootElement, true)
                .SetProperty(_DatenMeister._Reports._SimpleReportConfiguration.showFullName, true)
                .SetProperty(
                    _DatenMeister._Reports._SimpleReportConfiguration.rootElement,
                    rootElement.GetUri() ??
                    throw new InvalidOperationException("Uri of element could not be retrieved"))
                .SetProperty(
                    _DatenMeister._Reports._SimpleReportConfiguration.workspaceId,
                    rootElement.GetUriExtentOf()?.GetWorkspace()?.id ?? WorkspaceNames.WorkspaceData);
            
        string tmpPath;
        using (var streamWriter = GetRandomWriter(out tmpPath))
        {
            var reportCreator = new SimpleReportCreator(GiveMe.Scope.WorkspaceLogic, 
                GiveMe.Scope.ScopeStorage,
                simpleReportConfiguration);
            reportCreator.CreateReport(streamWriter);
        }

        DotNetHelper.CreateProcess(tmpPath);
    }

    private static StreamWriter GetRandomWriter(out string tmpPath, string extension = ".html")
    {
        var id = StringManipulation.RandomString(10);
        tmpPath = Path.Combine(Path.GetTempPath(), id + extension);
        return new StreamWriter(tmpPath, false, Encoding.UTF8);
    }

    /// <summary>
    /// Creates the report for a certain detail html element
    /// </summary>
    /// <param name="effectiveForm">Defines the effective form</param>
    /// <param name="selectedItem">The item being selected</param>
    private void CreateReportForDetailElement(IObject effectiveForm, IObject selectedItem)
    {
        var id = StringManipulation.RandomString(10);
        var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");

        using (var report = new HtmlReport(tmpPath))
        {
            report.StartReport("Detail: " + selectedItem);
            report.Add(new HtmlHeadline("Detail Information", 1));
            var itemFormatter = new ItemFormatter(report);
            itemFormatter.FormatItem(selectedItem, effectiveForm);
            report.EndReport();
        }

        DotNetHelper.CreateProcess(tmpPath);
    }
}