using System.Reflection;
using System.Xml.Linq;
using DatenMeister.BootStrap;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Provider.Xmi.Provider.XMI;
using NUnit.Framework;

namespace DatenMeister.Tests.Xmi;

/// <summary>
/// Zusammenfassungsbeschreibung f�r MofObjectTests
/// </summary>
[TestFixture]
public class XmiTests
{
    [Test]
    public void LoadUmlInfrastructure()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///target", null);
        Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var factory = new MofFactory(extent);
        Assert.That(extent.elements().Count(), Is.EqualTo(0));
        var loader = new SimpleLoader();
        loader.LoadFromFile(factory, extent, "Xmi/UML.xmi");

        var firstElement = (extent.elements().ElementAt(0) as IObject);
        Assert.That(firstElement, Is.Not.Null);
        Assert.That(firstElement!.get("name")!.ToString(), Is.EqualTo("UML"));
    }

    [Test]
    public void TestIdValidity()
    {
        var document = XDocument.Load("Xmi/MOF.xmi");
        Assert.That(XmiId.IsValid(document), Is.True);

        // Adds an artificial node, which duplicates an id.
        document.Root!.Add(
            new XElement("other", new XAttribute(Namespaces.Xmi + "id", "_MOF-Identifiers-Extent")));

        Assert.That(XmiId.IsValid(document), Is.False);
    }

    [Test]
    public void TestGetUriAndRetrieveElement()
    {
        Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        var data = WorkspaceLogic.InitDefault();
        var dataLayerLogic = WorkspaceLogic.Create(data);

        var strapper = Bootstrapper.PerformFullBootstrap(dataLayerLogic,
            data.Uml,
            BootstrapMode.Mof,
            new Bootstrapper.FilePaths
            {
                PathPrimitive = "Xmi/PrimitiveTypes.xmi",
                PathUml = "Xmi/UML.xmi",
                PathMof = "Xmi/MOF.xmi"
            });
        var umlExtent = strapper.UmlInfrastructure;
        Assert.That(umlExtent, Is.Not.Null);
        var element = umlExtent!.elements().ElementAt(0) as IElement;
        Assert.That(element, Is.Not.Null);

        var elementUri = umlExtent.uri(element!);
        Assert.That(elementUri, Is.Not.Null);
        var foundElement = umlExtent.element(elementUri!);
        Assert.That(foundElement, Is.Not.Null);
        Assert.That(foundElement, Is.EqualTo(element));

        // Retrieve another element
        element = AllDescendentsQuery.GetDescendents(umlExtent).ElementAt(300) as IElement;
        Assert.That(element, Is.Not.Null);
        elementUri = umlExtent.uri(element!);
        Assert.That(elementUri, Is.Not.Null);
        foundElement = umlExtent.element(elementUri!);
        Assert.That(foundElement, Is.Not.Null);
        Assert.That(foundElement, Is.EqualTo(element));
    }

    [Test]
    public async Task TestThatPropertyIsIElement()
    {
        var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var comment = dm.WorkspaceLogic.GetUmlWorkspace()
                .Resolve(_UML.TheOne.CommonStructure.__Comment.GetUri()!, ResolveType.Default)
            as IElement;
        Assert.That(_UML.TheOne.CommonStructure.Comment._body, Is.Not.Null);

        var commentBody = dm.WorkspaceLogic.GetUmlWorkspace()
                .Resolve(_UML.TheOne.CommonStructure.Comment._body.GetUri()!, ResolveType.Default)
            as IElement;
        Assert.That(comment, Is.InstanceOf<IElement>());
        Assert.That(_UML._CommonStructure._Comment.body, Is.InstanceOf<string>());
        Assert.That(commentBody, Is.InstanceOf<IElement>());

        Assert.That(
            commentBody!
                .isSet(_UML._CommonStructure._NamedElement.name),
            Is.True);

        Assert.That(
            commentBody
                .get(_UML._CommonStructure._NamedElement.name),
            Is.Not.Null);
    }

    [Test]
    public async Task TestThatGeneralizationsAreOk()
    {
        var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var package = dm.WorkspaceLogic.GetUmlWorkspace()
                .Resolve(_UML.TheOne.Packages.__Package.GetUri()!, ResolveType.Default)
            as IElement;

        Assert.That(package, Is.Not.Null);

        // Old behavior
        IEnumerable<object> generalizedElements;
        string generalProperty;
        if (package!.isSet("generalization"))
        {
            generalizedElements = (package.get("generalization") as IEnumerable<object>)!.ToList();
            generalProperty = "general";
        }
        else
        {
            throw new InvalidOperationException("No generalizations currently found");
        }

        Assert.That(generalizedElements, Is.Not.Null);
        Assert.That(generalizedElements.All(x => x is IElement));
        Assert.That(generalizedElements.Count(), Is.EqualTo(3));

        var firstElement = generalizedElements.ElementAtOrDefault(0) as IElement;
        Assert.That(firstElement, Is.Not.Null);
        var generalContent = firstElement.getOrDefault<IElement>(generalProperty);
        Assert.That(generalContent, Is.InstanceOf<IElement>());
        Assert.That(generalContent.equals(_UML.TheOne.CommonStructure.__PackageableElement));
    }

    /// <summary>
    /// Creates a filled MOF and UML instance which can be used for further testing
    /// </summary>
    public static Bootstrapper CreateUmlAndMofInstance()
    {
        var data = WorkspaceLogic.InitDefault();
        var dataLayerLogic = WorkspaceLogic.Create(data);
        var strapper = Bootstrapper.PerformFullBootstrap(
            dataLayerLogic,
            data.Mof,
            BootstrapMode.Mof);
        Assert.That(strapper, Is.Not.Null);
        Assert.That(strapper.UmlInfrastructure, Is.Not.Null);

        Assert.That(
            AllDescendentsQuery.GetDescendents(strapper.UmlInfrastructure!).Count(),
            Is.GreaterThan(500));

        return strapper;
    }
}