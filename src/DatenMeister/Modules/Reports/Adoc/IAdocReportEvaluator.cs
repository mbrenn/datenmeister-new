﻿using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.Reports.Adoc
{
    /// <summary>
    /// Defines the evaluator for the report
    /// </summary>
    public interface IAdocReportEvaluator
    {
        /// <summary>
        /// Gets the information whether the report factory is relevant for the given element 
        /// </summary>
        public bool IsRelevant(IElement element);

        /// <summary>
        /// Performs the evaluation
        /// </summary>
        /// <param name="adocReportCreator">Report creator</param>
        /// <param name="reportNode">The report node</param>
        public void Evaluate(AdocReportCreator adocReportCreator, IElement reportNode, TextWriter writer);
    }
}