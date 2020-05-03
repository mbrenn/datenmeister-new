using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BurnSystems;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Modules.HtmlReporter.Formatter;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Provider.XMI;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Reports
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

        private readonly FormCreator _formCreator;

        /// <summary>
        /// Initializes a new instance of the ReportCreator class.
        /// </summary>
        /// <param name="workspaceLogic">Default workspace Logic to be used</param>
        public ReportCreator(IWorkspaceLogic workspaceLogic)
        {
            _defaultClassifierHints = new DefaultClassifierHints(workspaceLogic);
            _formCreator = new FormCreator(workspaceLogic, null, _defaultClassifierHints);
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
        
        /// <summary>
        /// Creates the html report according the given form and elements
        /// </summary>
        /// <param name="textWriter">Text Writer to be used for html file creation</param>
        /// <param name="configuration">Configuration of the report</param>
        public void CreateReport(TextWriter textWriter, ReportConfiguration configuration)
        {
            var creationMode = configuration.showMetaClasses
                ? CreationMode.All
                : CreationMode.All & ~CreationMode.AddMetaClass;
            
            var rootElement = configuration.rootElement;
            var form = configuration.form;
            if (rootElement == null)
                throw new InvalidOperationException("rootElement is null");
            

            using (var report = new HtmlReport(textWriter))
            {
                report.SetDefaultCssStyle();

                if (configuration.showRootElement)
                {
                    report.Add(new HtmlHeadline("Reported Item", 1));
                    var itemFormatter = new ItemFormatter(report);
                    var detailForm =
                        _formCreator.CreateDetailForm(rootElement, creationMode);
                    itemFormatter.FormatItem(rootElement, detailForm);
                }

                // And now start the report
                report.StartReport("Extent: " + NamedElementMethods.GetName(configuration.rootElement));
                
                // First, gets the elements to be shown
                IReflectiveCollection elements =
                    new TemporaryReflectiveCollection(_defaultClassifierHints.GetPackagedElements(rootElement));
                if (configuration.showDescendents)
                {
                    elements = elements.GetAllCompositesIncludingThemselves();
                }

                var first = (elements.FirstOrDefault(x => x is IElement) as IElement)?.metaclass;
                Debug.WriteLine(first);
                
                // Splits them up by metaclasses 
                var metaClasses =
                    elements.GroupBy(
                        x => x is IElement element ? element.metaclass : null,
                    new MofObjectEqualityComparer()).ToList();
                
                report.Add(new HtmlHeadline("Items in collection", 1));
                foreach (var metaClass in metaClasses)
                {
                    // Gets the name of the metaclass
                    var metaClassName = metaClass.Key == null
                        ? "Unclassified"
                        : "Classifier: " + NamedElementMethods.GetName(metaClass.Key);
                    
                    var itemFormatter = new ItemFormatter(report);
                    
                    report.Add(new HtmlHeadline(metaClassName, 2));

                    // Gets the reflective sequence for the name
                    var collection = new TemporaryReflectiveSequence(metaClass);
                    var foundForm = form;
                    if (foundForm == null)
                    {
                        foundForm = _formCreator.CreateListFormForElements(
                            collection,
                            creationMode);
                    }

                    itemFormatter.FormatCollectionOfItems(collection, foundForm);
                }

                report.EndReport();
            }
        }
    }
}