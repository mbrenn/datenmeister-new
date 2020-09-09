using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autofac;
using BurnSystems;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Reports;
using DatenMeister.Models.Reports.Simple;
using DatenMeister.Modules.HtmlExporter.Formatter;
using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Modules.Reports;
using DatenMeister.Modules.Reports.Simple;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ReportManager
{
    public class DefaultReportManagerViewExtensions : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionInfo viewExtensionInfo)
        {
            // Check if the current query is about the detail form
            var detailFormControl = viewExtensionInfo.GetDetailFormControlOfDetailWindow();
            if (detailFormControl != null)
            {
                var effectiveForm = detailFormControl.EffectiveForm ??
                                    throw new InvalidOperationException("effectiveForm == null");
                yield return
                    new ItemMenuButtonDefinition(
                        "As Html",
                        (x) => CreateReportForDetailElement(effectiveForm, x),
                        null,
                        "Item"
                    );
            }

            var simpleReportInfo = viewExtensionInfo.IsItemInDetailWindowOfType(
                _Reports.TheOne.__SimpleReportConfiguration);
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
                                var configuration = DotNetConverter.ConvertToDotNetObject<SimpleReportConfiguration>(tempObject);
                                configuration.rootElement = result;
                                var simpleReport = new SimpleReportCreator(workspaceLogic, configuration);

                                var tmpPath = GetRandomWriter(out var streamWriter);
                                simpleReport.CreateReport(streamWriter);

                                DotNetHelper.CreateProcess(tmpPath);
                            }
                        });
            }
            
            var reportInstance = viewExtensionInfo.IsItemInDetailWindowOfType(
                _Reports.TheOne.__HtmlReportInstance);
            if (reportInstance != null)
            {
                yield return
                    new RowItemButtonDefinition(
                        "Create Report",
                        (x, y) =>
                        {
                            var reportGenerator =
                                new HtmlReportCreator(GiveMe.Scope.WorkspaceLogic, GiveMe.Scope.ScopeStorage);
                            var reportLogic = GiveMe.Scope.Resolve<ReportLogic>();
                            var sources = reportLogic.EvaluateSources(y);
                            foreach (var source in sources)
                            {
                                reportGenerator.AddSource(source.Name, source.Collection);
                            }

                            var reportDefinition =
                                y.getOrDefault<IElement>(_Reports._HtmlReportInstance.reportDefinition);
                            if (reportDefinition == null)
                            {
                                MessageBox.Show("The report is not found");
                                return;
                            }

                            var filePath = GetRandomWriter(out var writer);
                            reportGenerator.GenerateReportByDefinition(reportDefinition, writer);

                            DotNetHelper.CreateProcess(filePath);
                        });
            }

            // Check if the the query is about the current view
            /*var listViewControl = viewExtensionInfo.GetListViewControl();
            if (listViewControl != null)
            {
                var effectiveForm = listViewControl.EffectiveForm ??
                                    throw new InvalidOperationException("effectiveForm == null");
                yield return new CollectionMenuButtonDefinition(
                    "As Html",
                    (x) => CreateReportForExplorerView(effectiveForm, x),
                    null,
                    "Collection")
                {
                    IsTopCategoryFixed = true
                };
            }*/

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
                                    DotNetConverter.ConvertToDotNetObject<SimpleReportConfiguration>(
                                        simpleConfiguration));
                            }
                            else
                            {
                                CreateReportForExplorerView(
                                    x,
                                    DotNetConverter.ConvertToDotNetObject<SimpleReportConfiguration>(
                                        simpleConfiguration));
                            }
                        }
                    },
                    null,
                    "Export") {Priority = 1};
            }
        }

        /// <summary>
        /// Creates the report for the currently selected element
        /// </summary>
        /// <param name="rootElement">Defines the item that is selected</param>
        /// <param name="simpleReportConfiguration">Describes the configuration to be used, otherwise a default
        /// configuration will be created</param>
        private void CreateReportForExplorerView(IObject rootElement, SimpleReportConfiguration? simpleReportConfiguration = null)
        {
            simpleReportConfiguration ??= new SimpleReportConfiguration
            {
                form = null, 
                showDescendents = true,
                showRootElement = true,
                showFullName = true
            };

            simpleReportConfiguration.rootElement = rootElement;

            var tmpPath = GetRandomWriter(out var streamWriter);

            var reportCreator = new SimpleReportCreator(GiveMe.Scope.WorkspaceLogic, simpleReportConfiguration);
            reportCreator.CreateReport(streamWriter);

            DotNetHelper.CreateProcess(tmpPath);
        }

        private static string GetRandomWriter(out StreamWriter streamWriter)
        {
            var id = StringManipulation.RandomString(10);
            var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");
            streamWriter = new StreamWriter(tmpPath, false, Encoding.UTF8);
            return tmpPath;
        }

        private void CreateReportForDetailElement(IObject effectiveForm, IObject selectedItem)
        {
            var id = StringManipulation.RandomString(10);
            var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");

            using (var report = new HtmlReport(tmpPath))
            {
                report.StartReport("Detail: " + selectedItem);
                report.Add(new HtmlHeadline("Detail Information", 1));
                var itemFormatter = new ItemFormatter(report, GiveMe.Scope.WorkspaceLogic);
                itemFormatter.FormatItem(selectedItem, effectiveForm);
                report.EndReport();
            }

            DotNetHelper.CreateProcess(tmpPath);
        }
    }
}