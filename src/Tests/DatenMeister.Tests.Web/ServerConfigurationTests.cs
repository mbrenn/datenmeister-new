using System.Xml.Linq;
using DatenMeister.Core;
using DatenMeister.WebServer.Library.ServerConfiguration;
using NUnit.Framework;

namespace DatenMeister.Tests.Web;

[TestFixture]
public class ServerConfigurationTests
{
    [Test]
    public void TestLoadingOfServerConfiguration()
    {
        var text = @"<xmi><settings databasePath=""test"">
<web backgroundColor='#ffeedd' /></settings></xmi>";
        var xmlDocument = XDocument.Parse(text);

        var configurationExtent = ConfigurationLoader.LoadConfigurationByXml(xmlDocument);
        var webConfiguration = WebServerSettingsLoader.LoadSettingsFromExtent(configurationExtent);
        
        Assert.That(webConfiguration.backgroundColor, Is.EqualTo("#ffeedd"));
    }
}