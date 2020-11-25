using System;
using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Modules.Reports.Generic;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocGenericReportCreator : GenericReportCreator
    {
        private readonly ClassLogger Logger = new ClassLogger(typeof(AdocGenericReportCreator));
        private TextWriter _textWriter;

        /// <summary>
        /// Gets or sets the text writer to be used for the report creator
        /// </summary>
        public TextWriter TextWriter
        {
            get => _textWriter ?? throw new InvalidOperationException("TextWriter == null");
            set => _textWriter = value;
        }

        public AdocGenericReportCreator(
            IWorkspaceLogic workspaceLogic, 
            IScopeStorage scopeStorage)
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
        public override void GenerateReportByDefinition(IObject reportDefinition, TextWriter writer)
        {
            TextWriter = writer;

            var title = reportDefinition.getOrDefault<string>(_DatenMeister._Reports._ReportDefinition.title);
            if (!string.IsNullOrEmpty(title))
            {
                TextWriter.WriteLine($"= {title}");
                TextWriter.WriteLine(string.Empty);
            }

            var evaluators = ScopeStorage.Get<AdocReportEvaluators>();

            var elements = reportDefinition.getOrDefault<IReflectiveCollection>(
                _DatenMeister._Reports._ReportDefinition.elements);

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