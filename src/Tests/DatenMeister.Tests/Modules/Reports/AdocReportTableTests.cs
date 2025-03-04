﻿using System.Collections.Generic;
using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Reports;
using DatenMeister.Reports.Adoc;
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
            var extent = new MofUriExtent(inMemoryProvider, "dm:///test", scopeStorage);
            workspaceLogic.GetDataWorkspace().AddExtent(extent);

            /* Creates the working object */
            var factory = new MofFactory(extent);
            HtmlReportTableTests.AddData(extent, factory);

            /* Creates the report definition */
            var reportDefinition = factory.create(_DatenMeister.TheOne.Reports.__ReportDefinition);
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.name, "Report Definition");
            extent.elements().add(reportDefinition);

            /* Sets the viewnode */
            var dynamicViewNode = factory.create(_DatenMeister.TheOne.DataViews.__DynamicSourceNode);
            dynamicViewNode.set(_DatenMeister._DataViews._DynamicSourceNode.name, "input");
            extent.elements().add(dynamicViewNode);

            var filterMetaClass = factory.create(_DatenMeister.TheOne.DataViews.__FilterByMetaclassNode);
            filterMetaClass.set(_DatenMeister._DataViews._FilterByMetaclassNode.name, "filter");
            filterMetaClass.set(_DatenMeister._DataViews._FilterByMetaclassNode.metaClass, null);
            filterMetaClass.set(_DatenMeister._DataViews._FilterByMetaclassNode.input, dynamicViewNode);
            extent.elements().add(filterMetaClass);

            /* Create the report paragraph and its corresponding view node */
            var reportHeadline = factory.create(_DatenMeister.TheOne.Reports.Elements.__ReportHeadline);
            reportHeadline.set(_DatenMeister._Reports._Elements._ReportHeadline.title, "This is a headline");

            /* Create the report paragraph and its corresponding view node */
            var reportParagraph = factory.create(_DatenMeister.TheOne.Reports.Elements.__ReportParagraph);
            reportParagraph.set(_DatenMeister._Reports._Elements._ReportParagraph.paragraph, "This is a paragraph");

            /* Create the report paragraph and its corresponding view node */
            var reportTable = factory.create(_DatenMeister.TheOne.Reports.Elements.__ReportTable);

            var form = factory.create(_DatenMeister.TheOne.Forms.__TableForm);
            var field1 = factory.create(_DatenMeister.TheOne.Forms.__EvalTextFieldData)
                .SetProperties(
                    new Dictionary<string, object>
                    {
                        [_DatenMeister._Forms._EvalTextFieldData.name] = "name",
                        [_DatenMeister._Forms._EvalTextFieldData.title] = "Name",
                        [_DatenMeister._Forms._EvalTextFieldData.evalCellProperties] =
                            "{{if(i.age>18)\r\n" +
                            " c.text = c.text + \" (over18)\"\r\n" +
                            "else\r\n" +
                            " c.text = c.text\r\n" +
                            "end}}"
                    });

            var field2 = factory.create(_DatenMeister.TheOne.Forms.__TextFieldData)
                .SetProperties(
                    new Dictionary<string, object>
                    {
                        [_DatenMeister._Forms._TextFieldData.name] = "age",
                        [_DatenMeister._Forms._TextFieldData.title] = "age"
                    });
            form.set(_DatenMeister._Forms._TableForm.field, new[] {field1, field2});

            reportTable.SetProperties(
                new Dictionary<string, object>
                {
                    [_DatenMeister._Reports._Elements._ReportTable.name] = "Table",
                    [_DatenMeister._Reports._Elements._ReportTable.form] = form,
                    [_DatenMeister._Reports._Elements._ReportTable.viewNode] = filterMetaClass
                });

            /* Attached it to the report definition */
            reportDefinition.set(_DatenMeister._Reports._ReportDefinition.elements,
                new[] {reportHeadline, reportParagraph, reportTable});

            /* Creates the report instance */
            var reportInstance = factory.create(_DatenMeister.TheOne.Reports.__AdocReportInstance);
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
            var htmlReport = new AdocReportCreator(writer);
            var logic = new ReportLogic(workspaceLogic, scopeStorage, htmlReport);
            logic.GenerateReportByInstance(reportInstance);

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