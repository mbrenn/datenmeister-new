using System.Collections.Generic;
using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Models;
using DatenMeister.Models.DataViews;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Reports.Adoc;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Reports
{
    [TestFixture]
    public class AdocReportTableTests
    {
        [Test]
        public void TestHeadlineParagraphAndTable()
        {
            var (scopeStorage, workspaceLogic) = HtmlReportTests.PrepareWorkspaceLogic();

            var inMemoryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(inMemoryProvider, "dm:///test");
            workspaceLogic.GetDataWorkspace().AddExtent(extent);

            /* Creates the working object */
            var factory = new MofFactory(extent);
            HtmlReportTableTests.AddData(extent, factory);

            /* Creates the report definition */
            var reportDefinition = factory.create(_DatenMeister.TheOne.Reports.__ReportDefinition);
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.name, "Report Definition");
            extent.elements().add(reportDefinition);

            /* Sets the viewnode */
            var dynamicViewNode = factory.create(_DataViews.TheOne.__DynamicSourceNode);
            dynamicViewNode.set(_DataViews._DynamicSourceNode.name, "input");
            extent.elements().add(dynamicViewNode);

            var filterMetaClass = factory.create(_DataViews.TheOne.__FilterTypeNode);
            filterMetaClass.set(_DataViews._FilterTypeNode.name, "filter");
            filterMetaClass.set(_DataViews._FilterTypeNode.type, null);
            filterMetaClass.set(_DataViews._FilterTypeNode.input, dynamicViewNode);
            extent.elements().add(filterMetaClass);
            
            /* Create the report paragraph and its corresponding view node */
            var reportHeadline = factory.create(_DatenMeister.TheOne.Reports.__ReportHeadline);
            reportHeadline.set(_DatenMeister._Reports._ReportHeadline.title, "This is a headline");
            
            /* Create the report paragraph and its corresponding view node */
            var reportParagraph = factory.create(_DatenMeister.TheOne.Reports.__ReportParagraph);
            reportParagraph.set(_DatenMeister._Reports._ReportParagraph.paragraph, "This is a paragraph");

            /* Create the report paragraph and its corresponding view node */
            var reportTable = factory.create(_DatenMeister.TheOne.Reports.__ReportTable);

            var form = factory.create(_FormAndFields.TheOne.__ListForm);
            var field1 = factory.create(_FormAndFields.TheOne.__EvalTextFieldData)
                .SetProperties(
                    new Dictionary<string, object>
                    {
                        [_FormAndFields._EvalTextFieldData.name] = "name",
                        [_FormAndFields._EvalTextFieldData.title] = "Name",
                        [_FormAndFields._EvalTextFieldData.evalCellProperties] =
                            "if(i.age>18)\r\n" +
                            " c.text = c.text + \" (over18)\"\r\n" +
                            "else\r\n" +
                            " c.text = c.text\r\n" +
                            "end"
                    });
            
            var field2 = factory.create(_FormAndFields.TheOne.__TextFieldData)
                .SetProperties(
                    new Dictionary<string, object>
                    {
                        [_FormAndFields._TextFieldData.name] = "age",
                        [_FormAndFields._TextFieldData.title] = "age"
                    });
            form.set(_FormAndFields._ListForm.field, new[] {field1, field2});

            reportTable.SetProperties(
                new Dictionary<string, object>
                {
                    [_DatenMeister._Reports._ReportTable.name] = "Table",
                    [_DatenMeister._Reports._ReportTable.form] = form,
                    [_DatenMeister._Reports._ReportTable.viewNode] = filterMetaClass
                });

            /* Attached it to the report definition */
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.elements, new[] {reportHeadline, reportParagraph, reportTable});

            /* Creates the report instance */
            var reportInstance = factory.create(_DatenMeister.TheOne.Reports.__AdocReportInstance);
            extent.elements().add(reportInstance);
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.name, "Report");

            var source = factory.create(_DatenMeister.TheOne.Reports.__ReportInstanceSource);
            source.set(_DatenMeister._Reports._ReportInstanceSource.name, "input");
            source.set(_DatenMeister._Reports._ReportInstanceSource.source, "dm:///test");
            source.set(_DatenMeister._Reports._ReportInstanceSource.workspaceId, "Data");
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.sources, new[] {source});
            reportInstance.set(_DatenMeister._Reports._HtmlReportInstance.reportDefinition, reportDefinition);

            /* Now create the report */
            var writer = new StringWriter();
            var htmlReport = new AdocReportCreator(workspaceLogic, scopeStorage);
            htmlReport.GenerateReportByInstance(reportInstance, writer);

            var asString = writer.ToString();
            Assert.That(asString.Contains("headline"), Is.True);
            Assert.That(asString.Contains("paragraph"), Is.True);
            Assert.That(asString.Contains("Father"), Is.True);
            Assert.That(asString.Contains("Mother"), Is.True);
            Assert.That(asString.Contains("Child1"), Is.True);
            Assert.That(asString.Contains("Child2"), Is.True);
            Assert.That(asString.Contains("Child3"), Is.True);
            Assert.That(asString.Contains("34"), Is.True);
            Assert.That(asString.Contains("over18"), Is.True);
        }
    }
}