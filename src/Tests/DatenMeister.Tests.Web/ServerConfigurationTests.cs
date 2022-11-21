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

    [Test]
    public void TestTheOne()
    {
        Assert.That(WebServerSettingHandler.TheOne.WebServerSettings, Is.Not.Null);
        var settings = new WebServerSettings { backgroundColor = "TEST" };
        WebServerSettingHandler.TheOne.WebServerSettings = settings;
        
        Assert.That(WebServerSettingHandler.TheOne.WebServerSettings.backgroundColor, Is.EqualTo("TEST"));
    }

    [Test]
    public void TestBackgroundColor()
    {
        var settings = new WebServerSettings();
        var handler = new WebServerSettingHandler
        {
            WebServerSettings = settings
        };
        Assert.That(handler.BodyCssStyle, Is.EqualTo(string.Empty));

        settings.backgroundColor = "#000000";
        Assert.That(handler.BodyCssStyle.ToLower().StartsWith("background-color"), Is.True);
        Assert.That(handler.BodyCssStyle.ToLower().Contains("#000000"), Is.True);

    }
}