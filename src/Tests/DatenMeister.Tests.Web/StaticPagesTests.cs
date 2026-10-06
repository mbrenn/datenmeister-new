using System.Text;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.HtmlEngine;
using ElementHeadline = DatenMeister.Web.StaticPages.HtmlElements.HtmlHeadline;
using ElementParagraph = DatenMeister.Web.StaticPages.HtmlElements.HtmlParagraph;
using NUnit.Framework;

namespace DatenMeister.Tests.Web;

[TestFixture]
public class StaticPagesTests
{
    [Test]
    public void TestHtmlHeadlineIsResponsible()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
        var factory = new MofFactory(extent);

        var headlineElement = factory.create(_Reports.TheOne.Elements.__ReportHeadline);
        var paragraphElement = factory.create(_Reports.TheOne.Elements.__ReportParagraph);

        var headlineHandler = new ElementHeadline();
        Assert.That(headlineHandler.IsResponsible(headlineElement), Is.True);
        Assert.That(headlineHandler.IsResponsible(paragraphElement), Is.False);
    }

    [Test]
    public void TestHtmlHeadlineGenerate()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
        var factory = new MofFactory(extent);

        var headlineElement = factory.create(_Reports.TheOne.Elements.__ReportHeadline);
        headlineElement.set(_Reports._Elements._ReportHeadline.title, "My Great Headline");

        var builder = new StringBuilder();
        var memory = new StringWriter(builder);
        var reporter = new HtmlReport(memory);
        reporter.StartReport("Test");

        var headlineHandler = new ElementHeadline();
        headlineHandler.Generate(reporter, headlineElement);

        reporter.EndReport();

        var output = builder.ToString();
        Assert.That(output.Contains("<h1>"), Is.True, output);
        Assert.That(output.Contains("My Great Headline"), Is.True, output);
    }

    [Test]
    public void TestHtmlParagraphIsResponsible()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
        var factory = new MofFactory(extent);

        var headlineElement = factory.create(_Reports.TheOne.Elements.__ReportHeadline);
        var paragraphElement = factory.create(_Reports.TheOne.Elements.__ReportParagraph);

        var paragraphHandler = new ElementParagraph();
        Assert.That(paragraphHandler.IsResponsible(paragraphElement), Is.True);
        Assert.That(paragraphHandler.IsResponsible(headlineElement), Is.False);
    }

    [Test]
    public void TestHtmlParagraphGenerate()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
        var factory = new MofFactory(extent);

        var paragraphElement = factory.create(_Reports.TheOne.Elements.__ReportParagraph);
        paragraphElement.set(_Reports._Elements._ReportParagraph.paragraph, "This is a test paragraph.");
        paragraphElement.set(_Reports._Elements._ReportParagraph.cssClass, "lead-text");

        var builder = new StringBuilder();
        var memory = new StringWriter(builder);
        var reporter = new HtmlReport(memory);
        reporter.StartReport("Test");

        var paragraphHandler = new ElementParagraph();
        paragraphHandler.Generate(reporter, paragraphElement);

        reporter.EndReport();

        var output = builder.ToString();
        Assert.That(output.Contains("<p"), Is.True, output);
        Assert.That(output.Contains("class=\"lead-text\""), Is.True, output);
        Assert.That(output.Contains("This is a test paragraph."), Is.True, output);
    }
}
