using System.Xml.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Types.Actions;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Types;

[TestFixture]
public class TestMigrateAlternativeReferences
{
    public const string DmInternTypesDomainsDatenmeister = "dm:///intern.types.issues.datenmeister/";
    
    [Test]
    public async Task TestMigrate()
    {
        var xml = """
                  <?xml version="1.0" encoding="utf-8"?>
                  <xmi>
                    <meta p2:id="35a560b9-7b7d-433c-994b-400451643d50" p2:type="dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties" __ExtentType="IssueMeister " xmlns:p2="http://www.omg.org/spec/XMI/20131001" xmlns="http://datenmeister.net/" __AutoEnumerateType="Ordinal" __AutoEnumerateValue="2" __uri="dm:///issues" />
                    <item p2:type="dm:///intern.types.issues.datenmeister/#IssueMeister.Issue" p2:id="1" name="fdsafdsa" description="fdsafdsafdsa" xmlns:p2="http://www.omg.org/spec/XMI/20131001" />
                    <item p2:type="dm:///intern.types.issues.datenmeister/#IssueMeister.Issue" p2:id="2" name="fdsafdsafdsa" description="fdsafds" xmlns:p2="http://www.omg.org/spec/XMI/20131001" />
                    <item p2:type="dm:///_internal/types/internal#IssueMeister.Issue" p2:id="22" id="22" name="Moving of issues is not working" state="Closed" description="When moving issues, we get the following error message: &#xA;&#xA;228.160: [Error] DatenMeister.Actions.ActionLogic: System.ArgumentNullException: Value cannot be null. (Parameter 'property')&#xA;   at DatenMeister.Core.Provider.InMemory.InMemoryObject.MoveElementUp(String property, Object value) in C:\Projekte\datenmeister-new\src\DatenMeister.Core\Provider\InMemory\InMemoryObject.cs:line 137&#xA;   at DatenMeister.Core.Helper.CollectionHelper.MoveElementUp(IReflectiveSequence collection, IObject elementToBeMovedUp) in C:\Projekte\datenmeister-new\src\DatenMeister.Core\Helper\CollectionHelper.cs:line 137&#xA;   at DatenMeister.Actions.ActionHandler.MoveUpDownActionHandler.Evaluate(ActionLogic actionLogic, IElement action) in C:\Projekte\datenmeister-new\src\DatenMeister.Actions\ActionHandler\MoveUpDownActionHandler.cs:line 44&#xA;   at DatenMeister.Actions.ActionLogic.&lt;&gt;c__DisplayClass10_1.&lt;ExecuteAction&gt;b__0() in C:\Projekte\datenmeister-new\src\DatenMeister.Actions\ActionLogic.cs:line 102&#xA;" xmlns:p2="http://www.omg.org/spec/XMI/20131001" />
                  </xmi>
                  """;

        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "dm_test_extent.xmi");
        await File.WriteAllTextAsync(path, xml);
        
        var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var extentManager = dm.Resolve<ExtentManager>();
        
        var configuration = ExtentLoaderConfigs.XmiStorageLoaderConfig_Wrapper.Create(InMemoryObject.TemporaryFactory);
        configuration.extentUri = DmInternTypesDomainsDatenmeister;
        configuration.dropExisting = true;
        configuration.filePath = path;

        var result = await extentManager.LoadExtent(configuration.GetWrappedElement(), ExtentCreationFlags.LoadOrCreate);
        Assert.That(result.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
        var umlExtent = result.Extent as MofUriExtent;
        Assert.That(umlExtent, Is.Not.Null);

        MofUriExtent.Migration.AddConverter("dm:///_internal/types/internal#IssueMeister.Issue", DmInternTypesDomainsDatenmeister + "#IssueMeister.Issue");

        var provider = umlExtent!.Provider as XmiProvider;
        Assert.That(provider, Is.Not.Null);
                 
        XNamespace p2 = Namespaces.Xmi;
        var items = provider!.Document.Root?.Elements("item").ToList();
        Assert.That(items, Is.Not.Null);
        Assert.That(items!.Count, Is.EqualTo(3));
        
        Assert.That(items[0].Attribute(p2 + "type")?.Value, Is.EqualTo("dm:///intern.types.issues.datenmeister/#IssueMeister.Issue"));
        Assert.That(items[1].Attribute(p2 + "type")?.Value, Is.EqualTo("dm:///intern.types.issues.datenmeister/#IssueMeister.Issue"));
        Assert.That(items[2].Attribute(p2 + "type")?.Value, Is.EqualTo("dm:///_internal/types/internal#IssueMeister.Issue"));

        var action = new MigrateAlternativeTypeReferencesActionLogic(dm.WorkspaceLogic);
        await action.MigrateAsync();

        items = provider.Document.Root?.Elements("item").ToList();
        Assert.That(items, Is.Not.Null);
        Assert.That(items!.Count, Is.EqualTo(3));
        
        Assert.That(items[0].Attribute(p2 + "type")?.Value, Is.EqualTo("dm:///intern.types.issues.datenmeister/#IssueMeister.Issue"));
        Assert.That(items[1].Attribute(p2 + "type")?.Value, Is.EqualTo("dm:///intern.types.issues.datenmeister/#IssueMeister.Issue"));
        Assert.That(items[2].Attribute(p2 + "type")?.Value, Is.EqualTo("dm:///intern.types.issues.datenmeister/#IssueMeister.Issue"));
    }
}