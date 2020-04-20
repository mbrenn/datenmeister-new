using System.IO;
using System.Text;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class HtmlReportTests
    {
        [Test]
        public void TestCssLoading()
        {
            var builder = new StringBuilder();
            var memory = new StringWriter(builder);
            var reporter= new HtmlReport(memory);
            
            reporter.SetDefaultCssStyle();
            Assert.That(reporter.CssStyleSheet, Is.Not.Empty);
            Assert.That(reporter.CssStyleSheet, Is.Not.Null);
        }
    }
}