﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.HtmlEngine;
using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Html
{
    /// <summary>
    /// Creates the report
    /// </summary>
    public class HtmlReportCreator : GenericReportCreator
    {
        private readonly ClassLogger Logger = new(typeof(HtmlReportCreator));

        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        private readonly Dictionary<string, IReflectiveCollection> _sources = new();

        private HtmlReport? _htmlReporter;

        public TextWriter TextWriter { get; set; }

        /// <summary>
        /// Inhibits the creation for CSS Style sheets
        /// </summary>
        public bool EmbedInExistingPage
        {
            get;set;
        }

        public HtmlReportCreator(TextWriter textWriter)
        {
            TextWriter = textWriter;
        }

        public HtmlReport HtmlReporter =>
            _htmlReporter ?? throw new InvalidOperationException("_htmlReporter is null");


        public override void StartReport(ReportLogic logic, IElement reportInstance, IObject reportDefinition)
        {
            _htmlReporter = new HtmlReport(TextWriter);

            var title = reportDefinition.getOrDefault<string>(_DatenMeister._Reports._ReportDefinition.title);

            if (!EmbedInExistingPage)
            {
                _htmlReporter.SetDefaultCssStyle();

                var cssFile = reportInstance.getOrDefault<string>(_DatenMeister._Reports._HtmlReportInstance.cssFile);
                if (!string.IsNullOrEmpty(cssFile))
                {
                    _htmlReporter.AddCssFile(cssFile);
                }

                var cssStyleSheet = reportInstance.getOrDefault<string>(_DatenMeister._Reports._HtmlReportInstance.cssStyleSheet);
                if (!string.IsNullOrEmpty(cssStyleSheet))
                {
                    _htmlReporter.AddCssStyleSheet(cssStyleSheet);
                }

                _htmlReporter.StartReport(title);
            }
        }

        public override void EndReport(ReportLogic logic, IObject definition)
        {
            if (!EmbedInExistingPage)
            {
                _htmlReporter?.EndReport();
            }
        }

        /// <summary>
        /// Generates a report after the sources has been manually attached
        /// It is important that the reportDefinition value is not of type ReportInstance.
        /// If a report shall be generated upon a Report Instance, use GenerateByInstance
        /// </summary>
        /// <param name="reportLogic">The report-logic to be used</param>
        /// <param name="reportElements">The reportElements to be used</param>
        public override void EvaluateElements(
            ReportLogic reportLogic, 
            IReflectiveCollection reportElements)
        {
            var evaluators = reportLogic.ScopeStorage.Get<HtmlReportEvaluators>();
            foreach (var element in reportElements.OfType<IElement>())
            {
                var foundItem =
                    (from x in evaluators.Evaluators
                        where x.IsRelevant(element)
                        select x)
                    .FirstOrDefault();

                if (foundItem != null)
                {
                    foundItem.Evaluate(reportLogic, this, element);
                }
                else
                {
                    Logger.Warn(
                        "No evaluator found for: "
                        + element
                        + " ("
                        + (element.getMetaClass()?.ToString() ?? "No Metaclass")
                        + ")");
                }
            }
        }
    }
}