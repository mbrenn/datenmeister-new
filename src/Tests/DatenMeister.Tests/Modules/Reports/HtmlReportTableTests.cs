using System.Text.RegularExpressions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Reports;
using DatenMeister.Reports.Html;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Reports;

[TestFixture]
public class HtmlReportTableTests
{
    [Test]
    public void TestHtmlTableCreationWithCellEvaluation()
    {
        var (scopeStorage, workspaceLogic) = HtmlReportTests.PrepareWorkspaceLogic();

        var inMemoryProvider = new InMemoryProvider();
        var extent = new MofUriExtent(inMemoryProvider, "dm:///test", scopeStorage);
        workspaceLogic.GetDataWorkspace().AddExtent(extent);

        /* Creates the working object */
        var factory = new MofFactory(extent);
        AddData(extent, factory);

        /* Creates the report definition */
        var reportDefinition = factory.create(_Reports.TheOne.__ReportDefinition);
        reportDefinition.set(_Reports._ReportDefinition.name, "Report Definition");
        extent.elements().add(reportDefinition);

        /* Sets the viewnode */
        var dynamicViewNode = factory.create(_DataViews.TheOne.__DynamicSourceNode);
        dynamicViewNode.set(_DataViews._DynamicSourceNode.name, "input");
        extent.elements().add(dynamicViewNode);

        var filterMetaClass = factory.create(_DataViews.TheOne.__RowFilterByMetaclassNode);
        filterMetaClass.set(_DataViews._RowFilterByMetaclassNode.name, "filter");
        filterMetaClass.set(_DataViews._RowFilterByMetaclassNode.metaClass, null);
        filterMetaClass.set(_DataViews._RowFilterByMetaclassNode.input, dynamicViewNode);
        extent.elements().add(filterMetaClass);

        /* Create the report paragraph and its corresponding view node */
        var reportTable = factory.create(_Reports.TheOne.Elements.__ReportTable);

        var form = factory.create(_Forms.TheOne.__TableForm);
        var field = factory.create(_Forms.TheOne.__EvalTextFieldData)
            .SetProperties(
                new Dictionary<string, object>
                {
                    [_Forms._EvalTextFieldData.name] = "name",
                    [_Forms._EvalTextFieldData.title] = "Name",
                    [_Forms._EvalTextFieldData.evalCellProperties] =
                        "{{if(i.age>18)\r\n" +
                        " c.cssClass=\"over18\"\r\n" +
                        "else\r\n" +
                        " c.cssClass=\"under18\"\r\n" +
                        "end}}"
                });
        form.set(_Forms._TableForm.field, new[] {field});

        reportTable.SetProperties(
            new Dictionary<string, object>
            {
                [_Reports._Elements._ReportTable.name] = "Table",
                [_Reports._Elements._ReportTable.form] = form,
                [_Reports._Elements._ReportTable.viewNode] = filterMetaClass
            });

        /* Attached it to the report definition */
        reportDefinition.set(_Reports._ReportDefinition.elements, new[] {reportTable});

        /* Creates the report instance */
        var reportInstance = factory.create(_Reports.TheOne.__HtmlReportInstance);
        extent.elements().add(reportInstance);
        reportInstance.set(_Reports._HtmlReportInstance.name, "Report");

        var source = factory.create(_Reports.TheOne.__ReportInstanceSource);
        source.set(_Reports._ReportInstanceSource.name, "input");
        source.set(_Reports._ReportInstanceSource.path, "dm:///test");
        source.set(_Reports._ReportInstanceSource.workspaceId, "Data");
        reportInstance.set(_Reports._HtmlReportInstance.sources, new[] {source});
        reportInstance.set(_Reports._HtmlReportInstance.reportDefinition, reportDefinition);

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