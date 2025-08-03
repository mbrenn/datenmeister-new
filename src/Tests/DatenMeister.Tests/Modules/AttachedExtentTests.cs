using DatenMeister.AttachedExtent;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;
using static DatenMeister.Core.Models._AttachedExtent;

namespace DatenMeister.Tests.Modules;

[TestFixture]
public class AttachedExtentTests
{
    [Test]
    public void TestRetrievalOfConfiguration()
    {
        // Prepare some stuff
        var workspaceLogic = WorkspaceLogic.Create(new WorkspaceData());
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
        var factory = new MofFactory(extent);
        var type = factory.create(_UML.TheOne.StructuredClassifiers.__Class);
        extent.elements().add(type);

        // Create the attachedconfiguration
        var attachedConfiguration = factory.create(
                _AttachedExtent.TheOne.__AttachedExtentConfiguration)
            .SetProperties(
                new Dictionary<string, object>
                {
                    ["name"] = "Test",
                    ["referencedExtent"] = "dm:///other",
                    ["referencedWorkspace"] = "Data",
                    ["referenceProperty"] = "referenceid",
                    ["referenceType"] = type
                });

        extent.set(
            AttachedExtentHandler.AttachedExtentProperty, attachedConfiguration);

        // Now test the attached handler
        var attachedHandler = new AttachedExtentHandler(workspaceLogic);
        var foundConfiguration = attachedHandler.GetConfiguration(extent);
        Assert.That(foundConfiguration, Is.Not.Null);
        Assert.That(foundConfiguration.getOrDefault<string>(_AttachedExtentConfiguration.name), Is.EqualTo("Test"));
        Assert.That(foundConfiguration.getOrDefault<string>(_AttachedExtentConfiguration.referencedExtent),
            Is.EqualTo("dm:///other"));
        Assert.That(foundConfiguration.getOrDefault<string>(_AttachedExtentConfiguration.referencedWorkspace),
            Is.EqualTo("Data"));
        Assert.That(foundConfiguration.getOrDefault<string>(_AttachedExtentConfiguration.referenceProperty),
            Is.EqualTo("referenceid"));
        Assert.That(foundConfiguration.getOrDefault<IElement>(_AttachedExtentConfiguration.referenceType),
            Is.EqualTo(type));
    }

    [Test]
    public void TestSetupCreation()
    {
        var setup = CreateTestSetup();
        Assert.That(setup, Is.Not.Null);
        Assert.That(setup.WorkspaceLogic, Is.Not.Null);
        Assert.That(setup.AttachedExtentHandler, Is.Not.Null);
        Assert.That(setup.OriginalExtent, Is.Not.Null);
        Assert.That(setup.AttachedExtent, Is.Not.Null);
        Assert.That(setup.AttachedExtent2, Is.Not.Null);
    }

    [Test]
    public void TestGetOriginalExtent()
    {
        var setup = CreateTestSetup();
        var originalExtent = setup.AttachedExtentHandler!.GetOriginalExtent(setup.AttachedExtent!);
        Assert.That(originalExtent, Is.Not.Null);
        Assert.That(originalExtent!.contextURI(), Is.EqualTo(setup.OriginalExtent!.contextURI()));
    }


    [Test]
    public void TestGetAttachedExtent()
    {
        var setup = CreateTestSetup();
        var attachedExtents =
            setup.AttachedExtentHandler!.FindAttachedExtents(setup.OriginalExtent!)
                .ToList();

        Assert.That(attachedExtents, Is.Not.Null);
        Assert.That(attachedExtents.Count, Is.EqualTo(2));
        Assert.That(attachedExtents.Any(x => x.contextURI() == setup.AttachedExtent!.contextURI()));
        Assert.That(attachedExtents.Any(x => x.contextURI() == setup.AttachedExtent2!.contextURI()));
    }

    [Test]
    public void TestAttachedItems()
    {
        var setup = CreateTestSetup();
        var originalItem1 = MofFactory.CreateElement(setup.OriginalExtent!, null);
        var originalItem2 = MofFactory.CreateElement(setup.OriginalExtent!, null);

        setup.OriginalExtent!.elements().add(originalItem1);
        setup.OriginalExtent.elements().add(originalItem2);

        var attached1 = setup.AttachedExtentHandler!.GetOrCreateAttachedItem(originalItem1, setup.AttachedExtent!);
        var attached2 = setup.AttachedExtentHandler.GetOrCreateAttachedItem(originalItem2, setup.AttachedExtent!);

        Assert.That(attached1, Is.Not.Null);
        Assert.That(attached2, Is.Not.Null);
        Assert.That(attached1.metaclass, Is.EqualTo(setup.ReferenceType));

        attached1.set("name", "Attached 1");
        attached2.set("name", "Attached 2");

        var attached3 = setup.AttachedExtentHandler.GetOrCreateAttachedItem(originalItem1, setup.AttachedExtent!);
        Assert.That(attached3, Is.Not.Null);
        Assert.That(attached3.getOrDefault<string>("name"), Is.EqualTo("Attached 1"));
    }

    public class TestSetup
    {
        public AttachedExtentHandler? AttachedExtentHandler { get; set; }

        public IElement? AttachedExtentConfiguration { get; set; }

        public IWorkspaceLogic? WorkspaceLogic { get; set; }

        public IUriExtent? TypeExtent { get; set; }

        public IElement? ReferenceType { get; set; }

        public IUriExtent? OriginalExtent { get; set; }

        public IUriExtent? AttachedExtent { get; set; }

        public IUriExtent? AttachedExtent2 { get; set; }

        public IUriExtent? NonConnectedExtent { get; set; }
    }

    public TestSetup CreateTestSetup()
    {
        var testSetup = new TestSetup
        {
            WorkspaceLogic = WorkspaceLogic.Create(new WorkspaceData()),
            TypeExtent = new MofUriExtent(new InMemoryProvider(), "dm:///types", null),
            OriginalExtent = new MofUriExtent(new InMemoryProvider(), "dm:///originalExtent", null),
            AttachedExtent = new MofUriExtent(new InMemoryProvider(), "dm:///attachedExtent", null),
            AttachedExtent2 = new MofUriExtent(new InMemoryProvider(), "dm:///attachedExtent2", null),
            NonConnectedExtent = new MofUriExtent(new InMemoryProvider(), "dm:///nonconnectedExtent", null)
        };

        var type = MofFactory.CreateElement(testSetup.TypeExtent, null);
        testSetup.TypeExtent.elements().add(type);
        type.set("name", "ReferenceType");
        testSetup.ReferenceType = type;

        var attachedHandler =
            testSetup.AttachedExtentHandler = new AttachedExtentHandler(testSetup.WorkspaceLogic);

        var dataWorkspace = new Workspace("Data");
        testSetup.WorkspaceLogic.AddWorkspace(dataWorkspace);
        testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.OriginalExtent);
        testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.AttachedExtent);
        testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.AttachedExtent2);
        testSetup.WorkspaceLogic.AddExtent(dataWorkspace, testSetup.NonConnectedExtent);

        var config = InMemoryObject.CreateEmpty(
                _AttachedExtent.TheOne.__AttachedExtentConfiguration)
            .SetProperties(
                new Dictionary<string, object>
                {
                    ["referencedExtent"] = "dm:///originalExtent",
                    ["referencedWorkspace"] = "Data",
                    ["referenceProperty"] = "reference",
                    ["referenceType"] = type
                });

        attachedHandler.SetConfiguration(testSetup.AttachedExtent, config);
        attachedHandler.SetConfiguration(testSetup.AttachedExtent2, config);
        return testSetup;
    }
}