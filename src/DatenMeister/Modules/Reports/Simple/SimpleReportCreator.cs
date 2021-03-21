using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.HtmlExporter.Formatter;
using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using static DatenMeister.Models._DatenMeister._Reports;

namespace DatenMeister.Modules.Reports.Simple
{
    /// <summary>
    /// Defines the engine for simple reporting
    /// </summary>
    public class SimpleReportCreator
    {
        /// <summary>
        /// Stores the default classifier hints
        /// </summary>
        private readonly DefaultClassifierHints _defaultClassifierHints;

        private readonly FormCreator _formCreator;

        /// <summary>
        /// Gets or sets the workspace logic
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Stores the configuration of the report engine
        /// </summary>
        private readonly IElement _reportConfiguration;

        /// <summary>
        /// Initializes a new instance of the simple report reportCreator. 
        /// </summary>
        /// <param name="workspaceLogic">The workspace logic to be used</param>
        /// <param name="simpleReportConfiguration">The configuration defining the layout of the report</param>
        public SimpleReportCreator(IWorkspaceLogic workspaceLogic, IElement simpleReportConfiguration)
        {
            _workspaceLogic = workspaceLogic;
            _reportConfiguration = simpleReportConfiguration;
            _defaultClassifierHints = new DefaultClassifierHints();
            _formCreator = FormCreator.Create(workspaceLogic, null);
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
            var itemFormatter = new ItemFormatter(report, _workspaceLogic);

            report.SetDefaultCssStyle();

            if (_reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showRootElement))
            {
                var name = NamedElementMethods.GetFullName(rootElement);
                report.Add(new HtmlHeadline($"Reported Item '{name}'", 1));
                var detailForm =
                    _formCreator.CreateDetailForm(rootElement, creationMode);
                itemFormatter.FormatItem(rootElement, detailForm);
            }

            // And now start the report
            report.StartReport("Extent: " + NamedElementMethods.GetName(rootElement));

            // First, gets the elements to be shown
            IReflectiveCollection elements =
                new TemporaryReflectiveCollection(_defaultClassifierHints.GetPackagedElements(rootElement));
            if (_reportConfiguration.getOrDefault<bool>(_SimpleReportConfiguration.showDescendents))
            {
                elements = elements.GetAllCompositesIncludingThemselves();
            }

            var first = (elements.FirstOrDefault(x => x is IElement) as IElement)?.metaclass;
            Debug.WriteLine(first);

            report.Add(new HtmlHeadline("Items in collection", 1));

            var foundForm = _reportConfiguration.getOrDefault<IElement>(_SimpleReportConfiguration.form);
            if (_reportConfiguration.getOrDefault<_Elements.___ReportTableForTypeMode>(_SimpleReportConfiguration.typeMode) == _Elements.___ReportTableForTypeMode.PerType)
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

                    ReportItemCollection(collection, foundForm, itemFormatter);
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

                ReportItemCollection(elements, foundForm, itemFormatter);
            }

            report.EndReport();
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

        private void ReportItemCollection(IReflectiveCollection metaClass, IObject form, ItemFormatter itemFormatter)
        {
            // Gets the reflective sequence for the name
            var collection = new TemporaryReflectiveSequence(metaClass);

            itemFormatter.FormatCollectionOfItems(collection, form);
        }
    }
}