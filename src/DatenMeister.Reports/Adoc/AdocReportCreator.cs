using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Adoc
{
    public class AdocReportCreator : GenericReportCreator
    {
        private readonly ClassLogger Logger = new ClassLogger(typeof(AdocReportCreator));

        /// <summary>
        /// Gets or sets the text writer to be used for the report reportCreator
        /// </summary>
        public TextWriter TextWriter { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the AdocReportCreator
        /// </summary>
        /// <param name="workspaceLogic">Workspace logic to be used</param>
        /// <param name="scopeStorage">Scope storage to be used</param>
        /// <param name="textWriter">Text writer in which the report shall be stored</param>
        public AdocReportCreator(
            TextWriter textWriter)
        {
            TextWriter = textWriter;
        }

        /// <summary>
        /// Generates a report after the sources has been manually attached
        /// It is important that the reportDefinition value is not of type ReportInstance.
        /// If a report shall be generated upon a Report Instance, use GenerateByInstance
        /// </summary>
        /// <param name="reportDefinition">The report definition to be used</param>
        public override void StartReport(ReportLogic reportLogic, IObject reportDefinition)
        {
            var title = reportDefinition.getOrDefault<string>(_DatenMeister._Reports._ReportDefinition.title);
            if (!string.IsNullOrEmpty(title))
            {
                TextWriter.WriteLine($"= {title}");
                TextWriter.WriteLine(string.Empty);
            }
        }

        public override void EndReport(ReportLogic logic, IObject definition)
        {
        }

        public override void EvaluateElements(ReportLogic reportLogic, IReflectiveCollection reportElements)
        {
            var evaluators =
                reportLogic.ScopeStorage.Get<AdocReportEvaluators>().Evaluators.ToList();

            foreach (var element in reportElements.OfType<IElement>())
            {
                var foundItem =
                    (from x in evaluators
                        where x.IsRelevant(element)
                        select x)
                    .FirstOrDefault();
                if (foundItem != null)
                {
                    foundItem.Evaluate(reportLogic, this, element);
                }
                else
                {
                    Logger.Warn("No evaluator found" + element);
                }
            }
        }
    }
}