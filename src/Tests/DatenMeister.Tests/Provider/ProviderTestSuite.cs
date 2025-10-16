using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Provider;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider;

public class ProviderTestSuite
{
    /// <summary>
    /// Performs a set of tests on the provider object.
    /// This can be used to evaluate the capability of the provider
    /// </summary>
    /// <param name="provider"></param>
    public static void TestProviderObject(IProvider provider)
    {
        var element = provider.CreateElement(null);
        Assert.That(element, Is.Not.Null);

        element.SetProperty("name", "Martin");
        element.SetProperty("age", 40);
        element.SetProperty("nextday", new DateTime(2022, 04, 19));
        element.SetProperty("nothing", null);
        element.SetProperty("isMale", true);
        element.SetProperty("isFemale", false);

        // Now gets the check
        var name = element.GetProperty("name", ObjectType.String);
        var age = element.GetProperty("age", ObjectType.Integer);
        var nextDay = element.GetProperty("nextday", ObjectType.DateTime);
        var nothing = element.GetProperty("nothing");
        var isMale = element.GetProperty("isMale", ObjectType.Boolean);
        var isFemale = element.GetProperty("isFemale", ObjectType.Boolean);

        Assert.That(name, Is.EqualTo("Martin"));
        Assert.That(age, Is.EqualTo(40));
        Assert.That(nextDay, Is.EqualTo(new DateTime(2022, 04, 19)));
        Assert.That(nothing, Is.Null);
        Assert.That(isMale, Is.True);
        Assert.That(isFemale, Is.False);
            
        // Check, that we are case sensitive
        Assert.That(element.IsPropertySet("nextday"), Is.True);
        Assert.That(element.IsPropertySet("nextDay"), Is.False);

        TestListsWithObjects(provider);
        TestListMovement(provider);
        TestSetReferenceAndSetValue(provider);
    }

    public static void TestListsWithObjects(IProvider provider)
    {
        var element = provider.CreateElement(null);
        var child1 = provider.CreateElement(null);
        child1.SetProperty("name", "a");
        var child2 = provider.CreateElement(null);
        child2.SetProperty("name", "b");
        var child3 = provider.CreateElement(null);
        child3.SetProperty("name", "c");

        element.AddToProperty("list", child1);
        element.AddToProperty("list", child2);
        element.AddToProperty("list", child3);

        var asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list"))!.OfType<IProviderObject>()
            .ToList();
        Assert.That(asEnumerable.Count, Is.EqualTo(3));

        Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("a"));
        Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("b"));
        Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));

        element.RemoveFromProperty("list", child2);
        asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list"))!.OfType<IProviderObject>().ToList();
        Assert.That(asEnumerable.Count, Is.EqualTo(2));

        Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("a"));
        Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("c"));

        element.EmptyListForProperty("list");
        asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list", ObjectType.ReflectiveSequence))!.OfType<IProviderObject>().ToList();
        Assert.That(asEnumerable.Count, Is.EqualTo(0));
    }
        
        
    public static void TestListMovement(IProvider provider)
    {
        var element = provider.CreateElement(null);
        var child1 = provider.CreateElement(null);
        child1.SetProperty("name", "a");
        var child2 = provider.CreateElement(null);
        child2.SetProperty("name", "b");
        var child3 = provider.CreateElement(null);
        child3.SetProperty("name", "c");

        element.AddToProperty("list", child1);
        element.AddToProperty("list", child2);
        element.AddToProperty("list", child3);

        var elementMovements = element as IProviderObjectSupportsListMovements;
        Assert.That(elementMovements, Is.Not.Null);

        var asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list"))!.OfType<IProviderObject>()
            .ToList();
        Assert.That(asEnumerable.Count, Is.EqualTo(3));

        Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("a"));
        Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("b"));
        Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));

        elementMovements!.MoveElementUp("list", child2);
        asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list"))!.OfType<IProviderObject>().ToList();
        Assert.That(asEnumerable.Count, Is.EqualTo(3));

        Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
        Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("a"));
        Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));

        elementMovements.MoveElementUp("list", child2);
        asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list"))!.OfType<IProviderObject>().ToList();
        Assert.That(asEnumerable.Count, Is.EqualTo(3));

        Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
        Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("a"));
        Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));


        elementMovements.MoveElementUp("list", child3);
        asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list"))!.OfType<IProviderObject>().ToList();
        Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
        Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("c"));
        Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("a"));

        elementMovements.MoveElementDown("list", child3);
        asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list"))!.OfType<IProviderObject>().ToList();
        Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
        Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("a"));
        Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));

        elementMovements.MoveElementDown("list", child3);
        asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list"))!.OfType<IProviderObject>().ToList();
        Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
        Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("a"));
        Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));

        elementMovements.MoveElementDown("list", child2);
        asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list"))!.OfType<IProviderObject>().ToList();
        Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("a"));
        Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("b"));
        Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));
    }

    public static void TestSetReferenceAndSetValue(IProvider provider)
    {
        var element = provider.CreateElement(null);
        var child1 = provider.CreateElement(null);
        child1.SetProperty("name", "Brenn");

        // Set the child element
        element.SetProperty("child", child1);

        var property = element.GetPropertyAsSingle("child") as IProviderObject;
        Assert.That(property, Is.Not.Null);
        Assert.That(property!.GetProperty("name"), Is.EqualTo("Brenn"));

        // Overwrite it with local element
        element.SetProperty("child", "123");

        var propertyString = element.GetProperty("child") as string;
        Assert.That(propertyString, Is.Not.Null);
        Assert.That(propertyString, Is.EqualTo("123"));

        // Write again the child element
        element.SetProperty("child", child1);

        property = element.GetPropertyAsSingle("child") as IProviderObject;
        Assert.That(property, Is.Not.Null);
        Assert.That(property!.GetProperty("name"), Is.EqualTo("Brenn"));

        /////////////
        // Now try it with Uri references
        element.SetProperty("child", new UriReference("http://abc"));
        var uriReference = element.GetPropertyAsSingle("child") as UriReference;
        Assert.That(uriReference, Is.Not.Null);
        Assert.That(uriReference!.Uri, Is.EqualTo("http://abc"));

        element.SetProperty("child", "123");

        propertyString = element.GetProperty("child") as string;
        Assert.That(propertyString, Is.Not.Null);
        Assert.That(propertyString, Is.EqualTo("123"));

        element.SetProperty("child", new UriReference("http://abc"));
        uriReference = element.GetPropertyAsSingle("child") as UriReference;
        Assert.That(uriReference, Is.Not.Null);
        Assert.That(uriReference!.Uri, Is.EqualTo("http://abc"));
    }
}