using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Reports;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocReportCreator : ReportCreator
    {
        private readonly ClassLogger Logger = new ClassLogger(typeof(AdocReportCreator));
        
        public AdocReportCreator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
            : base(workspaceLogic, scopeStorage)
        {
        }

        /// <summary>
        /// Generates a report after the sources has been manually attached
        /// It is important that the reportDefinition value is not of type ReportInstance.
        /// If a report shall be generated upon a Report Instance, use GenerateByInstance
        /// </summary>
        /// <param name="reportDefinition">The report definition to be used</param>
        /// <param name="writer">The writer being used</param>
        public void GenerateReportByDefinition(IObject reportDefinition, TextWriter writer)
        {
            var title = reportDefinition.getOrDefault<string>(_Reports._ReportDefinition.title);
            if (!string.IsNullOrEmpty(title))
            {
                writer.WriteLine($"= {title}");
            }

            var evaluators = ScopeStorage.Get<AdocReportEvaluators>();

            var elements = reportDefinition.getOrDefault<IReflectiveCollection>(_Reports._ReportDefinition.elements);
            foreach (var element in elements.OfType<IElement>())
            {
                var foundItem =
                    (from x in evaluators.Evaluators
                        where x.IsRelevant(element)
                        select x)
                    .FirstOrDefault();
                if (foundItem != null)
                {
                    foundItem.Evaluate(this, element, writer);
                }
                else
                {
                    Logger.Warn("No evaluator found" + element);
                }
            }
            
        }
    }
}