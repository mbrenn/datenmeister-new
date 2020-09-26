using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
using DatenMeister.Provider.DotNet;
// Created by $DatenMeister.SourcecodeGenerator.DotNetIntegrationGenerator
// ReSharper disable RedundantNameQualifier

namespace DatenMeister.Models.Reports
{
    public static class IntegrateReports
    {
        /// <summary>
        /// Assigns the types of form and fields by converting the 
        /// .Net objects to DatenMeister elements and adds them into 
        /// the filler, the collection and also into the lookup. 
        /// </summary>
        /// <param name="uml">The uml metamodel to be used</param>
        /// <param name="factory">Factory being used for creation</param>
        /// <param name="collection">Collection that shall be filled</param>
        /// <param name="extent">And finally extent to which the types shall be registered</param>
        public static void Assign(IFactory factory, IReflectiveCollection collection, MofExtent extent)
        {
            var generator = new DotNetTypeGenerator(factory, extent);
            {
                var type = typeof(DatenMeister.Models.Reports.ReportDefinition);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.ReportElement);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.ReportHeadline);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.ReportParagraph);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.ReportTable);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.ReportInstanceSource);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.ReportInstance);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.Adoc.AdocReportInstance);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.Html.HtmlReportInstance);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.Simple.DescendentMode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.Simple.ReportTableForTypeMode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Reports.Simple.SimpleReportConfiguration);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
        }
    }
}
