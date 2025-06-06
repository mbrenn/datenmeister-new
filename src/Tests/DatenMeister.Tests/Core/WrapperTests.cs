using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Core;

[TestFixture]
public class WrapperTests
{
    [Test]
    public void TestSimpleWrapper()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///a", null);
        var factory = new MofFactory(extent);
        var variable = factory.create(_CommonTypes.TheOne.OSIntegration.__EnvironmentalVariable);
        var wrapper = new CommonTypes.OSIntegration.EnvironmentalVariable_Wrapper(variable)
        {
            name = "Test",
            value = "TestValue"
        };

        Assert.That(wrapper.name, Is.EqualTo("Test"));
        Assert.That(wrapper.value, Is.EqualTo("TestValue"));

        Assert.That(wrapper.GetWrappedElement(), Is.Not.Null);
        Assert.That(
            wrapper.GetWrappedElement().getOrDefault<string>(_CommonTypes._OSIntegration._EnvironmentalVariable.name),
            Is.EqualTo("Test"));

        wrapper.GetWrappedElement().set(_CommonTypes._OSIntegration._EnvironmentalVariable.name, "New Value");
        Assert.That(wrapper.name, Is.EqualTo("New Value"));
    }

    [Test]
    public void TestWrapperWithBoolean()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///a", null);
        var factory = new MofFactory(extent);
        var variable = factory.create(_Actions.TheOne.__ActionSet);
        var wrapper = new DatenMeister.Core.Models.Actions.ActionSet_Wrapper(variable)
        {
            name = "Test"
        };
        
        Assert.That(wrapper.isDisabled, Is.False);
        wrapper.isDisabled = true;

        Assert.That(wrapper.name, Is.EqualTo("Test"));
        Assert.That(wrapper.isDisabled, Is.True); 
    }

    [Test]
    public void TestSimpleObjectWrapper()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///a", null);
        var factory = new MofFactory(extent);
        var configuration  = factory.create(_Reports.TheOne.__SimpleReportConfiguration);
        var configurationWrapper =
            new DatenMeister.Core.Models.Reports.SimpleReportConfiguration_Wrapper(configuration)
            {
                name = "Test",
                showDescendents = true
            };

        var variable = factory.create(_Actions.TheOne.Reports.__SimpleReportAction);
        var wrapper = new DatenMeister.Core.Models.Actions.Reports.SimpleReportAction_Wrapper(variable)
        {
            isDisabled = true
        };
        
        Assert.That(wrapper.isDisabled, Is.True);
        wrapper.isDisabled = true;

        Assert.That(configurationWrapper.name, Is.EqualTo("Test"));
        Assert.That(wrapper.name, Is.Null);
        Assert.That(wrapper.isDisabled, Is.True);

        Assert.That(wrapper.configuration, Is.Null);
        
        wrapper.configuration = configurationWrapper;
        Assert.That(wrapper.configuration, Is.Not.Null);
        Assert.That(wrapper.configuration.name, Is.EqualTo("Test"));
        Assert.That(wrapper.configuration.showDescendents, Is.True);
    }
}