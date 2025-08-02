using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Functions;

[TestFixture]
public class QueryColumnTests
{
    [Test]
    public void TestExcludeColumns()
    {
        var testExtent = QueryTests.CreateQueryTestExtent();

        Assert.That(testExtent.elements().Count(), Is.GreaterThan(0));
        foreach (var element in testExtent.elements().OfType<IElement>())
        {
            Assert.That(element.isSet("iq"), Is.True);
        }
        
        var excludeColumns = new FilterColumnsExclude(testExtent.elements())
        {
            ExcludedColumns = ["age", "iq"]
        };
        
        foreach (var element in excludeColumns.OfType<IElement>())
        {
            Assert.That(element.isSet("iq"), Is.False);
        }
    }
    
    [Test]
    public void TestIncludeColumns()
    {
        var testExtent = QueryTests.CreateQueryTestExtent();

        Assert.That(testExtent.elements().Count(), Is.GreaterThan(0));
        foreach (var element in testExtent.elements().OfType<IElement>())
        {
            Assert.That(element.isSet("iq"), Is.True);
            Assert.That(element.isSet("name"), Is.True);
        }
        
        var excludeColumns = new FilterColumnsIncludeOnly(testExtent.elements())
        {
            IncludeColumns = ["name"]
        };
        
        foreach (var element in excludeColumns.OfType<IElement>())
        {
            Assert.That(element.isSet("iq"), Is.False);
            Assert.That(element.isSet("name"), Is.True);
        }
    }
}