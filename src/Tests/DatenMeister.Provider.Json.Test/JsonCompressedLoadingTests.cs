using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Provider.Json.Test;

[TestFixture]
public class JsonCompressedLoadingTests
{
  [Test]
  public void TestCompressedJsonSimple()
  {
    var provider = new InMemoryProvider();
    var extent = new MofUriExtent(provider, "dm:///test", null);

    var importer = new JsonCompressedImporter(new MofFactory(extent));
    importer.ImportFromText(SimpleCompressedJson, extent.elements());

    var elements = extent.elements().OfType<IElement>().ToList();
    Assert.That(elements.Count, Is.EqualTo(3));

    var secondElement = elements.ElementAt(1);
    Assert.That(secondElement, Is.Not.Null);
    Assert.That(secondElement.getOrDefault<string>("name"), Is.EqualTo("Little-Brenn"));
    Assert.That(secondElement.getOrDefault<string>("prename"), Is.EqualTo("Martinlein"));
    Assert.That(secondElement.getOrDefault<int>("age"), Is.EqualTo(13));
  }

  [Test]
  public void TestCompressedJsonComplex()
  {
    var provider = new InMemoryProvider();
    var extent = new MofUriExtent(provider, "dm:///test", null);

    var importer = new JsonCompressedImporter(new MofFactory(extent));
    importer.ImportFromText(ComplexCompressedJson, extent.elements());

    var elements = extent.elements().OfType<IElement>().ToList();
    Assert.That(elements.Count, Is.EqualTo(3));

    var secondElement = elements.ElementAt(1);
    Assert.That(secondElement, Is.Not.Null);
    Assert.That(secondElement.getOrDefault<string>("name"), Is.EqualTo("Little-Brenn"));
    Assert.That(secondElement.getOrDefault<string>("prename"), Is.EqualTo("Martinlein"));
    Assert.That(secondElement.getOrDefault<int>("age"), Is.EqualTo(13));

    var hobbies = secondElement.getOrDefault<IReflectiveCollection>("hobbies").ToList();
    Assert.That(hobbies.Count, Is.EqualTo(2));
    Assert.That(hobbies.OfType<string>().ElementAt(0), Is.EqualTo("gaming"));

    var address = secondElement.getOrDefault<IElement>("address");
    Assert.That(address, Is.Not.Null);
    Assert.That(
      address.getOrDefault<string>("town"), 
      Is.EqualTo("Offenbach"));
  }

  private const string SimpleCompressedJson =
    """
    {
      "columns": [
        "name",
        "prename",
        "age"
      ],
      "data": [
        [
          "Brenn",
          "Martin",
          42
        ],
        [
          "Little-Brenn",
          "Martinlein",
          13
        ],
        [
          "Spouse",
          "Might Exist",
          42
        ]
      ]
    }
    """;

  private const string ComplexCompressedJson =
    """
    {
      "columns": [
        "name",
        "prename",
        "age",
        "hobbies",
        "address"
      ],
      "data": [
        [
          "Brenn",
          "Martin",
          42,
          [
            "running",
            "walking",
            "talking"
          ],
          {
            "town": "Frankfurt",
            "street": "Mainzer Straße"
          }
        ],
        [
          "Little-Brenn",
          "Martinlein",
          13,
          [
            "gaming",
            "learning"
          ],
          {
            "town": "Offenbach",
            "street": "Frankfurter Allee"
          }
        ],
        [
          "Spouse",
          "Might Exist",
          42,
          [
            "painting",
            "powerrunning"
          ],
          {
            "town": "Wiesbaden",
            "street": "Offenbacher Tunnel"
          }
        ]
      ]
    }
    """;
}