using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
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
        public SimpleReportCreator(IWorkspaceLogic workspaceLogic, IElement simpleReportConfiguration)
        {
            _workspaceLogic = workspaceLogic;
            _reportConfiguration = simpleReportConfiguration;
            _formCreator = FormCreator.Create(workspaceLogic, null);
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
            var creationMode = _reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showMetaClasses)
                ? CreationMode.All
                : CreationMode.All & ~CreationMode.AddMetaClass;
 
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
            _itemFormatter = new ItemFormatter(report, _workspaceLogic);

            report.SetDefaultCssStyle();
            
            // And now start the report
            report.StartReport("Extent: " + NamedElementMethods.GetName(rootElement));

            if (_reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showRootElement))
            {
                var name = NamedElementMethods.GetFullName(rootElement);
                report.Add(new HtmlHeadline($"Reported Item '{name}'", 1));
                var detailForm =
                    _formCreator.CreateDetailForm(rootElement, creationMode);
                _itemFormatter.FormatItem(rootElement, detailForm);
            }
            
            // First, gets the elements to be shown
            IReflectiveCollection elements =
                new TemporaryReflectiveCollection(DefaultClassifierHints.GetPackagedElements(rootElement));
            if (_reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showDescendents))
            {
                elements = elements.GetAllCompositesIncludingThemselves();
            }

            WriteReportForCollection(report, elements, creationMode);

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
            var creationMode = _reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showMetaClasses)
                ? CreationMode.All
                : CreationMode.All & ~CreationMode.AddMetaClass;

            using var report = new HtmlReport(textWriter);
            _itemFormatter = new ItemFormatter(report, _workspaceLogic);
            
            report.SetDefaultCssStyle();
            report.StartReport("DatenMeister - Report");
            WriteReportForCollection(report, collection, creationMode);
            
            report.EndReport();
        }

        /// <summary>
        /// Writes the report for the attached elements
        /// </summary>
        /// <param name="report"></param>
        /// <param name="elements"></param>
        /// <param name="creationMode"></param>
        /// <param name="itemFormatter"></param>
        private void WriteReportForCollection(
            IHtmlReport report,
            IReflectiveCollection elements,
            CreationMode creationMode)
        {
            report.Add(new HtmlHeadline("Items in collection", 1));

            var foundForm = _reportConfiguration.getOrDefault<IElement>(_SimpleReportConfiguration.form);
            if (_reportConfiguration.getOrDefault<_Elements.___ReportTableForTypeMode>(
                    _SimpleReportConfiguration.typeMode)
                == _Elements.___ReportTableForTypeMode.PerType)
            {
                // Splits them up by metaclasses 
                var metaClasses =
                    elements.GroupBy(
                        x => x is IElement element ? element.metaclass : null,
                        new MofObjectEqualityComparer()).ToList();

                foreach (var metaClass in metaClasses)
                {
                    // Gets the name of the metaclass
                    var metaClassName = metaClass.Key == null
                        ? "Unclassified"
                        : "Classifier: " + NamedElementMethods.GetName(metaClass.Key);

                    report.Add(new HtmlHeadline(metaClassName, 2));

                    var collection = new TemporaryReflectiveCollection(metaClass);

                    if (metaClass.Key == null)
                    {
                        foundForm = _formCreator.CreateListFormForElements(
                            collection,
                            creationMode);
                    }
                    else
                    {
                        foundForm = _formCreator.CreateListFormForMetaClass(metaClass.Key, creationMode);
                    }

                    AddFullNameColumnIfNecessary(foundForm);

                    ReportItemCollection(collection, foundForm);
                }
            }
            else
            {
                if (foundForm == null)
                {
                    foundForm = _formCreator.CreateListFormForElements(
                        elements,
                        creationMode);

                    AddFullNameColumnIfNecessary(foundForm);
                }

                ReportItemCollection(elements, foundForm);
            }
        }

        private void AddFullNameColumnIfNecessary(IObject foundForm)
        {
            if (_reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showFullName))
            {
                // Create the metaclass as a field
                var fullNamefield = MofFactory.Create(foundForm, _DatenMeister.TheOne.Forms.__FullNameFieldData);
                fullNamefield.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Path");
                fullNamefield.set(_DatenMeister._Forms._MetaClassElementFieldData.title, "Path");
                foundForm.get<IReflectiveSequence>(_DatenMeister._Forms._ListForm.field).add(0, fullNamefield);
            }
        }

        private void ReportItemCollection(IReflectiveCollection metaClass, IObject form)
        {
            // Gets the reflective sequence for the name
            var collection = new TemporaryReflectiveSequence(metaClass);

            Debug.Assert(_itemFormatter != null, nameof(_itemFormatter) + " != null");
            _itemFormatter!.FormatCollectionOfItems(collection, form);
        }
    }
}