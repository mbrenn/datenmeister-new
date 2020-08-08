using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Models.Reports.Simple;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.HtmlReporter.Formatter;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

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
        private readonly SimpleReportConfiguration _reportConfiguration;

        public SimpleReportCreator(IWorkspaceLogic workspaceLogic, IElement element)
        {
            _workspaceLogic = workspaceLogic;
            _reportConfiguration = DotNetConverter.ConvertToDotNetObject<SimpleReportConfiguration>( element);
            _defaultClassifierHints = new DefaultClassifierHints(workspaceLogic);
            _formCreator = FormCreator.Create(workspaceLogic, null);
        }

        /// <summary>
        /// Initializes a new instance of the ReportCreator class.
        /// </summary>
        /// <param name="workspaceLogic">Default workspace Logic to be used</param>
        /// <param name="reportConfiguration">The report configuration to be used</param>
        public SimpleReportCreator(IWorkspaceLogic workspaceLogic, SimpleReportConfiguration reportConfiguration)
        {
            _workspaceLogic = workspaceLogic;
            _reportConfiguration = reportConfiguration;
            _defaultClassifierHints = new DefaultClassifierHints(workspaceLogic);
            _formCreator = FormCreator.Create(workspaceLogic, null);
        }

        /// <summary>
        /// Creates the html report according the given form and elements
        /// </summary>
        /// <param name="textWriter">Text Writer to be used for html file creation</param>
        public void CreateReport(TextWriter textWriter)
        {
            var creationMode = _reportConfiguration.showMetaClasses
                ? CreationMode.All
                : CreationMode.All & ~CreationMode.AddMetaClass;

            var rootElement = _reportConfiguration.rootElement;
            if (rootElement == null)
                throw new InvalidOperationException("rootElement is null");

            using (var report = new HtmlReport(textWriter))
            {
                var itemFormatter = new ItemFormatter(report, _workspaceLogic);

                report.SetDefaultCssStyle();

                if (_reportConfiguration.showRootElement)
                {
                    var name = NamedElementMethods.GetFullName(rootElement);
                    report.Add(new HtmlHeadline($"Reported Item + '{name}'", 1));
                    var detailForm =
                        _formCreator.CreateDetailForm(rootElement, creationMode);
                    itemFormatter.FormatItem(rootElement, detailForm);
                }

                // And now start the report
                report.StartReport("Extent: " + NamedElementMethods.GetName(_reportConfiguration.rootElement));

                // First, gets the elements to be shown
                IReflectiveCollection elements =
                    new TemporaryReflectiveCollection(_defaultClassifierHints.GetPackagedElements(rootElement));
                if (_reportConfiguration.showDescendents)
                {
                    elements = elements.GetAllCompositesIncludingThemselves();
                }

                var first = (elements.FirstOrDefault(x => x is IElement) as IElement)?.metaclass;
                Debug.WriteLine(first);

                report.Add(new HtmlHeadline("Items in collection", 1));

                var foundForm = _reportConfiguration.form;
                if (_reportConfiguration.typeMode == ReportTableForTypeMode.PerType)
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
        }

        private void AddFullNameColumnIfNecessary(IObject foundForm)
        {
            if (_reportConfiguration.showFullName)
            {
                var formAndFields = _workspaceLogic.GetTypesWorkspace().Get<_FormAndFields>()
                                    ?? throw new InvalidOperationException(
                                        "FormAndFields are not found");

                // Create the metaclass as a field
                var fullNamefield = MofFactory.Create(foundForm, formAndFields.__FullNameFieldData);
                fullNamefield.set(_FormAndFields._MetaClassElementFieldData.name, "Path");
                fullNamefield.set(_FormAndFields._MetaClassElementFieldData.title, "Path");
                foundForm.get<IReflectiveSequence>(_FormAndFields._ListForm.field).add(fullNamefield);
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