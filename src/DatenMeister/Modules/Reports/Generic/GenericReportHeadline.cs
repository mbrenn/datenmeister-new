using System;
using System.Collections.Generic;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;

namespace DatenMeister.Modules.Reports.Generic
{
    public class GenericReportHeadline
    {
        /// <summary>
        /// Stores the relevant MetaClass
        /// </summary>
        private IElement _relevantMetaClass;

        public GenericReportHeadline()
        {
            _relevantMetaClass = _DatenMeister.TheOne.Reports.__ReportHeadline;
        }


        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_relevantMetaClass) == true;
        }
    }
}
