using System.Collections.Generic;
using System.IO;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports
{
    /// <summary>
    /// Creates the report
    /// </summary>
    public class HtmlReportCreator
    {
        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        private Dictionary<string, IReflectiveCollection> _sources = new Dictionary<string, IReflectiveCollection>();
        
        private  HtmlReport? _htmlReporter;
        
        /// <summary>
        /// Adds the source to the report
        /// </summary>
        /// <param name="id">Id of the source</param>
        /// <param name="collection">Collection to be evaluated</param>
        public void AddSource(string id, IReflectiveCollection collection)
        {
            _sources[id] = collection;
        }

        public void GenerateReport(IElement reportDefinition, Stream report)
        {
            _htmlReporter = new HtmlReport(report);
            
            var title = reportDefinition.getOrDefault<string>(_Reports._ReportDefinition.title);
            _htmlReporter.StartReport(title);
            _htmlReporter.EndReport();
        }
    }
}