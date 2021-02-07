using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Models;
using DatenMeister.Modules.Reports;
using DatenMeister.Modules.Reports.Html;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Reports
{
    [TestFixture]
    public class HtmlReportTableTests
    {
        [Test]
        public void TestHtmlTableCreationWithCellEvaluation()
        {
            var (scopeStorage, workspaceLogic) = HtmlReportTests.PrepareWorkspaceLogic();

            var inMemoryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(inMemoryProvider, "dm:///test");
            workspaceLogic.GetDataWorkspace().AddExtent(extent);

            /* Creates the working object */
            var factory = new MofFactory(extent);
            AddData(extent, factory);

            /* Creates the report definition */
            var reportDefinition = factory.create(_DatenMeister.TheOne.Reports.__ReportDefinition);
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.name, "Report Definition");
            extent.elements().add(reportDefinition);

            /* Sets the viewnode */
            var dynamicViewNode = factory.create(_DatenMeister.TheOne.DataViews.__DynamicSourceNode);
            dynamicViewNode.set(_DatenMeister._DataViews._DynamicSourceNode.name, "input");
            extent.elements().add(dynamicViewNode);
            
            var filterMetaClass = factory.create(_DatenMeister.TheOne.DataViews.__FilterTypeNode);
            filterMetaClass.set(_DatenMeister._DataViews._FilterTypeNode.name, "filter");
            filterMetaClass.set(_DatenMeister._DataViews._FilterTypeNode.type, null);
            filterMetaClass.set(_DatenMeister._DataViews._FilterTypeNode.input, dynamicViewNode);
            extent.elements().add(filterMetaClass);

            /* Create the report paragraph and its corresponding view node */
            var reportTable = factory.create(_DatenMeister.TheOne.Reports.Elements.__ReportTable);

            var form = factory.create(_DatenMeister.TheOne.Forms.__ListForm);
            var field = factory.create(_DatenMeister.TheOne.Forms.__EvalTextFieldData)
                .SetProperties(
                    new Dictionary<string, object>
                    {
                        [_DatenMeister._Forms._EvalTextFieldData.name] = "name",
                        [_DatenMeister._Forms._EvalTextFieldData.title] = "Name",
                        [_DatenMeister._Forms._EvalTextFieldData.evalCellProperties] =
                            "{{if(i.age>18)\r\n" +
                            " c.cssClass=\"over18\"\r\n" +
                            "else\r\n" +
                            " c.cssClass=\"under18\"\r\n" +
                            "end}}"
                    });
            form.set(_DatenMeister._Forms._ListForm.field, new[] {field});

            reportTable.SetProperties(
                new Dictionary<string, object>
                {
                    [_DatenMeister._Reports._Elements._ReportTable.name] = "Table",
                    [_DatenMeister._Reports._Elements._ReportTable.form] = form,
                    [_DatenMeister._Reports._Elements._ReportTable.viewNode] = filterMetaClass
                });

            /* Attached it to the report definition */
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.elements, new[] {reportTable});

            /* Creates the report instance */
            var reportInstance = factory.create(_DatenMeister.TheOne.Reports.__HtmlReportInstance);
            extent.elements().add(reportInstance);
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.name, "Report");

            var source = factory.create(_DatenMeister.TheOne.Reports.__ReportInstanceSource);
            source.set(_DatenMeister._Reports._ReportInstanceSource.name, "input");
            source.set(_DatenMeister._Reports._ReportInstanceSource.path, "dm:///test");
            source.set(_DatenMeister._Reports._ReportInstanceSource.workspaceId, "Data");
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.sources, new[] {source});
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.reportDefinition, reportDefinition);

            /* Now create the report */
            var writer = new StringWriter();
            var htmlReport = new HtmlReportCreator(writer);
            var htmlReportLogic = new ReportLogic(workspaceLogic, scopeStorage, htmlReport);
            htmlReportLogic.GenerateReportByInstance(reportInstance);

            var asString = writer.ToString();
            Assert.That(asString.Contains("Father"), Is.True);
            Assert.That(asString.Contains("Mother"), Is.True);
            Assert.That(asString.Contains("Child1"), Is.True);
            Assert.That(asString.Contains("Child2"), Is.True);
            Assert.That(asString.Contains("Child3"), Is.True);
            Assert.That(asString.Contains("over18"), Is.True);
            Assert.That(asString.Contains("under18"), Is.True);

            Assert.That(Regex.Matches(asString, "over18").Count, Is.EqualTo(2));
            Assert.That(Regex.Matches(asString, "under18").Count, Is.EqualTo(3));
        }

        public static void AddData(MofUriExtent extent, MofFactory factory)
        {
            extent.elements().add(factory.create(null)
                .SetProperties(new Dictionary<string, object>
                {
                    ["name"] = "Father",
                    ["age"] = 34
                }));
            extent.elements().add(factory.create(null)
                .SetProperties(new Dictionary<string, object>
                {
                    ["name"] = "Mother",
                    ["age"] = 32
                }));
            extent.elements().add(factory.create(null)
                .SetProperties(new Dictionary<string, object>
                {
                    ["name"] = "Child1",
                    ["age"] = 8
                }));
            extent.elements().add(factory.create(null)
                .SetProperties(new Dictionary<string, object>
                {
                    ["name"] = "Child2",
                    ["age"] = 9
                }));
            extent.elements().add(factory.create(null)
                .SetProperties(new Dictionary<string, object>
                {
                    ["name"] = "Child3",
                    ["age"] = 15
                }));
        }
    }
}