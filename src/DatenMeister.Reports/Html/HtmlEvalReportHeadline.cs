﻿using DatenMeister.HtmlEngine;
using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Html
{
    public class HtmlEvalReportHeadline : GenericReportHeadline<HtmlReportCreator>
    {
        public override void WriteHeadline(HtmlReportCreator reportCreator, string headline)
        {
            reportCreator.HtmlReporter.Add(new HtmlHeadline(headline, 1));
        }
    }
}