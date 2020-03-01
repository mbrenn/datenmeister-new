using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
{
    /// <summary>
    /// This class contains several test helper which can be used to test the
    /// behavior of the provider interface
    /// </summary>
    public class ProviderTestHelper
    {
        public static void TestGetAndSetOfPrimitiveTypes(IProvider provider)
        {
            var providerObject = provider.CreateElement(null);
            
            providerObject.SetProperty("string", "abc");
            providerObject.SetProperty("double", 23.2342);
            providerObject.SetProperty("int", 232);

            Assert.That(
                DotNetHelper.AsString(
                    providerObject.GetProperty("string")), Is.EqualTo("abc"));
            
            Assert.That(
                DotNetHelper.AsDouble(
                    providerObject.GetProperty("double")), Is.EqualTo(23.2342).Within(0.0005));
            
            Assert.That(
                DotNetHelper.AsInteger(
                    providerObject.GetProperty("int")), Is.EqualTo(232));

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

            var asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
            Assert.That(asEnumerable.Count, Is.EqualTo(3));

            Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("a"));
            Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("b"));
            Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));

            element.RemoveFromProperty("list", child2);
            asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
            Assert.That(asEnumerable.Count, Is.EqualTo(2));
            
            Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("a"));
            Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("c"));

            element.EmptyListForProperty("list");
            asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
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

            var asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
            Assert.That(asEnumerable.Count, Is.EqualTo(3));

            Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("a"));
            Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("b"));
            Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));

            elementMovements.MoveElementUp("list", child2);
            asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
            Assert.That(asEnumerable.Count, Is.EqualTo(3));

            Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
            Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("a"));
            Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));
            
            elementMovements.MoveElementUp("list", child2);
            asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
            Assert.That(asEnumerable.Count, Is.EqualTo(3));

            Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
            Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("a"));
            Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));


            elementMovements.MoveElementUp("list", child3);
            asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
            Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
            Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("c"));
            Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("a"));
            
            elementMovements.MoveElementDown("list", child3);
            asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
            Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
            Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("a"));
            Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));
            
            elementMovements.MoveElementDown("list", child3);
            asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
            Assert.That(DotNetHelper.AsString(asEnumerable[0].GetProperty("name")), Is.EqualTo("b"));
            Assert.That(DotNetHelper.AsString(asEnumerable[1].GetProperty("name")), Is.EqualTo("a"));
            Assert.That(DotNetHelper.AsString(asEnumerable[2].GetProperty("name")), Is.EqualTo("c"));
            
            elementMovements.MoveElementDown("list", child2);
            asEnumerable = DotNetHelper.AsEnumeration(element.GetProperty("list")).OfType<IProviderObject>().ToList();
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
            Assert.That(property.GetProperty("name"), Is.EqualTo("Brenn"));

            // Overwrite it with local element
            element.SetProperty("child", "123");

            var propertyString = element.GetProperty("child") as string;
            Assert.That(propertyString, Is.Not.Null);
            Assert.That(propertyString, Is.EqualTo("123"));

            // Write again the child element
            element.SetProperty("child", child1);

            property = element.GetPropertyAsSingle("child") as IProviderObject;
            Assert.That(property, Is.Not.Null);
            Assert.That(property.GetProperty("name"), Is.EqualTo("Brenn"));

            /////////////
            // Now try it with Uri references
            element.SetProperty("child", new UriReference("http://abc"));
            var uriReference = element.GetPropertyAsSingle("child") as UriReference;
            Assert.That(uriReference, Is.Not.Null);
            Assert.That(uriReference.Uri, Is.EqualTo("http://abc"));
            
            
            
            element.SetProperty("child", "123");

             propertyString = element.GetProperty("child") as string;
            Assert.That(propertyString, Is.Not.Null);
            Assert.That(propertyString, Is.EqualTo("123"));
            
            element.SetProperty("child", new UriReference("http://abc"));
            uriReference = element.GetPropertyAsSingle("child") as UriReference;
            Assert.That(uriReference, Is.Not.Null);
            Assert.That(uriReference.Uri, Is.EqualTo("http://abc"));
            
            
        }
    }
}