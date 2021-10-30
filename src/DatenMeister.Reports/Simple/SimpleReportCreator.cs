using System;
using System.IO;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormCreator;
using DatenMeister.Html;
using DatenMeister.HtmlEngine;
using static DatenMeister.Core.Models._DatenMeister._Reports;

namespace DatenMeister.Reports.Simple
{
    /// <summary>
    /// Defines the engine for simple reporting
    /// </summary>
    public class SimpleReportCreator
    {
        private readonly FormCreator _formCreator;

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
        /// <param name="simpleReportConfiguration">The configuration defining the layout of the report</param>
        public SimpleReportCreator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage, IElement simpleReportConfiguration)
        {
            _workspaceLogic = workspaceLogic;
            _reportConfiguration = simpleReportConfiguration;
            _formCreator = FormCreator.Create(workspaceLogic, scopeStorage);
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
            var formFactoryConfiguration = new FormFactoryConfiguration
            {
                AutomaticMetaClassField =
                    _reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showMetaClasses)
            };
            
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
                var detailForm =
                    _formCreator.CreateDetailForm(rootElement, formFactoryConfiguration);
                _itemFormatter.FormatItem(rootElement, detailForm);
            }
            
            // First, gets the elements to be shown
            IReflectiveCollection elements =
                new TemporaryReflectiveCollection(DefaultClassifierHints.GetPackagedElements(rootElement));
            if (_reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showDescendents))
            {
                elements = elements.GetAllCompositesIncludingThemselves();
            }

            WriteReportForCollection(report, elements, formFactoryConfiguration);

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
            var formFactoryConfiguration = new FormFactoryConfiguration
            {
                AutomaticMetaClassField =
                    _reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showMetaClasses)
            };

            using var report = new HtmlReport(textWriter);
            _itemFormatter = new ItemFormatter(report);
            
            report.SetDefaultCssStyle();
            report.StartReport("DatenMeister - Report");
            WriteReportForCollection(report, collection, formFactoryConfiguration);
            
            report.EndReport();
        }

        /// <summary>
        /// Writes the report for the attached elements
        /// </summary>
        /// <param name="report"></param>
        /// <param name="elements"></param>
        /// <param name="creationMode"></param>
        private void WriteReportForCollection(
            IHtmlReport report,
            IReflectiveCollection elements,
            FormFactoryConfiguration creationMode)
        {
            report.Add(new HtmlHeadline("Items in collection", 1));

            var reportTableMode =
                _reportConfiguration.getOrDefault<_Elements.___ReportTableForTypeMode>(
                    _SimpleReportConfiguration.typeMode);
            var foundForm = _reportConfiguration.getOrDefault<IElement>(_SimpleReportConfiguration.form);
            var addFullNameColumn =
                _reportConfiguration.getOrDefault<bool>(_DatenMeister._Reports._SimpleReportConfiguration.showFullName);
            var collectionReporter = new SimpleReportForCollection(
                _formCreator,
                _itemFormatter ?? throw new InvalidOperationException("itemFormatter is null"), 
                report)
            {
                TableForTypeMode = reportTableMode,
                Form = foundForm,
                AddFullNameColumn = addFullNameColumn
            };
            
            collectionReporter.WriteReportForCollection(elements, creationMode);

        }
    }
}