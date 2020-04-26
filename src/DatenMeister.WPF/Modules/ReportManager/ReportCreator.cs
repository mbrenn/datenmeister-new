using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.HtmlReporter.Formatter;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Uml.Helper;

namespace DatenMeister.WPF.Modules.ReportManager
{
    /// <summary>
    /// Defines the engine for simple reporting
    /// </summary>
    public class ReportCreator
    {
        /// <summary>
        /// Stores the default classifier hints
        /// </summary>
        private readonly DefaultClassifierHints _defaultClassifierHints;
        
        /// <summary>
        /// Initializes a new instance of the ReportCreator class.
        /// </summary>
        /// <param name="defaultClassifierHints">Default classifiers to be used</param>
        public ReportCreator(DefaultClassifierHints defaultClassifierHints)
        {
            _defaultClassifierHints = defaultClassifierHints;
        }
        
        public void CreateReport(TextWriter textWriter, ReportConfiguration configuration)
        {
            var rootElement = configuration.rootElement;
            var form = configuration.form;
            if (rootElement == null)
                throw new InvalidOperationException("rootElement is null");

            if (form == null)
                throw new InvalidOperationException("form is null");

            using (var report = new HtmlReport(textWriter))
            {
                var elements = _defaultClassifierHints.GetPackagedElements(rootElement);
                report.StartReport("Extent: " + NamedElementMethods.GetName(configuration.rootElement));
                report.Add(new HtmlHeadline("Items in collection", 1));
                var itemFormatter = new ItemFormatter(report);
                itemFormatter.FormatCollectionOfItems(elements, form);
                report.EndReport();
            }
        }
    }
}