using System;
using System.Data;
using System.IO;
using BurnSystems;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Modules.HtmlReporter.Formatter;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Reports
{
    /// <summary>
    /// Defines the engine for simple reporting
    /// </summary>
    public class ReportCreator
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Stores the default classifier hints
        /// </summary>
        private readonly DefaultClassifierHints _defaultClassifierHints;

        /// <summary>
        /// Initializes a new instance of the ReportCreator class.
        /// </summary>
        /// <param name="workspaceLogic">Default workspace Logic to be used</param>
        public ReportCreator(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
            _defaultClassifierHints = new DefaultClassifierHints(workspaceLogic);
        }

        /// <summary>
        /// Creates a random file and returns the stream writing instance
        /// to the file.
        /// </summary>
        /// <param name="directoryPath">Sets the directory path</param>
        /// <param name="path">The path to the created file</param>
        /// <returns>The streamwriter for the file</returns>
        public static TextWriter CreateRandomFile(out string path, string? directoryPath = null)
        {
            if (directoryPath == null)
            {
                path = StringManipulation.RandomString(10) + ".html";
            }
            else
            {
                path = Path.Combine(directoryPath, StringManipulation.RandomString(10) + ".html");
            }

            return new StreamWriter(path);
        }
        
        public void CreateReport(TextWriter textWriter, ReportConfiguration configuration)
        {
            var rootElement = configuration.rootElement;
            var form = configuration.form;
            if (rootElement == null)
                throw new InvalidOperationException("rootElement is null");

            using (var report = new HtmlReport(textWriter))
            {
                report.SetDefaultCssStyle();
                
                var elements = _defaultClassifierHints.GetPackagedElements(rootElement);
                var collection = new TemporaryReflectiveSequence(elements);
                report.StartReport("Extent: " + NamedElementMethods.GetName(configuration.rootElement));
                report.Add(new HtmlHeadline("Items in collection", 1));
                var itemFormatter = new ItemFormatter(report);

                var foundForm = form;
                if (foundForm == null)
                {
                    var formCreator = new FormCreator(_workspaceLogic, null, _defaultClassifierHints);
                    foundForm = formCreator.CreateListFormForElements(
                        collection,
                        CreationMode.All);
                }

                itemFormatter.FormatCollectionOfItems(collection, foundForm);
                report.EndReport();
            }
        }
    }
}